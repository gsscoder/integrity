using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using CSharpx;
using Bogus.DataSets;
using BogusSystem = Bogus.DataSets.System;
using Integrity.TestingHelpers;

static class ArbitraryNetwork
{
    static readonly Random _random = new CryptoRandom();
    static readonly BogusSystem _system = new BogusSystem();

    public static IEnumerable<MockHost> Hosts(Maybe<ushort> samples, params string[] additionals)
    {
        var number = samples.FromJust(() => (ushort)_random.Next(1, 10));
        var generated = new List<string>(number + additionals.Count());
        var enumerator = additionals.GetEnumerator();
        for (var i = 0; i < number; i++) {
            generated.Add(GenerateHost());
            if (enumerator.MoveNext()) generated.Add((string)enumerator.Current);
        }
        return from name in generated select new MockHost(name);
        string GenerateHost() {
            var path = new Internet().DomainWord();
            if (generated.Contains(path)) return GenerateHost();
            return path;
        }
    }
}