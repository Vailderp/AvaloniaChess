using System.Net.Sockets;

namespace VailderChessDesktop.Network.Contract;

public interface ISerializePacket
{
    void Serialize(NetworkStream networkStream, TcpClient client);
}
