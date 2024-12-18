namespace StreetNameRegistry.Tests.AggregateTests.WhenChangingMunicipalityNisCode
{
    using System.Collections.Generic;
    using AutoFixture;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Be.Vlaanderen.Basisregisters.AggregateSource.Snapshotting;
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using FluentAssertions;
    using global::AutoFixture;
    using Municipality;
    using Municipality.Commands;
    using Municipality.Events;
    using Municipality.Exceptions;
    using Testing;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class GivenMunicipality : StreetNameRegistryTest
    {
        private readonly MunicipalityStreamId _streamId;

        public GivenMunicipality(ITestOutputHelper output) : base(output)
        {
            Fixture.Customize(new InfrastructureCustomization());
            Fixture.Customize(new WithFixedMunicipalityId());
            _streamId = Fixture.Create<MunicipalityStreamId>();
        }

        [Fact]
        public void ThenNisCodeChanged()
        {
            var command = Fixture.Create<ChangeMunicipalityNisCode>();

            Assert(new Scenario()
                .Given(_streamId,
                    Fixture.Create<MunicipalityWasImported>())
                .When(command)
                .Then(new Fact(_streamId,
                    new MunicipalityNisCodeWasChanged(command.MunicipalityId, command.NisCode, []))));
        }

        [Fact]
        public void ThenNisCodeChangedToNisCodeRonse()
        {
            var nisCode = new NisCode("45041");
            var command = Fixture.Create<ChangeMunicipalityNisCode>()
                .WithNisCode(nisCode);

            var streetNameWasProposedV2 = Fixture.Create<StreetNameWasProposedV2>();

            Assert(new Scenario()
                .Given(_streamId,
                    Fixture.Create<MunicipalityWasImported>(),
                    streetNameWasProposedV2)
                .When(command)
                .Then(new[]
                {
                    new Fact(_streamId, new MunicipalityNisCodeWasChanged(command.MunicipalityId, nisCode, [new PersistentLocalId(streetNameWasProposedV2.PersistentLocalId)]))
                }));
        }

        [Fact]
        public void WithNoNisCode_ThenThrowsNoNisCodeHasNoValueException()
        {
            var command = Fixture.Create<ChangeMunicipalityNisCode>()
                .WithNisCode(null!);

            Assert(new Scenario()
                .Given(_streamId,
                    Fixture.Create<MunicipalityWasImported>())
                .When(command)
                .Throws(new NoNisCodeHasNoValueException("NisCode of a municipality cannot be empty.")));
        }

        [Fact]
        public void WithTheSameNisCode_ThenNone()
        {
            var municipalityWasImported = Fixture.Create<MunicipalityWasImported>();
            var command = Fixture.Create<ChangeMunicipalityNisCode>()
                .WithNisCode(new NisCode(municipalityWasImported.NisCode));

            Assert(new Scenario()
                .Given(_streamId, municipalityWasImported)
                .When(command)
                .ThenNone());
        }

        [Fact]
        public void StateCheck()
        {
            var aggregate = new MunicipalityFactory(NoSnapshotStrategy.Instance).Create();
            var nisCode = Fixture.Create<NisCode>();

            var municipalityWasImported = Fixture.Create<MunicipalityWasImported>();

            aggregate.Initialize(new List<object>
            {
                municipalityWasImported
            });

            // Act
            aggregate.DefineOrChangeNisCode(nisCode);

            // Assert
            municipalityWasImported.NisCode.Should().NotBe(nisCode);
            aggregate.NisCode.Should().Be(nisCode);
        }
    }

    public static class MunicipalityNisCodeWasChangedExtensions
    {
        public static ChangeMunicipalityNisCode WithNisCode(
            this ChangeMunicipalityNisCode command,
            NisCode nisCode)
        {
            return new ChangeMunicipalityNisCode(new MunicipalityId(command.MunicipalityId), nisCode,
                command.Provenance);
        }
    }
}
