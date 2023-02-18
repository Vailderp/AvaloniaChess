using System;
using System.Threading.Tasks;
using System.Timers;
using Avalonia.Threading;
using ReactiveUI;
using Splat;
using VailderChessDesktop.Game;
using VailderChessDesktop.Network.Common;

namespace VailderChessDesktop.ViewModels;

public class PlayerVSPlayerViewModel : ViewModelBase, IRoutableViewModel
{
    public bool IsPlayer1Round = true;

    private System.Timers.Timer _aTimer;
    
    public string Player1Name { get; set; }
    
    public string Player2Name { get; set; }

    private int _player1TimeSeconds;
    public int Player1TimeSeconds 
    {
        get => _player1TimeSeconds;
        set
        {
            _player1TimeSeconds = value;
            OnPropertyChanged();
        }
    }
    
    private int _player1TimeMinutes;
    public int Player1TimeMinutes 
    {
        get => _player1TimeMinutes;
        set
        {
            _player1TimeMinutes = value;
            OnPropertyChanged();
        }
    }
    
    private int _player1TimeHours;
    public int Player1TimeHours 
    {
        get => _player1TimeHours;
        set
        {
            _player1TimeHours = value;
            OnPropertyChanged();
        }
    }

    private int _player2TimeSeconds;
    public int Player2TimeSeconds 
    {
        get => _player2TimeSeconds;
        set
        {
            _player2TimeSeconds = value;
            OnPropertyChanged();
        }
    }
    
    private int _player2TimeMinutes;
    public int Player2TimeMinutes 
    {
        get => _player2TimeMinutes;
        set
        {
            _player2TimeMinutes = value;
            OnPropertyChanged();
        }
    }
    
    private int _player2TimeHours;
    public int Player2TimeHours 
    {
        get => _player2TimeHours;
        set
        {
            _player2TimeHours = value;
            OnPropertyChanged();
        }
    }
    
    public PlayerVSPlayerViewModel(RoomPacket roomPacket, bool beginByConnect, IScreen? hostScreen = null)
    {
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            while (true)
            {
                if (ResourceManager.ChessBoardPlayerVSPlayer == null)
                {
                    await Task.Delay(25);
                    continue;
                }

                if (beginByConnect)
                {
                    ResourceManager.ChessBoardPlayerVSPlayer.PlayerColor =
                        roomPacket.IsBlack ? PlayerColor.Black : PlayerColor.White;
                }
                else
                {
                    ResourceManager.ChessBoardPlayerVSPlayer.PlayerColor =
                        roomPacket.IsBlack ? PlayerColor.White : PlayerColor.Black;
                }

                return;
            }
        });

        HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
        if (beginByConnect)
        {
            Player1Name = roomPacket.RivalName;
            Player2Name = roomPacket.OwnerName;
        }
        else
        {
            Player1Name = roomPacket.OwnerName;
            Player2Name = roomPacket.RivalName;
        }

        Player1TimeSeconds = 0;
        Player2TimeSeconds = 0;
        Player1TimeMinutes = roomPacket.TimeMinutes;
        Player2TimeMinutes = roomPacket.TimeMinutes;
        Player1TimeHours = roomPacket.TimeHours;
        Player2TimeHours = roomPacket.TimeHours;

        _aTimer = new Timer(1000);
        _aTimer.Elapsed += OmTimerTick;
        _aTimer.AutoReset = true;
        _aTimer.Enabled = true;
        _aTimer.Start();
    }

    private void OmTimerTick(object? source, ElapsedEventArgs e)
    {
        if (IsPlayer1Round)
        {
            if (Player1TimeSeconds <= 0)
            {
                Player1TimeSeconds = 59;
                Player1TimeMinutes -= 1;
            }
            if (Player1TimeMinutes <= 0)
            {
                Player1TimeMinutes = 59;
                Player1TimeHours -= 1;
            }
            if (Player1TimeHours < 0)
            {
                // Stop
            }
            Player1TimeSeconds -= 1;   
        }
        else
        {
            if (Player2TimeSeconds <= 0)
            {
                Player2TimeSeconds = 59;
                Player2TimeMinutes -= 1;
            }
            if (Player2TimeMinutes <= 0)
            {
                Player2TimeMinutes = 59;
                Player2TimeHours -= 1;
            }
            if (Player2TimeHours < 0)
            {
                // Stop
            }
            Player2TimeSeconds -= 1;   
        }
    }

    public IScreen HostScreen { get; }

    public string UrlPathSegment => "/player_vs_player";
}