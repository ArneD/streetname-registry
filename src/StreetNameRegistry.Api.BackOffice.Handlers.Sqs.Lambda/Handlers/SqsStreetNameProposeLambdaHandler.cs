namespace StreetNameRegistry.Api.BackOffice.Handlers.Sqs.Lambda.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Municipality;
    using Municipality.Exceptions;
    using Requests;
    using TicketingService.Abstractions;

    public class SqsStreetNameProposeLambdaHandler : SqsLambdaHandler<SqsLambdaStreetNameProposeRequest>
    {
        private readonly IPersistentLocalIdGenerator _persistentLocalIdGenerator;
        private readonly BackOfficeContext _backOfficeContext;

        public SqsStreetNameProposeLambdaHandler(
            ITicketing ticketing,
            IPersistentLocalIdGenerator persistentLocalIdGenerator,
            IIdempotentCommandHandler idempotentCommandHandler,
            BackOfficeContext backOfficeContext,
            IMunicipalities municipalities
            ) : base(municipalities, ticketing, idempotentCommandHandler)
        {
            _persistentLocalIdGenerator = persistentLocalIdGenerator;
            _backOfficeContext = backOfficeContext;
        }

        protected override async Task<string> InnerHandle(SqsLambdaStreetNameProposeRequest request, CancellationToken cancellationToken)
        {
            var persistentLocalId = _persistentLocalIdGenerator.GenerateNextPersistentLocalId();

            var cmd = request.ToCommand(persistentLocalId);

            await IdempotentCommandHandler.Dispatch(
                cmd.CreateCommandId(),
                cmd,
                request.Metadata,
                cancellationToken);

            // Insert PersistentLocalId with MunicipalityId
            await _backOfficeContext
                .MunicipalityIdByPersistentLocalId
                .AddAsync(new MunicipalityIdByPersistentLocalId(persistentLocalId, request.MunicipalityId), cancellationToken);
            await _backOfficeContext.SaveChangesAsync(cancellationToken);

            var streetNameHash = await GetStreetNameHash(request.MunicipalityId, persistentLocalId, cancellationToken);

            return streetNameHash;
        }

        protected override TicketError? MapDomainException(DomainException exception)
        {
            return exception switch
            {
                StreetNameNameAlreadyExistsException nameExists => new TicketError(
                    ValidationErrorMessages.StreetName.StreetNameAlreadyExists(nameExists.Name),
                    ValidationErrorCodes.StreetName.StreetNameAlreadyExists),
                MunicipalityHasInvalidStatusException => new TicketError(
                    ValidationErrorMessages.Municipality.MunicipalityHasInvalidStatus,
                    ValidationErrorCodes.Municipality.MunicipalityHasInvalidStatus),
                StreetNameNameLanguageIsNotSupportedException _ => new TicketError(
                    ValidationErrorMessages.StreetName.StreetNameNameLanguageIsNotSupported,
                    ValidationErrorCodes.StreetName.StreetNameNameLanguageIsNotSupported),
                StreetNameIsMissingALanguageException _ => new TicketError(
                    ValidationErrorMessages.StreetName.StreetNameIsMissingALanguage,
                    ValidationErrorCodes.StreetName.StreetNameIsMissingALanguage),
                _ => null
            };
        }
    }
}