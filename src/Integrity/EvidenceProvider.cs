using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpx;
using RailwaySharp;

namespace Integrity
{
    public abstract class EvidenceProvider
    {
        public EvidenceProvider(IEnumerable<EvidenceProvider> dependencies)
        {
            Guard.AgainstNull(nameof(dependencies), dependencies);

            Dependencies = dependencies;
        } 

        public IEnumerable<EvidenceProvider> Dependencies { get; private set;}

        /// <summary>Verifies this evidence without taking care of dependencies.</summary>
        /// <remarks>This method should not be called directly, except for testing purpose.</remarks>
        public abstract Task<Result<Evidence, string>> VerifyAsync();

        internal async Task<IEnumerable<Result<Evidence, string>>> ProveAsync() =>
            (await Task.WhenAll(from provider in Dependencies select provider.ProveAsync()))
                .SelectMany(e => e).Concat(await VerifyAsync());
    }
}
