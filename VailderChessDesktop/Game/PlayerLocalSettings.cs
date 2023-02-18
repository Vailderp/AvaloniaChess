using System.Runtime.Serialization;

namespace VailderChessDesktop.Game;

[DataContract]
public class PlayerLocalSettings
{
    [DataMember]
    public string Name { get; }
    
    [DataMember]
    public string Sla { get; }
    
    [DataMember]
    public string Naw { get; }

    public PlayerLocalSettings()
    {
        Name = "Vladislav";
        Sla = "Gafiev";
        Naw = "604";
    }
}