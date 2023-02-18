using System.Reactive;
using System.Windows.Input;
using ReactiveUI;
using VailderChessDesktop.ViewModels;

namespace VailderChessDesktop.Game;

public class ChessEngine
{
    public ChessEngine(string name, string path)
    {
        Name = name;
        Path = path;
        _beginGameCommand = ReactiveCommand.Create(
            BeginGame
        );
    }

    private void BeginGame()
    {
        ResourceManager.MainWindowViewModel.Router.Navigate
            .Execute(new ChessEnginesViewModel(null, this));
    }

    public string Name { get; set; }
    
    public string Path { get; set; }
    
    private readonly ReactiveCommand<Unit, Unit> _beginGameCommand;
    
    public ICommand BeginGameCommand => _beginGameCommand;
}