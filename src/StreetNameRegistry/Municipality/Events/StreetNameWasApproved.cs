namespace StreetNameRegistry.Municipality.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;
    using Newtonsoft.Json;

    [EventTags(EventTag.For.Sync)]
    [EventName(EventName)]
    [EventDescription("De straatnaam werd goedgekeurd.")]
    public class StreetNameWasApproved : IMunicipalityEvent
    {
        public const string EventName = "StreetNameWasApproved"; // BE CAREFUL CHANGING THIS!!

        [EventPropertyDescription("Interne GUID van de gemeente.")]
        public Guid MunicipalityId { get; }
        public int PersistentLocalId { get; }
        public ProvenanceData Provenance { get; private set; }

        public StreetNameWasApproved(
            MunicipalityId municipalityId,
            PersistentLocalId persistentLocalId)
        {
            MunicipalityId = municipalityId;
            PersistentLocalId = persistentLocalId;
        }

        [JsonConstructor]
        private StreetNameWasApproved(
            Guid municipalityId,
            int persistentLocalId,
            ProvenanceData provenance
        ) :
            this(
                new MunicipalityId(municipalityId),
                new PersistentLocalId(persistentLocalId))
            => ((ISetProvenance)this).SetProvenance(provenance.ToProvenance());

        void ISetProvenance.SetProvenance(Provenance provenance) => Provenance = new ProvenanceData(provenance);

        public IEnumerable<string> GetHashFields()
        {
            var fields = Provenance.GetHashFields().ToList();
            fields.Add(MunicipalityId.ToString("D"));
            fields.Add(PersistentLocalId.ToString());
            return fields;
        }

        public string GetHash() => this.ToHash(EventName);
    }
}