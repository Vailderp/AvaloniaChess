using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using VailderChessDesktop.Game;
using ReactiveUI;

namespace VailderChessDesktop.ViewModels;

public class ChessFigureViewModel : ViewModelBase
{
    public ChessFigureViewModel()
    {
        _keyResource = "ImgChessFigureEmpty";
    }

    public void SetFigureProperties(FigureType figureType)
    {
        _keyResource = figureType switch
        {
            FigureType.BishopBlack => "ImgBishopBlack",
            FigureType.BishopWhite => "ImgBishopWhite",
            FigureType.KingBlack => "ImgKingBlack",
            FigureType.KingWhite => "ImgKingWhite",
            FigureType.KnightBlack => "ImgKnightBlack",
            FigureType.KnightWhite => "ImgKnightWhite",
            FigureType.PawnBlack => "ImgPawnBlack",
            FigureType.PawnWhite => "ImgPawnWhite",
            FigureType.QueenBlack => "ImgQueenBlack",
            FigureType.QueenWhite => "ImgQueenWhite",
            FigureType.RookBlack => "ImgRookBlack",
            FigureType.RookWhite => "ImgRookWhite",
            _ => throw new ArgumentOutOfRangeException(nameof(figureType), figureType, null)
        };
        OnPropertyChanged("ImgLocalChessFigure");
    }

    private string _keyResource; 
    
    
    public ImageBrush? ImgLocalChessFigure => 
        ResourceManager.AppDesignChessResourceDictionary[_keyResource] as ImageBrush;
}