using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using VailderChessDesktop.Game;
using VailderChessDesktop.ViewModels;

namespace VailderChessDesktop.UserControls;

public partial class ChessFigure : ReactiveUserControl<ChessFigureViewModel>
{
    public ChessFigure()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        ViewModel = new ChessFigureViewModel();
        this.WhenActivated((CompositeDisposable disposable) =>
        {
            this.GotFocus += (sender, args) =>
            {
                RoutedEventArgs routedEventArgs = new(routedEvent: SelectFigureEvent);
                RaiseEvent(routedEventArgs);
            };

            this.LostFocus += (sender, args) =>
            {
                RoutedEventArgs routedEventArgs = new(routedEvent: UnselectFigureEvent);
                RaiseEvent(routedEventArgs);
            };

            ViewModel.SetFigureProperties(FigureType);
        });
        AvaloniaXamlLoader.Load(this);
    }

    public static readonly RoutedEvent<RoutedEventArgs> SelectFigureEvent =             
        RoutedEvent.Register<ChessFigure, RoutedEventArgs>(nameof(SelectFigure), 
            RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
    
    public static readonly RoutedEvent<RoutedEventArgs> UnselectFigureEvent =             
        RoutedEvent.Register<ChessFigure, RoutedEventArgs>(nameof(UnselectFigure), 
            RoutingStrategies.Tunnel | RoutingStrategies.Bubble);

    public static readonly StyledProperty<FigureType> FigureTypeProperty =
        AvaloniaProperty.Register<ChessFigure, FigureType>(nameof(FigureType));

    public FigureType FigureType
    {
        get => GetValue(FigureTypeProperty);
        set
        {
            SetValue(FigureTypeProperty, value);
            ViewModel?.SetFigureProperties(FigureType);
        }
    }

    public event EventHandler<RoutedEventArgs>? SelectFigure
    {
        add { AddHandler(SelectFigureEvent, value); }
        remove { RemoveHandler(SelectFigureEvent, value); }
    }
    
    public event EventHandler<RoutedEventArgs>? UnselectFigure
    {
        add { AddHandler(UnselectFigureEvent, value); }
        remove { RemoveHandler(UnselectFigureEvent, value); }
    }
}