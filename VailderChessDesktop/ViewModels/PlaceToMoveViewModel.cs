using System;
using Avalonia.Media;
using VailderChessDesktop.Game;

namespace VailderChessDesktop.ViewModels;

public class PlaceToMoveViewModel : ViewModelBase
{
    public PlaceToMoveViewModel()
    {
        _keyResource = "ImgPlaceToMove";
    }

    public void SetFigureProperties(PlaceToMoveType placeToMoveType)
    {
        _keyResource = placeToMoveType switch
        {
            PlaceToMoveType.Empty => "ImgPlaceToMove",
            PlaceToMoveType.OnFigure => "ImgPlaceToMoveOnFigure",
            PlaceToMoveType.OnFriendlyFigure => "ImgPlaceToMoveOnFriendlyFigure",
            _ => throw new ArgumentOutOfRangeException(nameof(placeToMoveType), placeToMoveType, null)
        };
        OnPropertyChanged(nameof(ImgLocalPlaceToMove));
    }

    private string _keyResource;

    public ImageBrush? ImgLocalPlaceToMove =>
        ResourceManager.AppDesignChessResourceDictionary[_keyResource] as ImageBrush;
}