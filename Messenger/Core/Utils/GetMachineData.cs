using System.Net;
using System.Net.Sockets;

namespace Messenger.Core.Utils
{
    public class GetMachineData
    {
        public static string CurrentIpAddress()
        {
            string localIP = string.Empty;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }

            return localIP;
        }
    }
}