using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using VailderChessDesktop.Game;
using VailderChessDesktop.ViewModels;

namespace VailderChessDesktop.UserControls;

public partial class PlaceToMove : ReactiveUserControl<PlaceToMoveViewModel>
{
    public PlaceToMove()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        ViewModel = new PlaceToMoveViewModel();
        
        this.WhenActivated((CompositeDisposable disposable) =>
        {
            ViewModel.SetFigureProperties(PlaceToMoveType);
        });
        
        AvaloniaXamlLoader.Load(this);
    }
    
    public static readonly StyledProperty<PlaceToMoveType> PlaceToMoveTypeProperty =
        AvaloniaProperty.Register<ChessFigure, PlaceToMoveType>(nameof(PlaceToMoveType));
    
    public PlaceToMoveType PlaceToMoveType
    {
        get => GetValue(PlaceToMoveTypeProperty);
        set
        {
            SetValue(PlaceToMoveTypeProperty, value);
            ViewModel?.SetFigureProperties(value);
        }
    }
    
}