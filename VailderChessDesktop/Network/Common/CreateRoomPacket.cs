using System;
using System.Runtime.Serialization;

namespace VailderChessDesktop.Network.Common;

[Serializable]
public class CreateRoomPacket
{
    [DataMember]
    public string Name { get; set; }
    
    [DataMember]
    public int TimeMinutes  { get; set; }
    
    [DataMember]
    public int TimeHours  { get; set; }
    
    [DataMember]
    public bool IsBlack  { get; set; }
}