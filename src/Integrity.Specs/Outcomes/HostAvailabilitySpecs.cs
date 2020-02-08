using Xunit;
using FluentAssertions;
using CSharpx;
using RailwaySharp;
using Integrity;
using Integrity.TestingHelpers;

public class HostAvailabilitySpecs
{
    [Fact]
    public async void Should_prove_host_availability_if_exists()
    {
        const string fakeHost = "fake.org"; 
        var mock = new MockNetwork(ArbitraryNetwork.Hosts(
                samples: Maybe.Nothing<ushort>(), fakeHost));
        var sut = new HostAvailability(mock, new string[] { fakeHost });

        var outcome = await sut.VerifyAsync();

        outcome.Should().NotBeNull()
            .And.BeOfType<Ok<Evidence, string>>()
            .Which.SucceededWith().Should().BeEquivalentTo(new Evidence(typeof(HostAvailability),
                                                                        new string[] { fakeHost }));
    }
}