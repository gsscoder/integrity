using System.Linq;
using System.IO.Abstractions.TestingHelpers;
using Xunit;
using FluentAssertions;
using RailwaySharp;
using CSharpx;
using Integrity;

public class PathExistenceSpecs
{
    [Fact]
    public async void Should_prove_file_existence_if_exists()
    {
        const string fakePath = "/var/tests/fake.txt";
        var mock = ArbitraryIO.Files(ArbitraryIO.FilePaths(
            samples: Maybe.Nothing<ushort>(), fakePath));
        var sut = new PathExistence(mock, new Paths().AddFile(fakePath).Content);

        var outcome = await sut.VerifyAsync();

        outcome.Should().NotBeNull()
            .And.BeOfType<Ok<Evidence, string>>()
            .Which.SucceededWith().Should().BeEquivalentTo(new Evidence(typeof(PathExistence),
                                                                        new Paths().AddFile(fakePath).Content));
    }

    [Fact]
    public async void Should_prove_directory_existence_if_exists()
    {
        const string fakePath = "/var/tests";
        var mock = ArbitraryIO.Directories(ArbitraryIO.DirectoryPaths(
            samples: Maybe.Nothing<ushort>(), fakePath));
        var sut = new PathExistence(mock, new Paths().AddDirectory(fakePath).Content);

        var outcome = await sut.VerifyAsync();

        outcome.Should().NotBeNull()
            .And.BeOfType<Ok<Evidence, string>>()
            .Which.SucceededWith().Should().BeEquivalentTo(new Evidence(typeof(PathExistence),
                                                                        new Paths().AddDirectory(fakePath).Content));
    }

    [Fact]
    public async void Should_not_prove_file_existence_if_does_not_exists()
    {
        const string fakePath = "/var/tests/fake.txt";
        var mock = ArbitraryIO.Files(ArbitraryIO.FilePaths(samples: Maybe.Nothing<ushort>()));
        var sut = new PathExistence(mock, new Paths().AddFile(fakePath).Content);

        var outcome = await sut.VerifyAsync();

        outcome.Should().NotBeNull()
            .And.BeOfType<Bad<Evidence, string>>()
            .Which.FailedWith().Single().Should().Be($"{fakePath} file is not found.");
    }

    [Fact]
    public async void Should_not_prove_directory_existence_if_does_not_exists()
    {
        const string fakePath = "/var/tests";
        var mock = ArbitraryIO.Directories(ArbitraryIO.DirectoryPaths(samples: Maybe.Nothing<ushort>()));
        var sut = new PathExistence(mock, new Paths().AddDirectory(fakePath).Content);

        var outcome = await sut.VerifyAsync();

        outcome.Should().NotBeNull()
            .And.BeOfType<Bad<Evidence, string>>()
            .Which.FailedWith().Single().Should().Be($"{fakePath} directory is not found.");
    }

    [Fact]
    public async void Should_prove_files_and_dirs_existence_if_exist()
    {
        const string fakeFilePath = "/var/tests/fake.txt";
        const string fakeDirectoryPath = "/var/tests";
        var mock = ArbitraryIO.Files(ArbitraryIO.FilePaths(
            samples: Maybe.Nothing<ushort>(), fakeFilePath));
        ArbitraryIO.AddDirectories((MockFileSystem)mock, fakeDirectoryPath);
        var paths = new Paths().AddFile(fakeFilePath).AddDirectory(fakeDirectoryPath).Content;
        var sut = new PathExistence(mock, paths);

        var outcome = await sut.VerifyAsync();

        outcome.Should().NotBeNull()
            .And.BeOfType<Ok<Evidence, string>>()
            .Which.SucceededWith().Should().BeEquivalentTo(new Evidence(typeof(PathExistence),
                                                                        paths));
    }
}
