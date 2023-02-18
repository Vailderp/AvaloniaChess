using System.Text.Json;
using Avalonia.Threading;
using VailderChessDesktop.Game;
using VailderChessDesktop.Network.Core;

namespace VailderChessDesktop.Network.Common;

public class ChessClient
{
    public ChessClient(Client client, string name)
    {
        Client = client;
        Client.NetworkListener.ClientOn += NetworkListenerOnClientOn;
        Name = name;
    }

    private void NetworkListenerOnClientOn(Message message)
    {
        if (message.MessageString == "add room")
        {
            var roomPacket = message.GetData<RoomPacket>();
            if (roomPacket == null) return;
            ResourceManager.Rooms.Add(new Room(roomPacket));
        }
        else if (message.MessageString == "remove room")
        {
            if (message.Data is RoomPacket roomPacket)
            {
                //ResourceManager.Rooms.Remove(roomPacket);
            }
        }
        else if (message.MessageString == "player move server")
        {
            var playerMoveServerPacket = message.GetData<PlayerMoveServerPacket>();
            if (playerMoveServerPacket == null) return;
            if (ResourceManager.ChessBoardPlayerVSPlayer == null) return;
            if (ResourceManager.PlayerVSPlayerViewModel == null) return;
            Dispatcher.UIThread.InvokeAsync(() =>
                {
                    ResourceManager.ChessBoardPlayerVSPlayer.NextMove(playerMoveServerPacket.MoveString);
                    ResourceManager.PlayerVSPlayerViewModel.IsPlayer1Round = true;
                    // ResourceManager.PlayerVSPlayerViewModel.Player1TimeSeconds =
                    //     playerMoveServerPacket.Player1TimeSeconds;
                    // ResourceManager.PlayerVSPlayerViewModel.Player1TimeMinutes =
                    //     playerMoveServerPacket.Player1TimeMinutes;
                    // ResourceManager.PlayerVSPlayerViewModel.Player1TimeHours = playerMoveServerPacket.Player1TimeHours;
                    // ResourceManager.PlayerVSPlayerViewModel.Player2TimeSeconds =
                    //     playerMoveServerPacket.Player2TimeSeconds;
                    // ResourceManager.PlayerVSPlayerViewModel.Player2TimeMinutes =
                    //     playerMoveServerPacket.Player2TimeMinutes;
                    // ResourceManager.PlayerVSPlayerViewModel.Player2TimeHours = playerMoveServerPacket.Player2TimeHours;
                }
            );
        }
    }

    public Client Client { get; set; }
    
    public string Name { get; set; }
    
    
}