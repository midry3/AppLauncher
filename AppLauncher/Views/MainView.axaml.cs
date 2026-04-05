using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using AppLauncher.Core.Launcher;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using AppLauncher.Core.Sender;
using AppLauncher.ViewModels;

namespace AppLauncher.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    public async void Launch()
    {
        if (GameDataSender.IsPlaying) return;
        BlockScreen.IsVisible = true;
        BlockScreen.IsHitTestVisible = true;
        try
        {
            await GameDataSender.Launch();
        }
        catch (FileNotFoundException)
        {

        }
        finally
        {
            BlockScreen.IsVisible = false;
            BlockScreen.IsHitTestVisible = false;
        }
    }

    public void Next()
    {
        Debug.WriteLine("Next!");
        SelectGame(GameDataSender.NextIndex);
    }

    public void Back()
    {
        SelectGame(GameDataSender.BackIndex);
    }

    private void SelectGame(int idx)
    {
        if (idx < 0) return;
        GameDataSender.CurrentIndex = idx;
        (DataContext as MainViewModel).UpdateGameList();
    }

    private void Border1_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        Back();
    }

    private void Border2_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            Launch();
        }
    }

    private void Border3_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        Next();
    }
}
