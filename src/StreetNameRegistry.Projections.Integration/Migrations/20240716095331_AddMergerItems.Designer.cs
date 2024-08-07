﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StreetNameRegistry.Projections.Integration;

#nullable disable

namespace StreetNameRegistry.Projections.Integration.Migrations
{
    [DbContext(typeof(IntegrationContext))]
    [Migration("20240716095331_AddMergerItems")]
    partial class AddMergerItems
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner.ProjectionStates.ProjectionStateItem", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("DesiredState")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("DesiredStateChangedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("text");

                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.HasKey("Name");

                    b.ToTable("ProjectionStates", "integration_streetname");
                });

            modelBuilder.Entity("StreetNameRegistry.Projections.Integration.Merger.StreetNameMergerItem", b =>
                {
                    b.Property<int>("NewPersistentLocalId")
                        .HasColumnType("integer")
                        .HasColumnName("new_persistent_local_id");

                    b.Property<int>("MergedPersistentLocalId")
                        .HasColumnType("integer")
                        .HasColumnName("merged_persistent_local_id");

                    b.HasKey("NewPersistentLocalId", "MergedPersistentLocalId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("MergedPersistentLocalId");

                    b.HasIndex("NewPersistentLocalId");

                    b.ToTable("streetname_merger_items", "integration_streetname");
                });

            modelBuilder.Entity("StreetNameRegistry.Projections.Integration.StreetNameLatestItem", b =>
                {
                    b.Property<int>("PersistentLocalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("persistent_local_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PersistentLocalId"));

                    b.Property<string>("HomonymAdditionDutch")
                        .HasColumnType("text")
                        .HasColumnName("homonym_addition_dutch");

                    b.Property<string>("HomonymAdditionEnglish")
                        .HasColumnType("text")
                        .HasColumnName("homonym_addition_english");

                    b.Property<string>("HomonymAdditionFrench")
                        .HasColumnType("text")
                        .HasColumnName("homonym_addition_french");

                    b.Property<string>("HomonymAdditionGerman")
                        .HasColumnType("text")
                        .HasColumnName("homonym_addition_german");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("boolean")
                        .HasColumnName("is_removed");

                    b.Property<Guid>("MunicipalityId")
                        .HasColumnType("uuid")
                        .HasColumnName("municipality_id");

                    b.Property<string>("NameDutch")
                        .HasColumnType("text")
                        .HasColumnName("name_dutch");

                    b.Property<string>("NameEnglish")
                        .HasColumnType("text")
                        .HasColumnName("name_english");

                    b.Property<string>("NameFrench")
                        .HasColumnType("text")
                        .HasColumnName("name_french");

                    b.Property<string>("NameGerman")
                        .HasColumnType("text")
                        .HasColumnName("name_german");

                    b.Property<string>("Namespace")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("namespace");

                    b.Property<string>("NisCode")
                        .HasColumnType("text")
                        .HasColumnName("nis_code");

                    b.Property<string>("OsloStatus")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("oslo_status");

                    b.Property<string>("Puri")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("puri");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<string>("VersionAsString")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("version_as_string");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("version_timestamp");

                    b.HasKey("PersistentLocalId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("IsRemoved");

                    b.HasIndex("MunicipalityId");

                    b.HasIndex("NameDutch");

                    b.HasIndex("NameEnglish");

                    b.HasIndex("NameFrench");

                    b.HasIndex("NameGerman");

                    b.HasIndex("NisCode");

                    b.HasIndex("OsloStatus");

                    b.HasIndex("PersistentLocalId");

                    b.HasIndex("Status");

                    b.HasIndex("IsRemoved", "Status");

                    b.ToTable("streetname_latest_items", "integration_streetname");
                });

            modelBuilder.Entity("StreetNameRegistry.Projections.Integration.StreetNameVersion", b =>
                {
                    b.Property<long>("Position")
                        .HasColumnType("bigint")
                        .HasColumnName("position");

                    b.Property<int>("PersistentLocalId")
                        .HasColumnType("integer")
                        .HasColumnName("persistent_local_id");

                    b.Property<string>("CreatedOnAsString")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedOnTimestampAsDateTimeOffset")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on_timestamp");

                    b.Property<string>("HomonymAdditionDutch")
                        .HasColumnType("text")
                        .HasColumnName("homonym_addition_dutch");

                    b.Property<string>("HomonymAdditionEnglish")
                        .HasColumnType("text")
                        .HasColumnName("homonym_addition_english");

                    b.Property<string>("HomonymAdditionFrench")
                        .HasColumnType("text")
                        .HasColumnName("homonym_addition_french");

                    b.Property<string>("HomonymAdditionGerman")
                        .HasColumnType("text")
                        .HasColumnName("homonym_addition_german");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("boolean")
                        .HasColumnName("is_removed");

                    b.Property<Guid>("MunicipalityId")
                        .HasColumnType("uuid")
                        .HasColumnName("municipality_id");

                    b.Property<string>("NameDutch")
                        .HasColumnType("text")
                        .HasColumnName("name_dutch");

                    b.Property<string>("NameEnglish")
                        .HasColumnType("text")
                        .HasColumnName("name_english");

                    b.Property<string>("NameFrench")
                        .HasColumnType("text")
                        .HasColumnName("name_french");

                    b.Property<string>("NameGerman")
                        .HasColumnType("text")
                        .HasColumnName("name_german");

                    b.Property<string>("Namespace")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("namespace");

                    b.Property<string>("NisCode")
                        .HasColumnType("text")
                        .HasColumnName("nis_code");

                    b.Property<string>("OsloStatus")
                        .HasColumnType("text")
                        .HasColumnName("oslo_status");

                    b.Property<string>("Puri")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("puri");

                    b.Property<int?>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<Guid?>("StreetNameId")
                        .HasColumnType("uuid")
                        .HasColumnName("streetname_id");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<string>("VersionAsString")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("version_as_string");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("version_timestamp");

                    b.HasKey("Position", "PersistentLocalId");

                    b.HasIndex("IsRemoved");

                    b.HasIndex("MunicipalityId");

                    b.HasIndex("NisCode");

                    b.HasIndex("OsloStatus");

                    b.HasIndex("PersistentLocalId");

                    b.HasIndex("Status");

                    b.HasIndex("StreetNameId");

                    b.HasIndex("Type");

                    b.HasIndex("VersionTimestampAsDateTimeOffset");

                    b.ToTable("streetname_versions", "integration_streetname");
                });
#pragma warning restore 612, 618
        }
    }
}
