namespace StreetNameRegistry.Consumer.Infrastructure
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Be.Vlaanderen.Basisregisters.Aws.DistributedMutex;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Be.Vlaanderen.Basisregisters.MessageHandling.Kafka;
    using Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer;
    using Destructurama;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Modules;
    using Serilog;
    using Serilog.Debugging;
    using Serilog.Extensions.Logging;
    using StreetNameRegistry.Infrastructure;
    using Consumer = StreetNameRegistry.Consumer.Consumer;

    public sealed class Program
    {
        protected Program()
        {
        }

        public static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
                Log.Debug(
                    eventArgs.Exception,
                    "FirstChanceException event raised in {AppDomain}.",
                    AppDomain.CurrentDomain.FriendlyName);

            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
                Log.Fatal((Exception)eventArgs.ExceptionObject, "Encountered a fatal exception, exiting program.");

            Log.Information("Initializing StreetNameRegistry.Consumer");

            var host = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configurationBuilder) =>
                {
                    configurationBuilder
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                        .AddJsonFile($"appsettings.{Environment.MachineName.ToLowerInvariant()}.json", optional: true, reloadOnChange: false)
                        .AddEnvironmentVariables()
                        .AddCommandLine(args);
                })
                .ConfigureLogging((hostContext, loggingBuilder) =>
                {
                    SelfLog.Enable(Console.WriteLine);

                    Log.Logger = new LoggerConfiguration() //NOSONAR logging configuration is safe
                        .ReadFrom.Configuration(hostContext.Configuration)
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .Enrich.WithThreadId()
                        .Enrich.WithEnvironmentUserName()
                        .Destructure.JsonNetTypes()
                        .CreateLogger();

                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddSerilog(Log.Logger);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var loggerFactory = new SerilogLoggerFactory(Log.Logger); //NOSONAR logging configuration is safe

                    services
                        .AddDbContextFactory<IdempotentConsumerContext>((_, options) => options
                            .UseLoggerFactory(loggerFactory)
                            .UseSqlServer(hostContext.Configuration.GetConnectionString("Consumer"),
                                sqlServerOptions =>
                                {
                                    sqlServerOptions.EnableRetryOnFailure();
                                    sqlServerOptions.MigrationsHistoryTable(MigrationTables.Consumer, Schema.Consumer);
                                }));

                    services
                        .AddDbContext<ConsumerContext>((_, options) => options
                            .UseLoggerFactory(loggerFactory)
                            .UseSqlServer(hostContext.Configuration.GetConnectionString("Consumer"), sqlServerOptions =>
                            {
                                sqlServerOptions.EnableRetryOnFailure();
                                sqlServerOptions.MigrationsHistoryTable(MigrationTables.ConsumerProjections, Schema.ConsumerProjections);
                            }))
                        .AddDbContextFactory<ConsumerContext>((_, options) => options
                            .UseLoggerFactory(loggerFactory)
                            .UseSqlServer(hostContext.Configuration.GetConnectionString("Consumer"), sqlServerOptions =>
                            {
                                sqlServerOptions.EnableRetryOnFailure();
                                sqlServerOptions.MigrationsHistoryTable(MigrationTables.ConsumerProjections, Schema.ConsumerProjections);
                            }));
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>((hostContext, containerBuilder) =>
                {
                    var services = new ServiceCollection();
                    var loggerFactory = new SerilogLoggerFactory(Log.Logger); //NOSONAR logging configuration is safe

                    containerBuilder.Register(_ =>
                    {
                        var bootstrapServers = hostContext.Configuration["Kafka:BootstrapServers"];
                        var topic = $"{hostContext.Configuration["MunicipalityTopic"]}" ?? throw new ArgumentException("Configuration has no MunicipalityTopic.");
                        var suffix = hostContext.Configuration["MunicipalityConsumerGroupSuffix"];
                        var consumerGroupId = $"StreetNameRegistry.Consumer.{topic}{suffix}";

                        var consumerOptions = new ConsumerOptions(
                            new BootstrapServers(bootstrapServers),
                            new Topic(topic),
                            new ConsumerGroupId(consumerGroupId),
                            EventsJsonSerializerSettingsProvider.CreateSerializerSettings());

                        consumerOptions.ConfigureSaslAuthentication(new SaslAuthentication(
                            hostContext.Configuration["Kafka:SaslUserName"],
                            hostContext.Configuration["Kafka:SaslPassword"]));

                        var offset = hostContext.Configuration["MunicipalityOffset"];

                        if (!string.IsNullOrWhiteSpace(offset) && long.TryParse(offset, out var result))
                            consumerOptions.ConfigureOffset(new Offset(result));

                        return consumerOptions;
                    });

                    containerBuilder
                        .RegisterType<IdempotentConsumer<IdempotentConsumerContext>>()
                        .As<IIdempotentConsumer<IdempotentConsumerContext>>()
                        .SingleInstance();

                    containerBuilder
                        .RegisterType<Consumer>()
                        .As<IHostedService>()
                        .SingleInstance();

                    containerBuilder.RegisterModule(new ApiModule(hostContext.Configuration, services, loggerFactory));

                    containerBuilder
                        .RegisterType<ConsumerProjections>()
                        .As<IHostedService>()
                        .SingleInstance();

                    containerBuilder.Populate(services);
                })
                .UseConsoleLifetime()
                .Build();

            Log.Information("Starting StreetNameRegistry.Consumer");

            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
            var configuration = host.Services.GetRequiredService<IConfiguration>();

            try
            {
                await DistributedLock<Program>.RunAsync(
                        async () =>
                        {
                            await MigrationsHelper.RunAsync(
                                configuration.GetConnectionString("ConsumerAdmin"),
                                loggerFactory,
                                CancellationToken.None);

                            await host.RunAsync().ConfigureAwait(false);
                        },
                        DistributedLockOptions.LoadFromConfiguration(configuration),
                        logger)
                    .ConfigureAwait(false);
            }
            catch (AggregateException aggregateException)
            {
                foreach (var innerException in aggregateException.InnerExceptions)
                {
                    logger.LogCritical(innerException, "Encountered a fatal exception, exiting program.");
                }
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Encountered a fatal exception, exiting program.");
                Log.CloseAndFlush();

                // Allow some time for flushing before shutdown.
                await Task.Delay(500, default);
                throw;
            }
            finally
            {
                logger.LogInformation("Stopping...");
            }
        }
    }
}
