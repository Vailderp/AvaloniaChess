using Avalonia.Controls;
using Avalonia.Threading;
using MessageBox.Avalonia.DTO;

namespace VailderChessDesktop.Game;

public static class MessageBoxManager
{
    public static void CallMessageBox(string title, string message)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
            {
                var mb = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                    new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok, 
                        ContentTitle = title,
                        //ContentHeader = header,
                        ContentMessage = message,
                        Icon = MessageBox.Avalonia.Enums.Icon.Error,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        CanResize = true,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        ShowInCenter = true,
                    }
                );
                mb.Show();
            }
        );
    }

    public static void CallMessageBox(string titledMessage)
    {
        var strings = titledMessage.Split('|');
        if (strings.Length == 2)
            CallMessageBox(strings[0], strings[1]);
        else
            CallMessageBox("Error", titledMessage);
    }
}