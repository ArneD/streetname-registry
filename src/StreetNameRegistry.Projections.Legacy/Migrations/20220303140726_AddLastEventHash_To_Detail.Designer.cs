﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StreetNameRegistry.Projections.Legacy;

namespace StreetNameRegistry.Projections.Legacy.Migrations
{
    [DbContext(typeof(LegacyContext))]
    [Migration("20220303140726_AddLastEventHash_To_Detail")]
    partial class AddLastEventHash_To_Detail
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner.ProjectionStates.ProjectionStateItem", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DesiredState")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DesiredStateChangedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.HasKey("Name")
                        .IsClustered();

                    b.ToTable("ProjectionStates", "StreetNameRegistryLegacy");
                });

            modelBuilder.Entity("StreetNameRegistry.Projections.Legacy.StreetNameDetail.StreetNameDetail", b =>
                {
                    b.Property<Guid>("StreetNameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Complete")
                        .HasColumnType("bit");

                    b.Property<string>("HomonymAdditionDutch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionFrench")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionGerman")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameDutch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameFrench")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameGerman")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NisCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<bool>("Removed")
                        .HasColumnType("bit");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("VersionTimestamp");

                    b.HasKey("StreetNameId")
                        .IsClustered(false);

                    b.HasIndex("PersistentLocalId")
                        .IsUnique()
                        .HasDatabaseName("IX_StreetNameDetails_PersistentLocalId_1")
                        .HasFilter("([PersistentLocalId] IS NOT NULL)")
                        .IsClustered(false);

                    b.HasIndex("Removed");

                    b.ToTable("StreetNameDetails", "StreetNameRegistryLegacy");
                });

            modelBuilder.Entity("StreetNameRegistry.Projections.Legacy.StreetNameDetailV2.StreetNameDetailV2", b =>
                {
                    b.Property<int>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<string>("HomonymAdditionDutch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionFrench")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionGerman")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastEventHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("MunicipalityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NameDutch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameFrench")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameGerman")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NisCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Removed")
                        .HasColumnType("bit");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("VersionTimestamp");

                    b.HasKey("PersistentLocalId")
                        .IsClustered();

                    b.HasIndex("MunicipalityId");

                    b.HasIndex("Removed");

                    b.ToTable("StreetNameDetailsV2", "StreetNameRegistryLegacy");
                });

            modelBuilder.Entity("StreetNameRegistry.Projections.Legacy.StreetNameList.StreetNameListItem", b =>
                {
                    b.Property<Guid>("StreetNameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Complete")
                        .HasColumnType("bit");

                    b.Property<string>("HomonymAdditionDutch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionFrench")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionGerman")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameDutch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameDutchSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEnglishSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameFrench")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameFrenchSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameGerman")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameGermanSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NisCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<int?>("PrimaryLanguage")
                        .HasColumnType("int");

                    b.Property<bool>("Removed")
                        .HasColumnType("bit");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("VersionTimestamp");

                    b.HasKey("StreetNameId")
                        .IsClustered(false);

                    b.HasIndex("NameDutchSearch");

                    b.HasIndex("NameEnglishSearch");

                    b.HasIndex("NameFrenchSearch");

                    b.HasIndex("NameGermanSearch");

                    b.HasIndex("NisCode");

                    b.HasIndex("PersistentLocalId")
                        .IsUnique()
                        .HasDatabaseName("IX_StreetNameList_PersistentLocalId_1")
                        .HasFilter("([PersistentLocalId] IS NOT NULL)")
                        .IsClustered(false);

                    b.HasIndex("Status");

                    b.HasIndex("Complete", "Removed");

                    b.HasIndex("Complete", "Removed", "PersistentLocalId");

                    b.ToTable("StreetNameList", "StreetNameRegistryLegacy");
                });

            modelBuilder.Entity("StreetNameRegistry.Projections.Legacy.StreetNameList.StreetNameListViewCount", b =>
                {
                    b.Property<long>("Count")
                        .HasColumnType("bigint");

                    b.ToView("vw_StreetNameListIds", "StreetNameRegistryLegacy");
                });

            modelBuilder.Entity("StreetNameRegistry.Projections.Legacy.StreetNameListV2.StreetNameListItemV2", b =>
                {
                    b.Property<int>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<string>("HomonymAdditionDutch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionFrench")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionGerman")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("MunicipalityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NameDutch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameDutchSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEnglishSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameFrench")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameFrenchSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameGerman")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameGermanSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NisCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("PrimaryLanguage")
                        .HasColumnType("int");

                    b.Property<bool>("Removed")
                        .HasColumnType("bit");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("VersionTimestamp");

                    b.HasKey("PersistentLocalId")
                        .IsClustered();

                    b.HasIndex("MunicipalityId");

                    b.HasIndex("NameDutchSearch");

                    b.HasIndex("NameEnglishSearch");

                    b.HasIndex("NameFrenchSearch");

                    b.HasIndex("NameGermanSearch");

                    b.HasIndex("NisCode");

                    b.HasIndex("Removed");

                    b.HasIndex("Status");

                    b.ToTable("StreetNameListV2", "StreetNameRegistryLegacy");
                });

            modelBuilder.Entity("StreetNameRegistry.Projections.Legacy.StreetNameListV2.StreetNameListMunicipality", b =>
                {
                    b.Property<Guid>("MunicipalityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NisCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PrimaryLanguage")
                        .HasColumnType("int");

                    b.HasKey("MunicipalityId")
                        .IsClustered();

                    b.ToTable("StreetNameListMunicipality", "StreetNameRegistryLegacy");
                });

            modelBuilder.Entity("StreetNameRegistry.Projections.Legacy.StreetNameName.StreetNameName", b =>
                {
                    b.Property<Guid>("StreetNameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Complete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFlemishRegion")
                        .HasColumnType("bit");

                    b.Property<string>("NameDutch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameDutchSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameEnglish")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameEnglishSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameFrench")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameFrenchSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameGerman")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameGermanSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NisCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<bool>("Removed")
                        .HasColumnType("bit");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("VersionTimestamp");

                    b.HasKey("StreetNameId")
                        .IsClustered(false);

                    b.HasIndex("NameDutch");

                    b.HasIndex("NameDutchSearch");

                    b.HasIndex("NameEnglish");

                    b.HasIndex("NameEnglishSearch");

                    b.HasIndex("NameFrench");

                    b.HasIndex("NameFrenchSearch");

                    b.HasIndex("NameGerman");

                    b.HasIndex("NameGermanSearch");

                    b.HasIndex("NisCode");

                    b.HasIndex("PersistentLocalId")
                        .IsClustered();

                    b.HasIndex("Status");

                    b.HasIndex("VersionTimestampAsDateTimeOffset");

                    b.HasIndex("Removed", "IsFlemishRegion", "Complete");

                    b.ToTable("StreetNameName", "StreetNameRegistryLegacy");
                });

            modelBuilder.Entity("StreetNameRegistry.Projections.Legacy.StreetNameNameV2.StreetNameNameV2", b =>
                {
                    b.Property<int>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<bool>("IsFlemishRegion")
                        .HasColumnType("bit");

                    b.Property<Guid>("MunicipalityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NameDutch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameDutchSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameEnglish")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameEnglishSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameFrench")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameFrenchSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameGerman")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameGermanSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NisCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Removed")
                        .HasColumnType("bit");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("VersionTimestamp");

                    b.HasKey("PersistentLocalId")
                        .IsClustered();

                    b.HasIndex("MunicipalityId");

                    b.HasIndex("NameDutch");

                    b.HasIndex("NameDutchSearch");

                    b.HasIndex("NameEnglish");

                    b.HasIndex("NameEnglishSearch");

                    b.HasIndex("NameFrench");

                    b.HasIndex("NameFrenchSearch");

                    b.HasIndex("NameGerman");

                    b.HasIndex("NameGermanSearch");

                    b.HasIndex("NisCode");

                    b.HasIndex("Status");

                    b.HasIndex("VersionTimestampAsDateTimeOffset");

                    b.HasIndex("Removed", "IsFlemishRegion");

                    b.ToTable("StreetNameNameV2", "StreetNameRegistryLegacy");
                });

            modelBuilder.Entity("StreetNameRegistry.Projections.Legacy.StreetNameSyndication.StreetNameSyndicationItem", b =>
                {
                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.Property<int?>("Application")
                        .HasColumnType("int");

                    b.Property<string>("ChangeType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventDataAsXml")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionDutch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionFrench")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionGerman")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("LastChangedOnAsDateTimeOffset")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("LastChangedOn");

                    b.Property<int?>("Modification")
                        .HasColumnType("int");

                    b.Property<Guid?>("MunicipalityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NameDutch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameFrench")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameGerman")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NisCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Operator")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Organisation")
                        .HasColumnType("int");

                    b.Property<int?>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("RecordCreatedAtAsDateTimeOffset")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("RecordCreatedAt");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<Guid?>("StreetNameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("SyndicationItemCreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Position")
                        .IsClustered();

                    b.HasIndex("PersistentLocalId");

                    b.HasIndex("Position")
                        .HasDatabaseName("CI_StreetNameSyndication_Position")
                        .HasAnnotation("SqlServer:ColumnStoreIndex", "");

                    b.HasIndex("StreetNameId");

                    b.ToTable("StreetNameSyndication", "StreetNameRegistryLegacy");
                });
#pragma warning restore 612, 618
        }
    }
}
