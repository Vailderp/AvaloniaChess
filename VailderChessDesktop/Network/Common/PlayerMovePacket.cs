using System;
using System.Runtime.Serialization;

namespace VailderChessDesktop.Network.Common;

[Serializable]
public class PlayerMoveServerPacket
{
    [DataMember(Name = "P1TS")]
    public int Player1TimeSeconds  { get; set; }
    
    [DataMember(Name = "P1TM")]
    public int Player1TimeMinutes  { get; set; }
    
    [DataMember(Name = "P1TH")]
    public int Player1TimeHours  { get; set; }
    
    [DataMember(Name = "P2TS")]
    public int Player2TimeSeconds  { get; set; }
    
    [DataMember(Name = "P2TM")]
    public int Player2TimeMinutes  { get; set; }
    
    [DataMember(Name = "P2TH")]
    public int Player2TimeHours  { get; set; }

    [DataMember(Name = "MS")] 
    public string MoveString { get; set; } = "";
}

public class PlayerMoveClientPacket
{
    [DataMember(Name = "MS")] 
    public string MoveString { get; set; } = "";
}