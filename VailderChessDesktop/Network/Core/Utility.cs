using System.Text.RegularExpressions;

namespace VailderChessDesktop.Network.Core;

public static class Utility
{
    public struct ServerAddress
    {
        public string Localaddr;
        public int Port;

        public ServerAddress(string localaddr, int port)
        {
            Localaddr = localaddr;
            Port = port;
        }
    }

    public static ServerAddress ParseServerAddress(string serverAddress)
    {
        try
        {
            var splitAddress = serverAddress.Split(':');
            return new ServerAddress(splitAddress[0], int.Parse(splitAddress[1]));
        }
        catch
        {
            return new ServerAddress("", 0);
        }
    }

    public static bool IsCorrectServerAddress(string localAddress, string port)
    {
        const string regex0To255 = @"\b([0-1]?[0-9][0-9]?|2[0-4][0-9]|25[0-5])";
        var isMatchLocalAddress = Regex.IsMatch(localAddress, 
            @$"^{regex0To255}.{regex0To255}.{regex0To255}.{regex0To255}$");
        var tryParse = ushort.TryParse(port, out var portInt);
        return tryParse && isMatchLocalAddress;
    }
}