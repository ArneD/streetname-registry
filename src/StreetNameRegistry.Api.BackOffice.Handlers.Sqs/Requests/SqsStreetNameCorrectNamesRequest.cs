namespace StreetNameRegistry.Api.BackOffice.Handlers.Sqs.Requests
{
    using Abstractions.Requests;

    public class SqsStreetNameCorrectNamesRequest : SqsRequest, IHasBackOfficeRequest<StreetNameBackOfficeCorrectNamesRequest>
    {
        public int PersistentLocalId { get; set; }

        public StreetNameBackOfficeCorrectNamesRequest Request { get; set; }
    }
}
