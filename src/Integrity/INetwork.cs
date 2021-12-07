using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace Integrity
{
    public interface INetwork
    {
        Task<IPStatus> PingAsync(string host, int timeout);
    }
}
