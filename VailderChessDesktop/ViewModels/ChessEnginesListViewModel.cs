using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReactiveUI;
using Splat;
using VailderChessDesktop.Game;

namespace VailderChessDesktop.ViewModels;

public class ChessEnginesListViewModel : ViewModelBase, IRoutableViewModel
{
    public ChessEnginesListViewModel(IScreen? hostScreen = null)
    {
        HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
        ChessEngines = new List<ChessEngine>();
        if (Directory.Exists(ResourceManager.ChessEnginesDirectoryPath))
        {
            var directoryInfo = new DirectoryInfo(ResourceManager.ChessEnginesDirectoryPath);
            var files = directoryInfo.GetFiles("");
            foreach (var file in files)
            {
                if (Path.GetExtension(file.FullName) != ".ini" && Path.GetExtension(file.FullName) != ".txt")
                    ChessEngines.Add(new ChessEngine(file.Name, file.FullName));
            }
        }
    }

    public List<ChessEngine> ChessEngines { get; }
    
    public IScreen HostScreen { get; }

    public string UrlPathSegment => "/chess_engines_list";
}