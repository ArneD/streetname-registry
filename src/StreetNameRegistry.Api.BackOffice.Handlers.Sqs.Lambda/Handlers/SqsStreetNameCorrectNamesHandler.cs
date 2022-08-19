namespace StreetNameRegistry.Api.BackOffice.Handlers.Sqs.Lambda.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions.Requests;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency;
    using Municipality;
    using Municipality.Commands;
    using TicketingService.Abstractions;
    using MunicipalityId = Municipality.MunicipalityId;

    public class SqsStreetNameCorrectNamesHandler : SqsLambdaHandler<SqsStreetNameCorrectNamesRequest>
    {
        private readonly IMunicipalities _municipalities;
        private readonly IdempotencyContext _idempotencyContext;

        public SqsStreetNameCorrectNamesHandler(
            ITicketing ticketing,
            ITicketingUrl ticketingUrl,
            ICommandHandlerResolver bus,
            IMunicipalities municipalities,
            IdempotencyContext idempotencyContext)
            : base(ticketing, ticketingUrl, bus)
        {
            _municipalities = municipalities;
            _idempotencyContext = idempotencyContext;
        }

        protected override async Task<string> InnerHandle(SqsStreetNameCorrectNamesRequest request, CancellationToken cancellationToken)
        {
            var municipalityId = new MunicipalityId(Guid.Parse(request.MessageGroupId));
            var streetNamePersistentLocalId = new PersistentLocalId(request.PersistentLocalId);

            var cmd = request.ToCommand(
                municipalityId,
                CreateFakeProvenance());

            await IdempotentCommandHandlerDispatch(
                _idempotencyContext,
                cmd.CreateCommandId(),
                cmd,
                request.Metadata,
                cancellationToken);

            var lastEventHash = await GetStreetNameHash(_municipalities, municipalityId, streetNamePersistentLocalId, cancellationToken);

            return lastEventHash;
        }
    }
}