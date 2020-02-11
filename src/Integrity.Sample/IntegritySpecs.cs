using System;
using System.IO;
using System.Collections.Generic;
using Xunit;
using Integrity;
using Integrity.TestingHelpers;

static class AppComponents 
{
    static readonly string _deploymentPath = Environment.GetEnvironmentVariable("INTEGRITY_SAMPLE");

    public static Paths FileSystem => new Paths()
        .AddFile("empty0")
        .AddFile("empty1")
        .AddFile("empty2")
        .AddDirectory("empty");

    public static IEnumerable<string> Network
    {
        get {
            yield return "github.com";
            yield return "google.com";
        }
    }
}

public class IntegritySpecs
{
    [Fact]
    public async void Prove_integrity()
    {
        var context = new ProvingContext(
            new PathExistence(AppComponents.FileSystem.Value),
            new HostAvailability(AppComponents.Network));

        var evidence = await context.ProveAsync();

        evidence.ShouldBeProved();
    }
}