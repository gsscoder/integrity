using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;
using RailwaySharp;
using Integrity;

public class ProvingContextSpecs
{
    [Fact]
    public async void Should_produce_evidences_if_providers_and_dependencies_are_satisfied()
    {
        var mock1 = new MockEvidenceProvider(fail: false, Enumerable.Empty<EvidenceProvider>());
        var mock2 = new MockEvidenceProvider2(fail: false, new EvidenceProvider[] { mock1 });
        var mock3 = new MockEvidenceProvider3(fail: false, Enumerable.Empty<EvidenceProvider>());
        var sut = new ProvingContext(mock2, mock3);

        var outcome = await sut.ProveAsync() as Ok<IEnumerable<Evidence>, string>;

        outcome.Should().NotBeNull();
        outcome.Success.Should().NotBeNullOrEmpty()
            .And.HaveCount(3)
            .And.SatisfyRespectively(
                item => item.Should()
                    .BeEquivalentTo(new Evidence(typeof(MockEvidenceProvider), false)),
                item => item.Should()
                    .BeEquivalentTo(new Evidence(typeof(MockEvidenceProvider2), false)),
                item => item.Should()
                    .BeEquivalentTo(new Evidence(typeof(MockEvidenceProvider3), false)));
    }

    [Fact]
    public async void Should_produce_evidences_if_providers_with_nested_dependencies_are_satisfied()
    {
        var mock1 = new MockEvidenceProvider(fail: false, Enumerable.Empty<EvidenceProvider>());
        var mock2 = new MockEvidenceProvider2(fail: false, new EvidenceProvider[] { mock1 });
        var mock3 = new MockEvidenceProvider3(fail: false, new EvidenceProvider[] { mock2 });
        var sut = new ProvingContext(mock3);

        var outcome = await sut.ProveAsync() as Ok<IEnumerable<Evidence>, string>;

        outcome.Should().NotBeNull();
        outcome.Success.Should().NotBeNullOrEmpty()
            .And.HaveCount(3)
            .And.SatisfyRespectively(
                item => item.Should()
                    .BeEquivalentTo(new Evidence(typeof(MockEvidenceProvider), false)),
                item => item.Should()
                    .BeEquivalentTo(new Evidence(typeof(MockEvidenceProvider2), false)),
                item => item.Should()
                    .BeEquivalentTo(new Evidence(typeof(MockEvidenceProvider3), false)));
    }

    [Fact]
    public async void Should_not_produce_evidences_if_providers_and_dependencies_are_not_satisfied()
    {
        var mock1 = new MockEvidenceProvider(fail: true, Enumerable.Empty<EvidenceProvider>());
        var mock2 = new MockEvidenceProvider2(fail: true, Enumerable.Empty<EvidenceProvider>());
        var mock3 = new MockEvidenceProvider3(fail: false, new EvidenceProvider[] { mock1, mock2 });
        var sut = new ProvingContext(mock3);

        var outcome = await sut.ProveAsync() as Bad<IEnumerable<Evidence>, string>;

        outcome.Should().NotBeNull();
        outcome.Messages.Should().NotBeNullOrEmpty()
            .And.HaveCount(2)
            .And.SatisfyRespectively(
                item => item.Should().Be("Evidence not proven."),
                item => item.Should().Be("Evidence 2 not proven."));
    }

    [Fact]
    public async void Should_not_produce_evidences_if_providers_and_nested_dependencies_are_not_satisfied()
    {
        var mock1 = new MockEvidenceProvider(fail: true, Enumerable.Empty<EvidenceProvider>());
        var mock2 = new MockEvidenceProvider2(fail: true, new EvidenceProvider[] { mock1 });
        var mock3 = new MockEvidenceProvider3(fail: false, new EvidenceProvider[] { mock2 });
        var sut = new ProvingContext(mock3);

        var outcome = await sut.ProveAsync() as Bad<IEnumerable<Evidence>, string>;

        outcome.Should().NotBeNull();
        outcome.Messages.Should().NotBeNullOrEmpty()
            .And.HaveCount(2)
            .And.SatisfyRespectively(
                item => item.Should().Be("Evidence not proven."),
                item => item.Should().Be("Evidence 2 not proven."));
    }
}
