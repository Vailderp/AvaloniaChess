using System.Diagnostics;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using VailderChessDesktop.Game;
using VailderChessDesktop.UserControls;
using VailderChessDesktop.ViewModels;

namespace VailderChessDesktop.Views;

public partial class ChessEnginesView : ReactiveUserControl<ChessEnginesViewModel>
{
    public ChessBoardUserControl EngineChessBoard { get; set; }
    public ChessEnginesView()
    {
        //DataContext = new ChessEnginesViewModel();
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.WhenActivated((CompositeDisposable disposable) =>
        {
            EngineChessBoard = this.FindControl<ChessBoardUserControl>("MainChessBoard");
            if (EngineChessBoard == null) return;
            EngineChessBoard.PlayerColor = PlayerColor.Black;
            ViewModel.ChessEnginesView = this;
        });
        AvaloniaXamlLoader.Load(this);
    }
    
}