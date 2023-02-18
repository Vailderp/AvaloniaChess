using System;
using System.Diagnostics;
using System.Reactive;
using System.Windows.Input;
using ReactiveUI;
using Splat;
using VailderChessDesktop.Game;
using VailderChessDesktop.Network.Common;
using VailderChessDesktop.Network.Core;

namespace VailderChessDesktop.ViewModels;

public class ConnectToServerViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen { get; }

    public string UrlPathSegment => "/connectToServer";

    public ConnectToServerViewModel(IScreen? hostScreen = null)
    {
        HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
        _connectToServer = ReactiveCommand.Create(
            ConnectToServerEx
        );
        _connectToServer.ThrownExceptions.Subscribe(exception =>
        {
            MessageBoxManager.CallMessageBox(exception.Message);
        });
    }

    private void ConnectToServerEx()
    {
        if (PlayerName == "" || PlayerName.Length > 25) 
            throw new Exception("Ошибка|Неверно введёно имя пользователя. Должно быть не менее 1 и не более 25 символов!");
        NetworkListener networkListener = new NetworkListener();
        ResourceManager.NetworkClient = new Client(networkListener);
        ResourceManager.ChessClient = new ChessClient(ResourceManager.NetworkClient, PlayerName);
        var serverAddress = Utility.ParseServerAddress(ServerAddress);
        if (!Utility.IsCorrectServerAddress(serverAddress.Localaddr, serverAddress.Port.ToString())) 
            throw new Exception("Ошибка|Неверно введён адрес сервера");
        try
        {
            ResourceManager.NetworkClient.Connect(serverAddress.Localaddr, serverAddress.Port);
        }
        catch (Exception e)
        {
            throw new Exception("Ошибка|Не удалось подключться к серверу: " + e.Message);
        }
        AddRequireActions(PlayerName);
    }

    private void AddRequireActions(string playerName)
    {
        ResourceManager.NetworkClient?.OnRequire("name", message =>
        {
            var messageName = new Message { MessageString = "response", UID = message.UID, Data = playerName };
            ResourceManager.NetworkClient.Emit(messageName);
        });
        ResourceManager.NetworkClient?.OnRequire("disconnect", message =>
        {
            MessageBoxManager.CallMessageBox("Сервер отключил вас|" + message.Data.ToString());
            ResourceManager.NetworkClient.Release();
            ResourceManager.NetworkClient = null;
        });
    }

    public string ServerAddress { get; set; } = "";
    public string PlayerName { get; set; } = "";

    private readonly ReactiveCommand<Unit, Unit> _connectToServer;
    public ICommand ConnectToServer => _connectToServer;
    
}