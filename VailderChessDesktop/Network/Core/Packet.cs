using System.Net.Sockets;
using VailderChessDesktop.Network.Contract;

namespace VailderChessDesktop.Network.Core;

public class Packet 
{
    public Packet(TcpClient client)
    {
        _tcpClient = client;
        _networkStream = _tcpClient.GetStream();
    }

    private readonly NetworkStream _networkStream;
    
    private readonly TcpClient _tcpClient;

    public void Deserialize<T>(ref T? packetSerializable) where T : IDeserializePacket<T>
    {
        if (packetSerializable != null) 
            packetSerializable = packetSerializable.Deserialize(_tcpClient.GetStream(), _tcpClient);
    }

    public void Serialize(ISerializePacket packetSerializable)
    {
        packetSerializable.Serialize(_networkStream, _tcpClient);
    }
}