using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using PanddLauncher.Core.Launcher;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace PanddLauncher.Views;

public partial class MainView : UserControl
{
    private const double ZOOM_SCALE = 1.2;
    private bool IsPlaying;
    private List<GameData> _GameDataList;
    private List<Bitmap> _Thumbnails;
    private int _CurrentIndex;
    public MainView()
    {
        InitializeComponent();
        _CurrentIndex = -1;
        AttachedToVisualTree += (sender, e) =>
        {
            var window = (VisualRoot as Window);
            window.KeyDown += (sender, e) =>
            {
                if (IsPlaying) return;
                if (e.Key == Avalonia.Input.Key.Down && GameTitleGrid.Children.Count > 1)
                {
                    SelectGame((_CurrentIndex + 1) % GameTitleGrid.Children.Count);
                }
                else if (e.Key == Avalonia.Input.Key.Up && GameTitleGrid.Children.Count > 1)
                {
                    if (_CurrentIndex == 0)
                    {
                        SelectGame(GameTitleGrid.Children.Count - 1);
                    }
                    else
                    {
                        SelectGame(_CurrentIndex - 1);
                    }
                }
                else if (e.Key == Avalonia.Input.Key.Enter || e.Key == Avalonia.Input.Key.Space)
                {
                    if (_GameDataList.Count > 0)
                    {
                        Launch();
                    }
                }
            };
        };
        Load();
        if (GameTitleGrid.Children.Count > 0)
        {
            SelectGame(0);
        }
    }

    private void  Load()
    {
        _GameDataList = new();
        _Thumbnails = new();
        GameTitleGrid.Children.Clear();
        GameTitleGrid.RowDefinitions.Clear();
        var idx = 0;
        foreach (var d in Directory.GetDirectories("."))
        {
            var i = idx;
            var data = Core.Launcher.GameData.Load(d);
            if (data != null)
            {
                data.Directory = d;
                _GameDataList.Add(data);
                _Thumbnails.Add(new Bitmap(Path.Combine(d, data.Thumbnail)));
                var panel = new StackPanel
                {
                    Width = 300,
                    Height = 200,
                };
                var label = new TextBlock
                {
                    Text = data.GameTitle,
                    Foreground = Brushes.White,
                    FontSize = 60,
                };
                panel.Children.Add(label);
                var border = new Border
                {
                    Width = 300,
                    Height = 200,
                    BorderBrush = Brushes.White,
                    Padding = new Thickness(5),
                    BorderThickness = new Thickness(2),
                    Child = panel,
                    Background = Brushes.Black
                };
                border.PointerPressed += (sender, e) =>
                {
                    if (IsPlaying) return;
                    if (e.ClickCount == 2)
                    {
                        Launch();
                    }
                    else
                    {
                        SelectGame(i);
                    }
                };
                GameTitleGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(250) });
                GameTitleGrid.Children.Add(border);
                Grid.SetRow(border, idx);
            }
            idx++;
        }
    }

    private async void Launch()
    {
        Thumbnail.Opacity = 0.5;
        IsPlaying = true;
        await _GameDataList[_CurrentIndex].Launch();
        Thumbnail.Opacity = 1.0;
        IsPlaying = false;
    }

    private void SelectGame(int idx)
    {
        if (_CurrentIndex >= 0)
        {
            UnZoomAt(_CurrentIndex);
        }
        ZoomAt(idx);
        Thumbnail.Source = _Thumbnails[idx];
        _CurrentIndex = idx;
    }

    private void ZoomAt(int idx)
    {
        var panel = GameTitleGrid.Children[idx] as Border;
        panel.RenderTransform = new ScaleTransform
        {
            ScaleX = ZOOM_SCALE,
            ScaleY = ZOOM_SCALE,
        };
    }

    private void UnZoomAt(int idx)
    {
        var panel = GameTitleGrid.Children[idx] as Border;
        panel.RenderTransform = new ScaleTransform
        {
            ScaleX = 1,
            ScaleY = 1,
        };
    }
}
