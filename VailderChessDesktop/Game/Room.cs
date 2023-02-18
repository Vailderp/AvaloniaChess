using System;
using System.Reactive;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using ReactiveUI;
using VailderChessDesktop.Network.Common;
using VailderChessDesktop.Network.Core;

namespace VailderChessDesktop.Game;

public class Room
{
    public Room(RoomPacket roomPacket)
    {
        RoomPacket = roomPacket;
        _beginGameCommand = ReactiveCommand.Create(
            BeginGame
        );
    }

    private void BeginGame()
    {
        if (ResourceManager.NetworkClient == null) return;
        if (ResourceManager.PlayerVSPlayerViewModel != null) return;
        if (ResourceManager.ChessClient == null) return;
        RoomPacket.RivalName = ResourceManager.ChessClient.Name;
        ResourceManager.NetworkClient
            .EmitRequireAsync(Message.CreateRequireMessage(ResourceManager.NetworkClient, "begingame", RoomPacket))
            .ContinueWith(task =>
                {
                    var message = task.Result;
                    var canConnectToGame = message.Data.ToString();
                    if (canConnectToGame == "you can begin game")
                    {
                        ResourceManager.PlayerVSPlayerViewModel =
                            new ViewModels.PlayerVSPlayerViewModel(RoomPacket, true);
                        Dispatcher.UIThread.InvokeAsync(() =>
                            ResourceManager.MainWindowViewModel.Router.Navigate.Execute(ResourceManager
                                .PlayerVSPlayerViewModel));
                    }
                    else
                    {
                        RoomPacket.RivalName = "";
                        throw new Exception($"Запрос на создание комнаты отклонён|{canConnectToGame}");
                    }
                }
            );
    }

    public RoomPacket RoomPacket { get; set; }

    private readonly ReactiveCommand<Unit, Unit> _beginGameCommand;
    
    public ICommand BeginGameCommand => _beginGameCommand;

}