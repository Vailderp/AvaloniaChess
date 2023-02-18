using System.Net.Sockets;

namespace VailderChessDesktop.Network.Contract;

public interface IDeserializePacket<out T> where T : IDeserializePacket<T>
{
    T? Deserialize(NetworkStream networkStream, TcpClient client);
}