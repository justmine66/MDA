using System.Net;
using System.Net.NetworkInformation;

namespace MDA.Infrastructure.Utils
{
    public static class IPFinder
    {
        public static IPAddress GetNonLoopbackAddress()
        {
            foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (var address in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (address.Address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork) 
                        continue;

                    if (!IPAddress.IsLoopback(address.Address))
                    {
                        return address.Address;
                    }
                }
            }

            return null;
        }
    }
}
