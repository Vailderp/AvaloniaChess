using System.Collections.Generic;
using ReactiveUI;
using Splat;
using VailderChessDesktop.Game;
using VailderChessDesktop.Network.Common;

namespace VailderChessDesktop.ViewModels;

public class GameRoomsViewModel : ViewModelBase, IRoutableViewModel
{
    public GameRoomsViewModel(IScreen? hostScreen = null)
    {
        HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
    }

    public List<Room> Rooms => ResourceManager.Rooms;

    public IScreen HostScreen { get; }

    public string UrlPathSegment => "/game_rooms";
}