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
        var sut = new HostAvailability(mock, new Hosts().AddHost(fakeHost).Content);

        var outcome = await sut.VerifyAsync();

        outcome.Should().NotBeNull()
            .And.BeOfType<Ok<Evidence, string>>()
            .Which.SucceededWith().Should().BeEquivalentTo(new Evidence(typeof(HostAvailability),
                                                                        new Hosts().AddHost(fakeHost).Content));
    }
}
