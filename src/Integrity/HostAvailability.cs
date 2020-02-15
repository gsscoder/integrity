using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using RailwaySharp;

namespace Integrity
{
    public sealed class Hosts
    {
        List<string> _hosts = new List<string>();

        public Hosts AddHost(string host)
        {
            Guard.AgainstNull(nameof(host), host);
            Guard.AgainstEmptyWhiteSpace(nameof(host), host);

            _hosts.Add(host);
            return this;
        }

        internal IEnumerable<string> Content => _hosts;
    }

    public sealed class HostAvailability : EvidenceProvider
    {
        readonly INetwork _network;
        readonly int _timeout;
        readonly IEnumerable<string> _hosts;

        public HostAvailability(INetwork network,
            IEnumerable<EvidenceProvider> dependencies, int timeout, IEnumerable<string> hosts) : base(dependencies)
        {
            Guard.AgainstNegative(nameof(timeout), timeout);
            Guard.AgainstNull(nameof(hosts), hosts);

            _network = network;
            _timeout = timeout;
            _hosts = hosts;
        }

        public HostAvailability(IEnumerable<EvidenceProvider> dependencies, int timeout, IEnumerable<string> hosts)
            : this(new Network(), dependencies, timeout, hosts) { }

        public HostAvailability(INetwork network, IEnumerable<string> hosts)
            : this(network, Enumerable.Empty<EvidenceProvider>(), 1200, hosts) { }

        public HostAvailability(IEnumerable<string> hosts) : this(new Network(), hosts) { }

        public override async Task<Result<Evidence, string>> VerifyAsync()
        {
            foreach (var host in _hosts) {
                var status = await _network.PingAsync(host, _timeout);
                if (status != IPStatus.Success) {
                    return Result<Evidence, string>.FailWith($"{host} host is unknown.");
                }
            }
            return Result<Evidence, string>.Succeed(new Evidence(GetType(), _hosts));
        }
    }
}