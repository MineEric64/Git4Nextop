using System.Net.NetworkInformation;

namespace ProjectGFN.Others
{
    public class Network
    {
        public static bool IsAvailable => NetworkInterface.GetIsNetworkAvailable();
    }
}