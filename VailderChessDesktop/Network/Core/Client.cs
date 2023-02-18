using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using VailderChessDesktop.Game;
using VailderChessDesktop.Network.Contract;
using VailderChessDesktop.Network.Core;

namespace VailderChessDesktop.Network.Core;

public class Client : IGeneratorUID
{
    public volatile ConcurrentDictionary<string, Message> Responses;
    
    public volatile ConcurrentDictionary<string, Message> Requires;
    
    private uint _currentUID = 0;

    //public volatile ConcurrentBag<Message> Responses;
    
    //public volatile ConcurrentBag<Message> Requires;
    
    public TcpClient? TcpClient { get; protected set; }
    
    public Packet Packet { get; protected set; }

    public NetworkListener NetworkListener;
    
    public NetworkStream? NetworkStream => TcpClient?.GetStream();

    public bool Connected => TcpClient?.Connected ?? false;
    
    public Client(NetworkListener networkListener)
    {
        Responses = new ConcurrentDictionary<string, Message>();
        Requires = new ConcurrentDictionary<string, Message>();
        NetworkListener = networkListener;
        NetworkListener.ClientOn += NetworkListenerOnClientOn;
    }

    private void NetworkListenerOnClientOn(Message message)
    {
        if (message.MessageString == "response")
        {
            Responses.TryAdd(message.UID, message);
        }
        else if (message.MessageString.Split()[0] == "require")
        {
            Requires.TryAdd(message.MessageString.Split()[1], message);
        }

        //TcpClient.SendTimeout = 10;
    }

    public void Connect(string localaddr, int port)
    {
        TcpClient = new TcpClient(localaddr, port);
        if (TcpClient != null) 
            Packet = new Packet(TcpClient);
        StartListenServer();
        NetworkListener.OnClientConnected();
    }

    private void StartListenServer()
    {
        Task.Factory.StartNew(async () =>
            {
                if (NetworkStream == null) return;
                if (TcpClient == null) return;
                while (Connected)
                {
                    while (!NetworkStream.DataAvailable)
                    {
                        await Task.Delay(25);
                    }
                    Message? message = new Message(); 
                    Packet.Deserialize(ref message);
                    if (message == null) continue;
                    Console.WriteLine(message.MessageString);
                    NetworkListener.OnClientOn(message);
                }
            }
        );
    }

    public void Emit(Message message)
    {
        Packet.Serialize(message);
    }
    
    public async Task<Message> EmitRequireAsync(Message message)
    {
        Emit(message);
        while (!Responses.ContainsKey(message.UID))
        {
            await Task.Delay(50);
        }
        return Responses[message.UID];
    }
    
    public async Task<Message> WaitRequireAsync(string requireWhat)
    {
        while (!Requires.ContainsKey(requireWhat))
        {
            await Task.Delay(50);
        }
        
        Requires.Remove(requireWhat, out var remMessage);
        return remMessage;
    }
    
    public void OnRequire(string requireWhat, Action<Message> action)
    {
        Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    while (!Requires.ContainsKey(requireWhat))
                    {
                        await Task.Delay(50);
                    }
                    Requires.Remove(requireWhat, out var remMessage);
                    action.Invoke(remMessage);
                }
            }
        );
    }
    
    
    public string GenerateMessageUID()
    {
        return $"UID{_currentUID++}";
    }

    public Message CreateRequireMessage<TDataPacket>(string whatRequire, TDataPacket? data)
    {
        return Message.CreateRequireMessage(this, whatRequire, data);
    }
    
    public Message CreateResponseMessage<TDataPacket>(string whatResponse, TDataPacket? data)
    {
        return Message.CreateRequireMessage(this, whatResponse, data);
    }
    
    public void Release()
    {
        TcpClient?.Close();
    }
}
