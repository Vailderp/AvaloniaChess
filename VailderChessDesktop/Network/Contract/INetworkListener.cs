using VailderChessDesktop.Network.Core;

namespace VailderChessDesktop.Network.Contract;

public interface INetworkDistribute
{
    void AddNetworkListener(NetworkListener networkListener);
    
}