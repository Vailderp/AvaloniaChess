using System;
using System.IO;
using System.Reactive;
using System.Windows.Input;
using ReactiveUI;

namespace VailderChessDesktop.ViewModels
{
    public class MainWindowViewModel : ReactiveObject, IScreen
    {
        private readonly ReactiveCommand<Unit, Unit> _play;
        
        private readonly ReactiveCommand<Unit, Unit> _waitingRoom;
        
        private readonly ReactiveCommand<Unit, Unit> _engines;
        
        private readonly ReactiveCommand<Unit, Unit> _settings;
        
        private readonly ReactiveCommand<Unit, Unit> _connectToServer;
        
        private RoutingState _router = new RoutingState();

        public MainWindowViewModel()
        {
            _play = ReactiveCommand.Create(
                PlayEx
            );
            _waitingRoom = ReactiveCommand.Create(
                WaitingEx
            );
            _engines = ReactiveCommand.Create(
                EnginesEx
            );
            _settings = ReactiveCommand.Create(
                SettingsEx
            );
            _connectToServer = ReactiveCommand.Create(
                ConnectToServerEx
            );
        }

        private void ConnectToServerEx()
        {
            Router.Navigate.Execute(new ConnectToServerViewModel());
        }

        private void SettingsEx()
        {
            Router.Navigate.Execute(new SettingsViewModel());
        }

        private void EnginesEx()
        {
            Router.Navigate.Execute(new ChessEnginesListViewModel());
        }

        private void WaitingEx()
        {
            Router.Navigate.Execute(new GameRoomsViewModel());
        }

        private void PlayEx()
        {
            Router.Navigate.Execute(new PlayViewModel());
        }

        public RoutingState Router
        {
            get => _router;
            set => this.RaiseAndSetIfChanged(ref _router, value);
        }
        
        public ICommand Play => _play;
        
        public ICommand WaitingRoom => _waitingRoom;
        
        public ICommand Engines => _engines;
        
        public ICommand Settings => _settings;
        
        public ICommand ConnectToServer => _connectToServer;
    }
}