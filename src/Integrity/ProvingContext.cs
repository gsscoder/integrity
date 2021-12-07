using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using RailwaySharp;

namespace Integrity
{
    public sealed class ProvingContext
    {
        public ProvingContext(params EvidenceProvider[] providers)
        {
            Providers = providers;
        }

        public async Task<Result<IEnumerable<Evidence>, string>> ProveAsync() =>
            (await Task.WhenAll(
                from provider in Providers select provider.ProveAsync())).SelectMany(e => e).Collect();

        public IEnumerable<EvidenceProvider> Providers { get; private set;}
    }
}
