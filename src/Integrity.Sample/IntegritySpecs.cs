using System;
using System.IO;
using System.Collections.Generic;
using Xunit;
using Integrity;
using Integrity.TestingHelpers;

static class AppComponents 
{
    static readonly string _deploymentPath = Environment.GetEnvironmentVariable("INTEGRITY_SAMPLE");

    public static IEnumerable<PathItem> FileSystem
    {
        get {
            yield return PathItem.File(Path.Combine(_deploymentPath, "empty0"));
            yield return PathItem.File(Path.Combine(_deploymentPath, "empty1"));
            yield return PathItem.File(Path.Combine(_deploymentPath, "empty2"));
            yield return PathItem.Directory(Path.Combine(_deploymentPath, "empty"));
        }
    }

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
            new PathExistence(AppComponents.FileSystem),
            new HostAvailability(AppComponents.Network));

        var evidence = await context.ProveAsync();

        evidence.ShouldBeProved();
    }
}