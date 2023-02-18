using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using VailderChessDesktop.ViewModels;

namespace VailderChessDesktop.Views;

public partial class ConnectToServerView : ReactiveUserControl<ConnectToServerViewModel>
{
    public ConnectToServerView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.WhenActivated((CompositeDisposable disposable) => { });
        AvaloniaXamlLoader.Load(this);
    }
}