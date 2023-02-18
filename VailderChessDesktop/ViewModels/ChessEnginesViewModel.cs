using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using ReactiveUI;
using Splat;
using VailderChessDesktop.Game;
using VailderChessDesktop.UserControls;
using VailderChessDesktop.Views;

namespace VailderChessDesktop.ViewModels;

public class ChessEnginesViewModel : ViewModelBase, IRoutableViewModel
{
    private readonly UciConnector _uciConnector = new UciConnector();
    
    public ChessEnginesView ChessEnginesView { get; set; }

    private IChessPlayer _playerEngine = new ChessEnginePlayer();
    
    private IChessPlayer _playerReal = new ChessEnginePlayer();
    
    private ChessEngineProtocolWindow? _protocolWindow = null;
    
    private readonly ReactiveCommand<Unit, Unit> _openProtocolWindow;
    public ICommand OpenProtocolWindow => _openProtocolWindow;
    
    private readonly ReactiveCommand<Unit, Unit> _bestMove;
    public ICommand BestMove => _bestMove;
    
    private readonly ReactiveCommand<Unit, Unit> _stopEngine;
    public ICommand StopEngine => _stopEngine;
    
    private readonly ReactiveCommand<Unit, Unit> _quitEngine;
    public ICommand QuitEngine => _quitEngine;
    
    private readonly ReactiveCommand<Unit, Unit> _newGame;
    public ICommand NewGame => _newGame;
    
    public ChessEnginesViewModel(IScreen? hostScreen = null, ChessEngine? chessEngine = null)
    {
        HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
        _uciConnector.OutputDataReceived += UciConnectorOnOutputDataReceived;
        _uciConnector.InputDataReceived += UciConnectorOnInputDataReceived;
        
        _openProtocolWindow = ReactiveCommand.Create(
            () =>
            {
                _protocolWindow = new ChessEngineProtocolWindow
                {
                    UciConnector = _uciConnector
                };
                _protocolWindow.Show();
            }
        );
        
        _bestMove = ReactiveCommand.CreateFromTask(
            BestMoveEx
        );
        
        _stopEngine = ReactiveCommand.Create(
            StopEngineEx
        );
        
        _quitEngine = ReactiveCommand.Create(
            QuitEngineEx
        );
        
        _newGame = ReactiveCommand.Create(
            NewGameEx
        );
        
        _uciConnector.Depth = (int)_depthSearch;
        _uciConnector.Nodes = (int)_nodesCount;
        
        if(chessEngine == null) return;
        
        _uciConnector.ConnectTo(chessEngine.Path).ContinueWith(
            uciok =>
            {
                if (uciok.Result)
                    Debug.WriteLine("Uci is ready!!!!!!!!!!!!!!!!!!");
                else
                    Debug.WriteLine("Uci is not ready!!!!!!!!!!!!!!!!!!");
            }
        ).ContinueWith(_ =>
            _uciConnector.IsEngineReady().ContinueWith(
                isready =>
                {
                    if (isready.Result)
                        Debug.WriteLine("Engine is ready!!!!!!!!!!!!!!!!!!");
                    else
                        Debug.WriteLine("Engine is not ready!!!!!!!!!!!!!!!!!!");
                }
            )
        );
        
    }

    private void NewGameEx()
    {
        if (!_uciConnector.Connected) return;
        _uciConnector.NewGame();
        _uciConnector.SetStartPosition();
    }

    private void QuitEngineEx()
    {
        if (!_uciConnector.Connected) return;
        _uciConnector.Quit();
        ResourceManager.MainWindowViewModel.Router.Navigate
            .Execute(new ChessEnginesListViewModel());
    }

    private void StopEngineEx()
    {
        if (!_uciConnector.Connected) return;
        _uciConnector.Stop();
    }

    private async Task BestMoveEx()
    {
        if (!_uciConnector.Connected) return;
        _uciConnector.SetStartPosition(ChessEnginesView.EngineChessBoard.GetMoves());
        ChessEnginesView.EngineChessBoard.NextMove(await _uciConnector.GoBestMoveAsync());
    }

    private void UciConnectorOnInputDataReceived(string message)
    {
        _protocolWindow?.AddInput(message);
    }

    private void UciConnectorOnOutputDataReceived(string message)
    {
        _protocolWindow?.AddOutput(message);
    }

    public IScreen HostScreen { get; }

    public string UrlPathSegment => "/engines";

    private double _depthSearch = 10;
    public double DepthSearch
    {
        get => _depthSearch;
        set
        {
            _depthSearch = value;
            _uciConnector.Depth = (int)_depthSearch;
            OnPropertyChanged();
        }
    }

    private double _nodesCount = 100000;
    public double NodesCount
    {
        get => _nodesCount;
        set
        {
            _nodesCount = value;
            _uciConnector.Nodes = (int)_nodesCount;
            OnPropertyChanged();
        }
    }

    private bool _autoMove = true;
    public bool AutoMove
    {
        get => _autoMove;
        set
        {
            _autoMove = value;
            OnPropertyChanged();
        }
    }
}