namespace StreetNameRegistry.Municipality
{
    using System;
    using Be.Vlaanderen.Basisregisters.AggregateSource;

    public sealed class MunicipalityId : GuidValueObject<MunicipalityId>
    {
        public static MunicipalityId CreateFor(string municipalityId)
            => new MunicipalityId(Guid.Parse(municipalityId));

        public MunicipalityId(Guid municipalityId) : base(municipalityId) { }
    }
}
