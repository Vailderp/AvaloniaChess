using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using VailderChessDesktop.Game;
using VailderChessDesktop.UserControls;
using VailderChessDesktop.ViewModels;

namespace VailderChessDesktop.Views;

public partial class PlayerVSPlayerView :  ReactiveUserControl<PlayerVSPlayerViewModel>
{
    public PlayerVSPlayerView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.WhenActivated((CompositeDisposable disposable) =>
        {
            ResourceManager.ChessBoardPlayerVSPlayer = this.FindControl<ChessBoardUserControl>("MainChessBoard");
            if (ResourceManager.ChessBoardPlayerVSPlayer == null) return;
        });
        AvaloniaXamlLoader.Load(this);
    }
}