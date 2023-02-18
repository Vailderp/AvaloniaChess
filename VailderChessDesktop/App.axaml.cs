using System;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using Splat;
using VailderChessDesktop.Game;
using VailderChessDesktop.ViewModels;
using VailderChessDesktop.Views;

//lip_h6G2BuEROQ4sxHxWPlrZ

namespace VailderChessDesktop
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            Locator.CurrentMutable.RegisterConstant<IScreen>(ResourceManager.MainWindowViewModel);
            Locator.CurrentMutable.Register<IViewFor<PlayViewModel>>(() => new Views.PlayView());
            Locator.CurrentMutable.Register<IViewFor<ChessEnginesListViewModel>>(() => new Views.ChessEnginesListView());
            Locator.CurrentMutable.Register<IViewFor<ChessEnginesViewModel>>(() => new Views.ChessEnginesView());
            Locator.CurrentMutable.Register<IViewFor<SettingsViewModel>>(() => new Views.SettingsView());
            Locator.CurrentMutable.Register<IViewFor<ConnectToServerViewModel>>(() => new Views.ConnectToServerView());
            Locator.CurrentMutable.Register<IViewFor<GameRoomsViewModel>>(() => new Views.GameRoomsView());
            Locator.CurrentMutable.Register<IViewFor<PlayerVSPlayerViewModel>>(() => new Views.PlayerVSPlayerView());

            ResourceManager.MainWindowViewModel.Router.Navigate.Execute(new ConnectToServerViewModel());
            
            new MainWindow { DataContext = Locator.Current.GetService<IScreen>() }.Show();

            base.OnFrameworkInitializationCompleted();
        }
    }
}