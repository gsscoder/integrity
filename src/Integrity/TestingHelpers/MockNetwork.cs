using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace Integrity.TestingHelpers
{
    public sealed class MockHost
    {
        public MockHost(string name, int delay = 100)
        {
            Name = name;
            Delay = delay;
        }

        public string Name { get; private set; }

        public int Delay  { get; private set; }
    }

    public sealed class MockNetwork : INetwork
    {
        readonly IEnumerable<MockHost> _hosts;

        public MockNetwork(IEnumerable<MockHost> hosts)
        {
            _hosts = hosts;
        }

        public Task<IPStatus> PingAsync(string host, int timeout)
        {
            var @this =_hosts.SingleOrDefault(h => h.Name.Equals(host, StringComparison.Ordinal));
            if (@this == null) return Task.FromResult(IPStatus.Unknown);
            if (@this.Delay > timeout) return Task.FromResult(IPStatus.TimedOut);
            return Task.FromResult(IPStatus.Success);
        }
    }
}