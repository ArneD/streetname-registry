namespace StreetNameRegistry.Projections.LastChangedList.Console.Infrastructure.Modules
{
    using System;
    using StreetNameRegistry.Infrastructure;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Be.Vlaanderen.Basisregisters.DataDog.Tracing.Microsoft;
    using Be.Vlaanderen.Basisregisters.DependencyInjection;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Be.Vlaanderen.Basisregisters.EventHandling.Autofac;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.LastChangedList;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore.Autofac;
    using Be.Vlaanderen.Basisregisters.Projector;
    using Be.Vlaanderen.Basisregisters.Projector.ConnectedProjections;
    using Be.Vlaanderen.Basisregisters.Projector.Modules;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using LastChangedListContextMigrationFactory = LastChangedList.LastChangedListContextMigrationFactory;

    public class LastChangedListConsoleModule : Module
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceCollection _services;
        private readonly ILoggerFactory _loggerFactory;

        public LastChangedListConsoleModule(
            IConfiguration configuration,
            IServiceCollection services,
            ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _services = services;
            _loggerFactory = loggerFactory;
        }

        protected override void Load(ContainerBuilder builder)
        {
            _services.RegisterModule(new DataDogModule(_configuration));

            RegisterProjectionSetup(builder);

            builder
                .RegisterType<ProblemDetailsHelper>()
                .AsSelf();

            builder.Populate(_services);
        }

        private void RegisterProjectionSetup(ContainerBuilder builder)
        {
            builder
                .RegisterModule(
                    new EventHandlingModule(
                        typeof(DomainAssemblyMarker).Assembly,
                        EventsJsonSerializerSettingsProvider.CreateSerializerSettings()))
                .RegisterModule<EnvelopeModule>()
                .RegisterEventstreamModule(_configuration)
                .RegisterModule(new ProjectorModule(_configuration));

            RegisterProjections(builder);
        }

        private void RegisterProjections(ContainerBuilder builder)
        {
            var logger = _loggerFactory.CreateLogger<LastChangedListConsoleModule>();
            var connectionString = _configuration.GetConnectionString("LastChangedList");

            builder.RegisterModule(new LastChangedListModule(connectionString, _configuration["DataDog:ServiceName"], _services, _loggerFactory));

            logger.LogInformation(
                "Added {Context} to services:" +
                Environment.NewLine +
                "\tSchema: {Schema}" +
                Environment.NewLine +
                "\tTableName: {TableName}",
                nameof(LastChangedListContext), LastChangedListContext.Schema, LastChangedListContext.MigrationsHistoryTable);

            builder.Register(c =>
                    new LastChangedListStreetNameCacheValidator(
                        _configuration.GetConnectionString("LegacyProjections"),
                        "StreetNameRegistry.Projections.Legacy.StreetNameDetailV2.StreetNameDetailProjectionsV2"))
                .AsSelf();

            builder
                .RegisterProjectionMigrator<LastChangedListContextMigrationFactory>(
                    _configuration,
                    _loggerFactory)
                .RegisterProjectionMigrator<DataMigrationContextMigrationFactory>(
                    _configuration,
                    _loggerFactory)
                .RegisterProjections<LastChangedProjections, LastChangedListContext>(
                    context => new LastChangedProjections(context.Resolve<LastChangedListStreetNameCacheValidator>()),
                    ConnectedProjectionSettings.Default);
        }
    }
}