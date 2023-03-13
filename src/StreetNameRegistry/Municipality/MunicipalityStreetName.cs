namespace StreetNameRegistry.Municipality
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using DataStructures;
    using Events;
    using Exceptions;

    public sealed partial class MunicipalityStreetName : Entity
    {
        public void Approve()
        {
            if (Status == StreetNameStatus.Current)
            {
                return;
            }

            GuardStreetNameStatus(StreetNameStatus.Proposed);

            Apply(new StreetNameWasApproved(_municipalityId, PersistentLocalId));
        }

        public void Reject()
        {
            if (Status == StreetNameStatus.Rejected)
            {
                return;
            }

            GuardStreetNameStatus(StreetNameStatus.Proposed);

            Apply(new StreetNameWasRejected(_municipalityId, PersistentLocalId));
        }

        public void Retire()
        {
            if (Status == StreetNameStatus.Retired)
            {
                return;
            }

            GuardStreetNameStatus(StreetNameStatus.Current);

            Apply(new StreetNameWasRetiredV2(_municipalityId, PersistentLocalId));
        }

        public void CorrectNames(Names names, Action<Names, HomonymAdditions, PersistentLocalId> guardStreetNameNames)
        {
            GuardStreetNameStatus(StreetNameStatus.Proposed, StreetNameStatus.Current);
            guardStreetNameNames(names, HomonymAdditions, PersistentLocalId);

            var correctedNames = new Names(names.Where(name => !Names.HasMatch(name.Language, name.Name)));
            if (!correctedNames.Any())
            {
                return;
            }

            Apply(new StreetNameNamesWereCorrected(_municipalityId, PersistentLocalId, correctedNames));
        }

        public void ChangeNames(Names names, Action<Names, HomonymAdditions, PersistentLocalId> guardStreetNameNames)
        {
            GuardStreetNameStatus(StreetNameStatus.Proposed, StreetNameStatus.Current);
            guardStreetNameNames(names, HomonymAdditions, PersistentLocalId);

            var newNames = new Names(names.Where(name => !Names.HasMatch(name.Language, name.Name)));
            if (!newNames.Any())
            {
                return;
            }

            Apply(new StreetNameNamesWereChanged(_municipalityId, PersistentLocalId, newNames));
        }

        public void CorrectApproval()
        {
            if (Status == StreetNameStatus.Proposed)
            {
                return;
            }

            GuardStreetNameStatus(StreetNameStatus.Current);

            Apply(new StreetNameWasCorrectedFromApprovedToProposed(_municipalityId, PersistentLocalId));
        }

        public void CorrectRejection(Action<Names, HomonymAdditions, PersistentLocalId> guardUniqueActiveStreetNameNames)
        {
            if (Status == StreetNameStatus.Proposed)
            {
                return;
            }

            GuardStreetNameStatus(StreetNameStatus.Rejected);
            guardUniqueActiveStreetNameNames(Names, HomonymAdditions, PersistentLocalId);

            Apply(new StreetNameWasCorrectedFromRejectedToProposed(_municipalityId, PersistentLocalId));
        }

        public void CorrectRetirement(Action<Names, HomonymAdditions, PersistentLocalId> guardUniqueActiveStreetNameNames)
        {
            if (Status == StreetNameStatus.Current)
            {
                return;
            }

            GuardStreetNameStatus(StreetNameStatus.Retired);
            guardUniqueActiveStreetNameNames(Names, HomonymAdditions, PersistentLocalId);

            Apply(new StreetNameWasCorrectedFromRetiredToCurrent(_municipalityId, PersistentLocalId));
        }

        public void CorrectHomonymAdditions(
            HomonymAdditions homonymAdditions,
            Action<Names, HomonymAdditions, PersistentLocalId> guardUniqueActiveStreetNameNames)
        {
            GuardStreetNameStatus(StreetNameStatus.Proposed, StreetNameStatus.Current);

            foreach (var item in homonymAdditions)
            {
                if (!HomonymAdditions.HasLanguage(item.Language))
                {
                    throw new CannotAddHomonymAdditionException(item.Language);
                }
            }

            guardUniqueActiveStreetNameNames(Names, homonymAdditions, PersistentLocalId);

            var changedHomonymAdditions = homonymAdditions.Except(HomonymAdditions).ToList();

            if (changedHomonymAdditions.Any())
            {
                Apply(new StreetNameHomonymAdditionsWereCorrected(_municipalityId, PersistentLocalId, changedHomonymAdditions));
            }
        }

        public void RemoveHomonymAdditions(
            List<Language> languages,
            Action<Names, HomonymAdditions, PersistentLocalId> guardUniqueActiveStreetNameNames)
        {
            GuardStreetNameStatus(StreetNameStatus.Proposed, StreetNameStatus.Current);

            var names =  new Names(Names.Where(x => languages.Contains(x.Language)));
            guardUniqueActiveStreetNameNames(names, new HomonymAdditions(), PersistentLocalId);

            var homonymAdditionsToRemove = languages.Where(x => HomonymAdditions.HasLanguage(x)).ToList();

            if (homonymAdditionsToRemove.Any())
            {
                Apply(new StreetNameHomonymAdditionsWereRemoved(_municipalityId, PersistentLocalId, homonymAdditionsToRemove));
            }
        }

        public void RestoreSnapshot(MunicipalityId municipalityId, StreetNameData streetNameData)
        {
            _municipalityId = municipalityId;

            PersistentLocalId = new PersistentLocalId(streetNameData.StreetNamePersistentLocalId);
            Status = streetNameData.Status;
            IsRemoved = streetNameData.IsRemoved;

            Names = new Names(streetNameData.Names);
            HomonymAdditions = new HomonymAdditions(streetNameData.HomonymAdditions);

            LegacyStreetNameId = streetNameData.LegacyStreetNameId is null
                ? null
                : new StreetNameId(streetNameData.LegacyStreetNameId.Value);

            _lastSnapshotEventHash = streetNameData.LastEventHash;
            _lastSnapshotProvenance = streetNameData.LastProvenanceData;
        }

        private void GuardStreetNameStatus(params StreetNameStatus[] validStatuses)
        {
            if (!validStatuses.Contains(Status))
            {
                throw new StreetNameHasInvalidStatusException(PersistentLocalId);
            }
        }
    }
}
