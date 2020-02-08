using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace Integrity
{
    public sealed class Network : INetwork
    {
        readonly Ping _ping = new Ping();

        public async Task<IPStatus> PingAsync(string host, int timeout)
        {
            var reply = await _ping.SendPingAsync(host, timeout);
            return reply.Status;
        }
    }
}