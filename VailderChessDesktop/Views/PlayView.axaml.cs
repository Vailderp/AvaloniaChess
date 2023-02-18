using System;
using System.Reactive.Disposables;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace VailderChessDesktop.Views;

public partial class PlayView : ReactiveUserControl<ViewModels.PlayViewModel>
{
    public PlayView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.WhenActivated((CompositeDisposable disposable) => { });
        AvaloniaXamlLoader.Load(this);
        //this.Resources.MergedDictionaries[1] = (LanguageManager.CurrentLanguageResource);
    }
}