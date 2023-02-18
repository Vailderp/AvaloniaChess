using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace VailderChessDesktop.UserControls;

public partial class FastBeginGameUserControl : UserControl
{
    public FastBeginGameUserControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}