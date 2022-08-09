namespace StreetNameRegistry.Municipality
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Be.Vlaanderen.Basisregisters.AggregateSource.Snapshotting;
    using Events;
    using Exceptions;

    public partial class Municipality : AggregateRootEntity, ISnapshotable
    {
        public static Municipality Register(
            IMunicipalityFactory municipalityFactory,
            MunicipalityId municipalityId,
            NisCode nisCode)
        {
            var municipality = municipalityFactory.Create();
            municipality.ApplyChange(new MunicipalityWasImported(municipalityId, nisCode));
            return municipality;
        }

        public void ProposeStreetName(Names streetNameNames, PersistentLocalId persistentLocalId)
        {
            if (MunicipalityStatus == MunicipalityStatus.Retired)
            {
                throw new MunicipalityHasUnexpectedStatusException($"Municipality with id '{_municipalityId}' was retired");
            }

            foreach (var streetNameName in streetNameNames)
            {
                if (StreetNames.HasActiveStreetNameName(streetNameName))
                {
                    throw new StreetNameNameAlreadyExistsException(streetNameName.Name);
                }

                if (!_officialLanguages.Contains(streetNameName.Language)
                    && !_facilityLanguages.Contains(streetNameName.Language))
                {
                    throw new StreetNameNameLanguageNotSupportedException(
                        $"The language '{streetNameName.Language}' is not an official or facility language of municipality '{_municipalityId}'.");
                }
            }

            foreach (var language in _officialLanguages.Concat(_facilityLanguages))
            {
                if (!streetNameNames.HasLanguage(language))
                {
                    throw new StreetNameMissingLanguageException($"The language '{language}' is missing.");
                }
            }

            ApplyChange(new StreetNameWasProposedV2(_municipalityId, _nisCode, streetNameNames, persistentLocalId));
        }

        public void ApproveStreetName(PersistentLocalId persistentLocalId)
        {
            var streetName = StreetNames.FindByPersistentLocalId(persistentLocalId);

            if (streetName is null)
            {
                throw new StreetNameNotFoundException(persistentLocalId);
            }

            if (MunicipalityStatus != MunicipalityStatus.Current)
            {
                throw new MunicipalityHasUnexpectedStatusException();
            }

            streetName.Approve();
        }

        public void RejectStreetName(PersistentLocalId persistentLocalId)
        {
            var streetName = StreetNames.FindByPersistentLocalId(persistentLocalId);

            if (streetName is null)
            {
                throw new StreetNameNotFoundException(persistentLocalId);
            }

            if (MunicipalityStatus != MunicipalityStatus.Current)
            {
                throw new MunicipalityHasUnexpectedStatusException();
            }

            streetName.Reject();
        }

        public void RetireStreetName(PersistentLocalId persistentLocalId)
        {
            var streetName = StreetNames.FindByPersistentLocalId(persistentLocalId);

            if (streetName is null)
            {
                throw new StreetNameNotFoundException(persistentLocalId);
            }

            if (MunicipalityStatus != MunicipalityStatus.Current)
            {
                throw new MunicipalityHasUnexpectedStatusException();
            }

            streetName.Retire();
        }

        public void CorrectStreetNameName(Names streetNameNames, PersistentLocalId persistentLocalId)
        {
            var streetName = StreetNames.FindByPersistentLocalId(persistentLocalId);

            if (streetName is null)
            {
                throw new StreetNameNotFoundException(persistentLocalId);
            }

            foreach (var streetNameName in streetNameNames)
            {
                if (StreetNames.HasActiveStreetNameNameForOtherThan(streetNameName, persistentLocalId))
                {
                    throw new StreetNameNameAlreadyExistsException(streetNameName.Name);
                }

                if (!_officialLanguages.Contains(streetNameName.Language)
                    && !_facilityLanguages.Contains(streetNameName.Language))
                {
                    throw new StreetNameNameLanguageNotSupportedException(
                        $"The language '{streetNameName.Language}' is not an official or facility language of municipality '{_municipalityId}'.");
                }
            }

            streetName.CorrectNames(streetNameNames);
        }

        public void DefineOrChangeNisCode(NisCode nisCode)
        {
            if (string.IsNullOrWhiteSpace(nisCode))
            {
                throw new NoNisCodeException("NisCode of a municipality cannot be empty.");
            }

            if (nisCode != _nisCode)
            {
                ApplyChange(new MunicipalityNisCodeWasChanged(_municipalityId, nisCode));
            }
        }

        public void BecomeCurrent()
        {
            if (MunicipalityStatus != MunicipalityStatus.Current)
            {
                ApplyChange(new MunicipalityBecameCurrent(_municipalityId));
            }
        }

        public void CorrectToCurrent()
        {
            if (MunicipalityStatus != MunicipalityStatus.Current)
            {
                ApplyChange(new MunicipalityWasCorrectedToCurrent(_municipalityId));
            }
        }

        public void CorrectToRetired()
        {
            if (MunicipalityStatus != MunicipalityStatus.Retired)
            {
                ApplyChange(new MunicipalityWasCorrectedToRetired(_municipalityId));
            }
        }

        public void Retire()
        {
            if (MunicipalityStatus != MunicipalityStatus.Retired)
            {
                ApplyChange(new MunicipalityWasRetired(_municipalityId));
            }
        }

        public void NameMunicipality(MunicipalityName name)
        {
            ApplyChange(new MunicipalityWasNamed(_municipalityId, name));
        }

        public void AddOfficialLanguage(Language language)
        {
            if (!_officialLanguages.Contains(language))
            {
                ApplyChange(new MunicipalityOfficialLanguageWasAdded(_municipalityId, language));
            }
        }

        public void RemoveOfficialLanguage(Language language)
        {
            if (_officialLanguages.Contains(language))
            {
                ApplyChange(new MunicipalityOfficialLanguageWasRemoved(_municipalityId, language));
            }
        }

        public void AddFacilityLanguage(Language language)
        {
            if (!_facilityLanguages.Contains(language))
            {
                ApplyChange(new MunicipalityFacilityLanguageWasAdded(_municipalityId, language));
            }
        }

        public void RemoveFacilityLanguage(Language language)
        {
            if (_facilityLanguages.Contains(language))
            {
                ApplyChange(new MunicipalityFacilityLanguageWasRemoved(_municipalityId, language));
            }
        }

        public void MigrateStreetName(
            StreetNameId streetNameId,
            PersistentLocalId persistentLocalId,
            StreetNameStatus status,
            Language? primaryLanguage,
            Language? secondaryLanguage,
            Names names,
            HomonymAdditions homonymAdditions,
            bool isCompleted,
            bool isRemoved)
        {
            if (StreetNames.HasPersistentLocalId(persistentLocalId))
            {
                throw new InvalidOperationException(
                    $"Cannot migrate StreetName with id '{persistentLocalId}' in municipality '{_municipalityId}'.");
            }

            ApplyChange(new StreetNameWasMigratedToMunicipality(
                _municipalityId,
                _nisCode,
                streetNameId,
                persistentLocalId,
                status,
                primaryLanguage,
                secondaryLanguage,
                names,
                homonymAdditions,
                isCompleted,
                isRemoved));
        }

        public string GetStreetNameHash(PersistentLocalId persistentLocalId)
        {
            var streetName = StreetNames.FindByPersistentLocalId(persistentLocalId);
            if (streetName == null)
            {
                throw new AggregateSourceException($"Cannot find a streetname entity with id {persistentLocalId}");
            }

            return streetName.LastEventHash;
        }

        #region Metadata
        protected override void BeforeApplyChange(object @event)
        {
            new EventMetadataContext(new Dictionary<string, object>());
            base.BeforeApplyChange(@event);
        }

        #endregion

        public object TakeSnapshot()
        {
            return new MunicipalitySnapshot(
                MunicipalityId,
                _nisCode,
                MunicipalityStatus,
                _officialLanguages,
                _facilityLanguages,
                StreetNames);
        }

        public ISnapshotStrategy Strategy { get; }
    }
}
