using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using ReactiveUI;
using VailderChessDesktop.Game;
using VailderChessDesktop.ViewModels;

namespace VailderChessDesktop.Views;

public partial class ChessEngineProtocolWindow :  ReactiveWindow<ChessEngineProtocolWindowViewModel>
{
    private StackPanel? _mainPanel;

    private UciConnector? _uciConnector;

    public UciConnector? UciConnector
    {
        get => _uciConnector;
        set
        {
            _uciConnector = value;
            if (DataContext is ChessEngineProtocolWindowViewModel viewModel)
                viewModel.UciConnector = _uciConnector;
        }
    }

    public ChessEngineProtocolWindow()
    {
        InitializeComponent();
        DataContext = new ChessEngineProtocolWindowViewModel();
    }
    
    public ChessEngineProtocolWindow(UciConnector uciConnector)
    {
        InitializeComponent();
        UciConnector = uciConnector;
    }

    public void AddInput(string text)
    {
        var textBlock = new TextBlock
        {
            Text = "=>" + text,
            TextWrapping = TextWrapping.Wrap,
            Margin = Thickness.Parse("2")
        };
        _mainPanel?.Children.Add(textBlock);
    }
    
    public void AddOutput(string text)
    {
        var textBlock = new TextBlock
        {
            Text = "->" + text,
            TextWrapping = TextWrapping.Wrap,
            Margin = Thickness.Parse("2")
        };
        _mainPanel?.Children.Add(textBlock);
    }

    private void InitializeComponent()
    {
        this.WhenActivated((CompositeDisposable disposable) =>
        {
            _mainPanel = this.FindControl<StackPanel>("MainPanel");
        });
        AvaloniaXamlLoader.Load(this);
    }
}