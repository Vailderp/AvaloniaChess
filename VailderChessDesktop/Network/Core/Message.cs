using System;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using VailderChessDesktop.Network.Contract;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace VailderChessDesktop.Network.Core;

[Serializable]
public class Message : IDeserializePacket<Message>, ISerializePacket
{
    public const int NETWORK_BUFFER_BYTENESS = 1024;
    
    public Message()
    {
        MessageString = string.Empty;
    }
    
    public Message(string messageString)
    {
        MessageString = messageString;
    }
    
    public Message(string messageString, object data)
    {
        MessageString = messageString;
        Data = data;
    }
    
    public Message(string messageString, string uid, object data)
    {
        MessageString = messageString;
        UID = uid;
        Data = data;
    }
    
    [DataMember]
    public string MessageString { get; set; }
    
    [DataMember]
    public string UID { get; set; }
    
    [DataMember]
    public object Data { get; set; }

    public TData? GetData<TData>()
    {
        var jsonElement = (JsonElement)Data;
        return jsonElement.Deserialize<TData>();
    }

    public Message? Deserialize(NetworkStream networkStream, TcpClient client)
    {
        byte[] bytes = new byte[NETWORK_BUFFER_BYTENESS];
        networkStream.Read(bytes, 0, bytes.Length);
        string jsonString = Encoding.UTF8.GetString(bytes);
        jsonString = jsonString.Substring(0, jsonString.IndexOf(jsonString.First(ch => ch == 0x00)));
        var options = new JsonSerializerOptions
            { WriteIndented = true, IgnoreReadOnlyFields = true, IgnoreReadOnlyProperties = true };
        return JsonSerializer.Deserialize<Message>(jsonString, options);
    }
    
    public void Serialize(NetworkStream networkStream, TcpClient client)
    {
        var options = new JsonSerializerOptions
            { WriteIndented = true, IgnoreReadOnlyFields = true, IgnoreReadOnlyProperties = true };
        string jsonString = JsonSerializer.Serialize(this, options);
        byte[] bytes = new byte[NETWORK_BUFFER_BYTENESS];
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);
        jsonBytes.CopyTo(bytes, 0);
        networkStream.Write(bytes);
    }

    public static Message CreateRequireMessage<TDataPacket>(
        IGeneratorUID generatorUid,
        string whatRequire, 
        TDataPacket? data
        )
    {
        return new Message($"require {whatRequire}", generatorUid.GenerateMessageUID(), data);
    }
    
    public static Message CreateResponseMessage<TDataPacket>(
        IGeneratorUID generatorUid,
        string whatResponse, 
        TDataPacket? data
    )
    {
        return new Message($"response {whatResponse}", generatorUid.GenerateMessageUID(), data);
    }
}