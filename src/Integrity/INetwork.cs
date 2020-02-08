using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace Integrity
{
    public interface INetwork
    {
        Task<IPStatus> PingAsync(string host, int timeout);

        
    }
}