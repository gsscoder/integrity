# Integrity

Experimental .NET library to verify integrity of a system.

## Philosophy

The idea behind Integrity is to allow testability of a system deployment. It's a library that can be used by any .NET language, but aims to become a **C# DSL**.

## Targets

- .NET Standard 2.0
- .NET Framework 4.6.1

## Build and sample

```sh
# clone the repository
$ git clone https://github.com/gsscoder/integrity.git

# build the package
$ cd integrity/src/Integrity
$ dotnet build -c release

# execute sample
$ cd integrity/src/Integrity.Sample
$ dotnet build -c debug
$ env INTEGRITY_SAMPLE="/absolute/path/to/integrity/src/Integrity.Sample/data" dotnet test
...
Test Run Successful.
Total tests: 1
     Passed: 1
 Total time: 2.5333 Seconds
```

## At a glance

**CSharp:**

```csharp
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
        // Setup the proving context
        var context = new MyApplication().ToProvingContext();

        // Prove the system integrity
        var evidence = await context.ProveAsync();

        // Verify the outcome
        evidence.ShouldBeProved();
    }
}
```

## Libraries

- [System.IO.Abstractions](https://github.com/System-IO-Abstractions/System.IO.Abstractions)
- [RailwaySharp](https://github.com/gsscoder/railwaysharp)
- [CSharpx](https://github.com/gsscoder/csharpx)
- [xUnit.net](https://github.com/xunit/xunit)
- [FluentAssertions](https://github.com/fluentassertions/fluentassertions)
- [Bogus](https://github.com/bchavez/Bogus)

## Tools

- [Paket](https://github.com/fsprojects/Paket)

### Notes

- Integrity is still under development. Library has few `EvidenceProvider` and API could change.
