using System;
using System.Reactive.Disposables;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace VailderChessDesktop.Views
{
    public partial class MainWindow : ReactiveWindow<ViewModels.MainWindowViewModel>
    {
        public MainWindow()
        {
            //InitializeComponent();
            this.WhenActivated((CompositeDisposable disposable) => { });
            //this.Resources.MergedDictionaries.Add(LanguageManager.CurrentLanguageResource);
            AvaloniaXamlLoader.Load(this);
        }
    }
}