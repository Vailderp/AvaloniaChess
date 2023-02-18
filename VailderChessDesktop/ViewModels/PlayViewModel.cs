using System;
using System.Reactive;
using System.Windows.Input;
using Avalonia.Threading;
using ReactiveUI;
using Splat;
using VailderChessDesktop.Game;
using VailderChessDesktop.Network.Common;
using VailderChessDesktop.Network.Core;

namespace VailderChessDesktop.ViewModels;

public class PlayViewModel : ViewModelBase, IRoutableViewModel
{
    public PlayViewModel(IScreen? hostScreen = null)
    {
        HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
        _createNewRoom = ReactiveCommand.Create(
            CreateNewRoomEx
        );
        _createNewRoom.ThrownExceptions.Subscribe(exception =>
        {
            MessageBoxManager.CallMessageBox(exception.Message);
        });
    }

    private void CreateNewRoomEx()
    {
        if (GameRoomName == "" || GameRoomName.Length > 25) 
            throw new Exception("Ошибка|Неверно введёно название комнаты. Должно быть не менее 1 и не более 25 символов!");
        if (ResourceManager.NetworkClient is { Connected: true })
        {
            ResourceManager.NetworkClient.EmitRequireAsync(ResourceManager.NetworkClient.CreateRequireMessage("createroom", GetRoomPacket()))
                .ContinueWith(msg =>
                {
                    if (msg.Result.Data.ToString() == "n")
                    {
                        throw new Exception("Ошибка|Название комнаты уже занято!");
                    }
                    MessageBoxManager.CallMessageBox("Сервер принял запрос на создание игровой комнаты: " + msg.Result.MessageString);
                    ResourceManager.PlayerVSPlayerViewModel = new PlayerVSPlayerViewModel(GetRoomPacket(), false);
                    Dispatcher.UIThread.InvokeAsync(
                        () => ResourceManager.MainWindowViewModel.Router.Navigate.Execute(ResourceManager.PlayerVSPlayerViewModel)
                        );
                });
        }
        else
            throw new Exception("Ошибка сети|Вы не подключились к серверу");
    }

    private RoomPacket GetRoomPacket()
    {
        return new RoomPacket(
            GameRoomName, 
            ResourceManager.ChessClient?.Name ?? "", 
            "", 
            GameRoomTime.Minutes, 
            GameRoomTime.Hours, 
            _gameRoomFigureColor == "Чёрный");
    }
    
    private string _gameRoomName = "";
    public string GameRoomName
    {
        get => _gameRoomName;
        set
        {
            _gameRoomName = value;
            OnPropertyChanged();
        }
    }
    
    private TimeSpan _gameRoomTime = TimeSpan.Parse("01:30");
    public TimeSpan GameRoomTime
    {
        get => _gameRoomTime;
        set
        {
            _gameRoomTime = value;
            OnPropertyChanged();
        }
    }
    
    private string _gameRoomFigureColor;
    public string GameRoomFigureColor
    {
        get => _gameRoomFigureColor;
        set
        {
            _gameRoomFigureColor = value;
            OnPropertyChanged();
        }
    }
    
    private readonly ReactiveCommand<Unit, Unit> _createNewRoom;
    public ICommand CreateNewRoom => _createNewRoom;

    public IScreen HostScreen { get; }

    public string UrlPathSegment => "/play";
    
}