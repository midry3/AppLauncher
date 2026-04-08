using AppLauncher.Core.Sender;
using Avalonia.Controls;
using MsBox.Avalonia;

namespace AppLauncher.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (GameDataSender.IsPlaying)
        {
            if (e.Key == Avalonia.Input.Key.Escape)
            {
                GameDataSender.GameProcess.Kill();
                GameDataSender.GameProcess = null;
            }
            return;
        }
        if (e.Key == Avalonia.Input.Key.Down)
        {
            (Content as MainView).Next();
        }
        else if (e.Key == Avalonia.Input.Key.Up)
        {
            (Content as MainView).Back();
        }
        else if (e.Key == Avalonia.Input.Key.Enter || e.Key == Avalonia.Input.Key.Space)
        {
            (Content as MainView).Launch();
        }
    }

    private async void Window_Closing(object? sender, WindowClosingEventArgs e)
    {
        if (!GameDataSender.IsPlaying) return;
        e.Cancel = true;
        var box = MessageBoxManager.GetMessageBoxStandard("終了", "まだゲームが起動中です\nゲームを終了しますか？", MsBox.Avalonia.Enums.ButtonEnum.YesNo);
        if (await box.ShowWindowDialogAsync(this) == MsBox.Avalonia.Enums.ButtonResult.Yes)
        {
            if (GameDataSender.IsPlaying)
            {
                GameDataSender.GameProcess.Kill();
                GameDataSender.GameProcess = null;
            }
            Close();
        }
    }
}
