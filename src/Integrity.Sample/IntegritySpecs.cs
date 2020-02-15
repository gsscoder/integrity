using System;
using System.Collections.Generic;
using Xunit;
using Integrity;
using Integrity.TestingHelpers;

class MyApplication : Application 
{
    public Paths FileSystem => Paths
        .SetBasePath(Environment.GetEnvironmentVariable("INTEGRITY_SAMPLE"))
        .AddFile("empty0")
        .AddFile("empty1")
        .AddFile("empty2")
        .AddDirectory("empty");

    public Hosts Network => Hosts
        .AddHost("github.com")
        .AddHost("google.com");
}

public class IntegritySpecs
{
    [Fact]
    public async void Prove_integrity()
    {
        var context = new MyApplication().ToProvingContext();

        var evidence = await context.ProveAsync();

        evidence.ShouldBeProved();
    }
}