using System.Diagnostics;
using System.Reactive;
using System.Windows.Input;
using ReactiveUI;
using VailderChessDesktop.Game;

namespace VailderChessDesktop.ViewModels;

public class ChessEngineProtocolWindowViewModel : ViewModelBase
{
    public ChessEngineProtocolWindowViewModel()
    {
        
        _emitCommandLine =  ReactiveCommand.Create(
            () =>
            {
                Debug.WriteLine(CommandLineText);
                UciConnector?.EmitCommandLine(CommandLineText);
            }
        );
    }

    public UciConnector? UciConnector { set; private get; }

    private readonly ReactiveCommand<Unit, Unit> _emitCommandLine;
    
    public ICommand EmitCommandLine => _emitCommandLine;

    private string _commandLineText;
    
    public string CommandLineText
    {
        get => _commandLineText;
        set
        {
            _commandLineText = value;
            OnPropertyChanged();
        }
    }

}