using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using VailderChessDesktop.Game;
using VailderChessDesktop.ViewModels;

namespace VailderChessDesktop.Views;

public partial class ChessEnginesListView : ReactiveUserControl<ChessEnginesListViewModel>
{
    public ChessEnginesListView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        ResourceManager.ShowInOperationSystemExplorer(ResourceManager.ChessEnginesDirectoryPath);
    }
}