using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.MarkupExtensions;
using VailderChessDesktop.Network.Common;
using VailderChessDesktop.Network.Core;
using VailderChessDesktop.Resources;
using VailderChessDesktop.UserControls;
using VailderChessDesktop.ViewModels;
using VailderChessDesktop.Views;

namespace VailderChessDesktop.Game;

public static class ResourceManager
{
    static ResourceManager()
    {
        ChessEnginesView = new ChessEnginesView();
        MainWindowViewModel = new MainWindowViewModel();
        Rooms = new List<Room>();
        PlayerVSPlayerViewModel = null;
        //RD
        _appLanguageResourceDictionary = new AppLanguageRussian();
        _appDesignResourceDictionary = new AppDesignEggplant();
        _appDesignChessResourceDictionary = new AppStyleChessFiguresStandard();
        _appDesignChessboardResourceDictionary = new AppStyleChessboardBlackberry();
        Application.Current?.Resources.MergedDictionaries.Add(_appLanguageResourceDictionary);
        Application.Current?.Resources.MergedDictionaries.Add(_appDesignResourceDictionary);
        Application.Current?.Resources.MergedDictionaries.Add(_appDesignChessResourceDictionary);
        Application.Current?.Resources.MergedDictionaries.Add(_appDesignChessboardResourceDictionary);
    }
    
    public static int LanguageSelectedIndex = 0;
    public static int AppDesignSelectedIndex = 0;
    public static int ChessDesignSelectedIndex = 0;
    public static int ChessboardDesignSelectedIndex = 0;
    
    private static ResourceDictionary _appLanguageResourceDictionary;
    public static ResourceDictionary AppLanguageResourceDictionary
    {
        get => _appLanguageResourceDictionary;
        set
        {
            try
            {
                Application.Current?.Resources.MergedDictionaries.Remove(_appLanguageResourceDictionary);
                _appLanguageResourceDictionary = value;
                Application.Current?.Resources.MergedDictionaries.Add(_appLanguageResourceDictionary);
            } catch { }
        }
    }
    
    private static ResourceDictionary _appDesignResourceDictionary;
    public static ResourceDictionary AppDesignResourceDictionary    
    {
        get => _appDesignResourceDictionary;
        set
        {
            try
            {
                Application.Current?.Resources.MergedDictionaries.Remove(_appDesignResourceDictionary);
                _appDesignResourceDictionary = value;
                Application.Current?.Resources.MergedDictionaries.Add(_appDesignResourceDictionary);
            } catch { }
        }
    }
    
    private static ResourceDictionary _appDesignChessResourceDictionary;
    public static ResourceDictionary AppDesignChessResourceDictionary    
    {
        get => _appDesignChessResourceDictionary;
        set
        {
            try
            {
                Application.Current?.Resources.MergedDictionaries.Remove(_appDesignChessResourceDictionary);
                _appDesignChessResourceDictionary = value;
                Application.Current?.Resources.MergedDictionaries.Add(_appDesignChessResourceDictionary);
            } catch { }
        }
    }
    
    private static ResourceDictionary _appDesignChessboardResourceDictionary;
    public static ResourceDictionary AppDesignChessboardResourceDictionary
    {
        get => _appDesignChessboardResourceDictionary;
        set
        {
            try
            {
                Application.Current?.Resources.MergedDictionaries.Remove(_appDesignChessboardResourceDictionary);
                _appDesignChessboardResourceDictionary = value;
                Application.Current?.Resources.MergedDictionaries.Add(_appDesignChessboardResourceDictionary);
            } catch { }
        }
    }
    
    private static ChessBoardUserControl? _chessBoardPlayerVSPlayer = null;

    public static ChessBoardUserControl? ChessBoardPlayerVSPlayer
    {
        get => _chessBoardPlayerVSPlayer;
        set
        {
            _chessBoardPlayerVSPlayer = value;
            if (_chessBoardPlayerVSPlayer == null) return;
            if (NetworkClient == null) return;
            _chessBoardPlayerVSPlayer.PlayerMoved += move =>
            {
                if (PlayerVSPlayerViewModel != null)
                    PlayerVSPlayerViewModel.IsPlayer1Round = false;
                var playerMoveClientPacket = new PlayerMoveClientPacket
                {
                    MoveString = move
                };
                var message = Message.CreateRequireMessage(NetworkClient, "playermoveclient", playerMoveClientPacket);
                NetworkClient.Emit(message);
            };
        }
    }
    
    public static List<Room> Rooms { get; set; }
    public static Client? NetworkClient { get; set; }
    
    public static ChessClient? ChessClient { get; set; }
    


    public static MainWindowViewModel MainWindowViewModel;

    public static PlayerVSPlayerViewModel? PlayerVSPlayerViewModel;
    
    public static ChessEnginesView ChessEnginesView;

    public const string AppDataChessEnginesFolder = @"\VailderChess\ChessEngines";


    private static string? _appDataLocalPath = null;
    public static string AppDataLocalPath
    {
        get
        {
            if (_appDataLocalPath != null) return _appDataLocalPath;
            _appDataLocalPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (!Directory.Exists(_appDataLocalPath))
            {
                Directory.CreateDirectory(_appDataLocalPath);
            }
            return _appDataLocalPath;
        }
    }
    
    public static string VailderChessDirectoryPath => AppDataLocalPath + @"\VailderChess";
    
    public static string ChessEnginesDirectoryPath => VailderChessDirectoryPath + @"\ChessEngines";
    
    public static string ChessPartiesDirectoryPath => VailderChessDirectoryPath + @"\ChessParties";

    public static Uri? CreateResourceUri(string localPath)
    {
        var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name;
        if (assemblyName == null) return null;
        var uri = new Uri($"avares://{assemblyName}/{localPath}");
        return uri;
    }

    public static string? ShowInOperationSystemExplorer(string path)
    {
        try
        {
            System.Diagnostics.Process.Start(
                new System.Diagnostics.ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true,
                    Verb = "open"
                }
            );
        }
        catch (Exception e)
        {
            return e.Message;
        }

        return null;
    }
}