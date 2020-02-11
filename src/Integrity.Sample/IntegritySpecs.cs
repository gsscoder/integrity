using System;
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

    public static Hosts Network => new Hosts()
        .AddHost("github.com")
        .AddHost("google.com");
        
}

public class IntegritySpecs
{
    [Fact]
    public async void Prove_integrity()
    {
        var context = new ProvingContext(
            new PathExistence(AppComponents.FileSystem.Content),
            new HostAvailability(AppComponents.Network.Content));

        var evidence = await context.ProveAsync();

        evidence.ShouldBeProved();
    }
}