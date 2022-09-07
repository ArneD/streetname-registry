namespace StreetNameRegistry.Api.BackOffice.Handlers.Sqs.Lambda.Requests
{
    using Abstractions;
    using Abstractions.Requests;
    using Municipality;
    using Municipality.Commands;

    public class SqsLambdaStreetNameRejectRequest :
        SqsLambdaRequest,
        IHasBackOfficeRequest<StreetNameBackOfficeRejectRequest>,
        IHasStreetNamePersistentLocalId
    {
        public StreetNameBackOfficeRejectRequest Request { get; set; }

        public int StreetNamePersistentLocalId => Request.PersistentLocalId;

        /// <summary>
        /// Map to RejectStreetName command
        /// </summary>
        /// <returns>RejectStreetName.</returns>
        public RejectStreetName ToCommand(PersistentLocalId streetNamePersistentLocalId)
        {
            return new RejectStreetName(MunicipalityId, streetNamePersistentLocalId, Provenance);
        }
    }
}
