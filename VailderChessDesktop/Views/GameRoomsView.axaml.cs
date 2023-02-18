using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using VailderChessDesktop.ViewModels;

namespace VailderChessDesktop.Views;

public partial class GameRoomsView : ReactiveUserControl<GameRoomsViewModel>
{
    public GameRoomsView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}