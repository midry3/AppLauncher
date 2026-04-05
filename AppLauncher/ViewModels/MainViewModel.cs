using AppLauncher.Core.Sender;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ReactiveUI;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AppLauncher.ViewModels;

public class MainViewModel : ViewModelBase
{
    public string NoticePlaying => "\n\nゲームが起動中です...\nEscキーを押すとゲームを終了します";
    private string _GameTitle1 = string.Empty;
    public string GameTitle1
    {
        get => _GameTitle1;
        set
        {
            this.RaiseAndSetIfChanged(ref _GameTitle1, value);
        }
    }
    private string _GameTitle2 = string.Empty;
    public string GameTitle2
    {
        get => _GameTitle2;
        set
        {
            this.RaiseAndSetIfChanged(ref _GameTitle2, value);
        }
    }
    private string _GameTitle3 = string.Empty;
    public string GameTitle3
    {
        get => _GameTitle3;
        set
        {
            this.RaiseAndSetIfChanged(ref _GameTitle3, value);
        }
    }
    private Bitmap? _Thumbnail;
    public Bitmap? Thumbnail
    {
        get => _Thumbnail;
        set
        {
            if (GameDataSender.GameDataList.Count > 0)
            {
                this.RaiseAndSetIfChanged(ref _Thumbnail, value);
            }
            else
            {
                this.RaiseAndSetIfChanged(ref _Thumbnail, null);
            }
        }
    }

    public MainViewModel()
    {
        Load();
    }

    public void UpdateGameList()
    {
        if (GameDataSender.GameDataList.Count > 1)
        {
            GameTitle1 = GameDataSender.GameDataList[GameDataSender.CurrentIndex == 0 ? GameDataSender.GameDataList.Count - 1 : GameDataSender.CurrentIndex - 1].GameTitle;
        }
        if (GameDataSender.GameDataList.Count > 0)
        {
            GameTitle2 = GameDataSender.GameDataList[GameDataSender.CurrentIndex].GameTitle;
        }
        if (GameDataSender.GameDataList.Count > 1)
        {
            GameTitle3 = GameDataSender.GameDataList[(GameDataSender.CurrentIndex + 1) % GameDataSender.GameDataList.Count].GameTitle;
        }
        Thumbnail = GameDataSender.Thumbnails[GameDataSender.CurrentIndex];
    }

    private void Load()
    {
        GameDataSender.LoadGames();
        UpdateGameList();
    }
}
