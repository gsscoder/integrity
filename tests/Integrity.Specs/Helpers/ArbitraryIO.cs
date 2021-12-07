using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using CSharpx;
using Bogus.DataSets;
using BogusSystem = Bogus.DataSets.System;

static class ArbitraryIO
{
    static readonly Random _random = new CryptoRandom();
    static readonly BogusSystem _system = new BogusSystem();
    static readonly Lorem _text = new Lorem();

    public static IEnumerable<string> FilePaths(Maybe<ushort> samples, params string[] additionals) =>
        Paths(() => _system.FilePath(), samples, additionals);

    public static IEnumerable<string> DirectoryPaths(Maybe<ushort> samples, params string[] additionals) =>
        Paths(() => _system.DirectoryPath(), samples, additionals);

    public static IFileSystem Files(IEnumerable<string> paths)
    {
        var files = new Dictionary<string, MockFileData>();
        foreach (var path in paths) {
            files.Add(path, new MockFileData(_text.Sentence()));
        }
        return new MockFileSystem(files);
    }

    public static IFileSystem Directories(IEnumerable<string> paths)
    {
        var mock = new MockFileSystem();
        AddDirectories(mock, paths.ToArray());
        return mock;
    }

    public static void AddDirectories(MockFileSystem mock, params string[] paths)
    {
        foreach (var path in paths) mock.AddDirectory(path);
    }

    static IEnumerable<string> Paths(Func<string> generator, Maybe<ushort> samples, string[] additionals)
    {
        var number = samples.FromJust(() => (ushort)_random.Next(1, 10));
        var generated = new List<string>(number + additionals.Count());
        var enumerator = additionals.GetEnumerator();
        for (var i = 0; i < number; i++) {
            generated.Add(GeneratePath());
            if (enumerator.MoveNext()) generated.Add((string)enumerator.Current);
        }
        return generated;
        string GeneratePath() {
            var path = generator();
            if (generated.Contains(path)) return GeneratePath();
            return path;
        }
    }
}
