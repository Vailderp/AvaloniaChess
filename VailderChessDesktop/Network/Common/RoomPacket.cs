using System;
using System.Runtime.Serialization;

namespace VailderChessDesktop.Network.Common;

[Serializable]
public class RoomPacket
{
    public RoomPacket(string name, string ownerName, string rivalName, int timeMinutes, int timeHours, bool isBlack)
    {
        Name = name;
        OwnerName = ownerName;
        RivalName = rivalName;
        TimeMinutes = timeMinutes;
        TimeHours = timeHours;
        IsBlack = isBlack;
    }

    public RoomPacket()
    {
        Name = "";
        OwnerName = "";
        RivalName = "";
        TimeMinutes = default;
        TimeHours = default;
        IsBlack = false;
    }

    [DataMember]
    public string Name { get; set; }
    
    [DataMember]
    public string OwnerName { get; set; }
    
    [DataMember]
    public string RivalName { get; set; }

    [DataMember]
    public int TimeMinutes  { get; set; }
    
    [DataMember]
    public int TimeHours  { get; set; }
    
    [DataMember]
    public bool IsBlack  { get; set; }

    [IgnoreDataMember] 
    public string TimeOfGameStr => $"{(TimeHours < 10 ? "0" : "")}{TimeHours}:{(TimeMinutes < 10 ? "0" : "")}{TimeMinutes}";
    
    [IgnoreDataMember] 
    public string FigureColorStr => IsBlack ? "Белый" : "Чёрный";
}