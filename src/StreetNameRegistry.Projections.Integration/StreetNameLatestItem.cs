namespace StreetNameRegistry.Projections.Integration
{
    using System;
    using Be.Vlaanderen.Basisregisters.GrAr.Common;
    using Be.Vlaanderen.Basisregisters.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Municipality;
    using NodaTime;
    using StreetNameRegistry.Infrastructure;

    public sealed class StreetNameLatestItem
    {
        public const string VersionTimestampBackingPropertyName = nameof(VersionTimestampAsDateTimeOffset);

        public int PersistentLocalId { get; set; }
        public Guid MunicipalityId { get; set; }
        public StreetNameStatus Status { get; set; }
        public string OsloStatus { get; set; }

        public string? NisCode { get; set; }

        public string? NameDutch { get; set; }
        public string? NameFrench { get; set; }
        public string? NameGerman { get; set; }
        public string? NameEnglish { get; set; }

        public string? HomonymAdditionDutch { get; set; }
        public string? HomonymAdditionFrench { get; set; }
        public string? HomonymAdditionGerman { get; set; }
        public string? HomonymAdditionEnglish { get; set; }

        public bool IsRemoved { get; set; }

        public string Puri { get; set; }
        public string Namespace { get; set; }
        public string VersionAsString { get; set; }
        private DateTimeOffset VersionTimestampAsDateTimeOffset { get; set; }

        public Instant VersionTimestamp
        {
            get => Instant.FromDateTimeOffset(VersionTimestampAsDateTimeOffset);
            set
            {
                VersionTimestampAsDateTimeOffset = value.ToDateTimeOffset();
                VersionAsString = new Rfc3339SerializableDateTimeOffset(value.ToBelgianDateTimeOffset()).ToString();
            }
        }

        public StreetNameLatestItem()
        {  }
    }

    public sealed class StreetNameLatestItemConfiguration : IEntityTypeConfiguration<StreetNameLatestItem>
    {
        internal const string TableName = "streetname_latest_items";

        public void Configure(EntityTypeBuilder<StreetNameLatestItem> builder)
        {
            builder.ToTable(TableName, Schema.Integration)
                .HasKey(x => x.PersistentLocalId)
                .IsClustered();
            builder.Property(x => x.PersistentLocalId).HasColumnName("persistent_local_id");

            builder.Property(x => x.MunicipalityId).HasColumnName("municipality_id");
            builder.Property(x => x.NisCode).HasColumnName("nis_code");
            builder.Property(x => x.Status).HasColumnName("status");
            builder.Property(x => x.OsloStatus).HasColumnName("oslo_status");

            builder.Property(x => x.NameDutch).HasColumnName("name_dutch");
            builder.Property(x => x.NameFrench).HasColumnName("name_french");
            builder.Property(x => x.NameGerman).HasColumnName("name_german");
            builder.Property(x => x.NameEnglish).HasColumnName("name_english");

            builder.Property(x => x.HomonymAdditionDutch).HasColumnName("homonym_addition_dutch");
            builder.Property(x => x.HomonymAdditionFrench).HasColumnName("homonym_addition_french");
            builder.Property(x => x.HomonymAdditionGerman).HasColumnName("homonym_addition_german");
            builder.Property(x => x.HomonymAdditionEnglish).HasColumnName("homonym_addition_english");

            builder.Property(x => x.IsRemoved).HasColumnName("is_removed");

            builder.Property(x => x.Puri).HasColumnName("puri");
            builder.Property(x => x.Namespace).HasColumnName("namespace");
            builder.Property(x => x.VersionAsString).HasColumnName("version_as_string");
            builder.Property(StreetNameLatestItem.VersionTimestampBackingPropertyName).HasColumnName("version_timestamp");

            builder.Ignore(x => x.VersionTimestamp);

            builder.HasIndex(x => x.PersistentLocalId);
            builder.HasIndex(x => x.MunicipalityId);
            builder.HasIndex(x => x.NisCode);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.OsloStatus);
            builder.HasIndex(x => x.NameDutch);
            builder.HasIndex(x => x.NameEnglish);
            builder.HasIndex(x => x.NameFrench);
            builder.HasIndex(x => x.NameGerman);
            builder.HasIndex(x => x.IsRemoved);
            builder.HasIndex(x => new { x.IsRemoved, x.Status });
        }
    }
}
