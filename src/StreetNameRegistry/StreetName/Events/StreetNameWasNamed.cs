namespace StreetNameRegistry.StreetName.Events
{
    using System;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Newtonsoft.Json;
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;

    [EventName("StreetNameWasNamed")]
    [EventDescription("De straatnaam werd benoemd.")]
    public class StreetNameWasNamed : IHasStreetNameId, IHasProvenance, ISetProvenance
    {
        public Guid StreetNameId { get; }

        public string Name { get; }
        public Language? Language { get; }
        public ProvenanceData Provenance { get; private set; }

        public StreetNameWasNamed(
            StreetNameId streetNameId,
            StreetNameName name)
        {
            StreetNameId = streetNameId;
            Name = name.Name;
            Language = name.Language;
        }

        [JsonConstructor]
        private StreetNameWasNamed(
            Guid streetNameId,
            string name,
            Language? language,
            ProvenanceData provenance) :
            this(
                new StreetNameId(streetNameId),
                new StreetNameName(name, language)) => ((ISetProvenance)this).SetProvenance(provenance.ToProvenance());

        void ISetProvenance.SetProvenance(Provenance provenance) => Provenance = new ProvenanceData(provenance);
    }
}
