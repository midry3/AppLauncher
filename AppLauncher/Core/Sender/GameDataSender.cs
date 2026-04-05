using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLauncher.Core.Sender
{
    public class GameDataSender
    {
        public static Process? GameProcess;
        public static List<Bitmap?> Thumbnails = new();
        public static List<GameData> GameDataList = new();
        public static int CurrentIndex = 0;
        public static int BackIndex
        {
            get
            {
                if (GameDataList.Count == 0) return -1;
                return (CurrentIndex - 1 + GameDataList.Count) % GameDataList.Count;
            }
        }
        public static int NextIndex
        {
            get
            {
                if (GameDataList.Count == 0) return -1;
                return (CurrentIndex + 1) % GameDataList.Count;
            }
        }
        public static GameData CurrentGame
        {
            get => GameDataList[CurrentIndex];
        }
        public static bool IsPlaying
        {
            get => GameProcess != null;
        }

        public static async Task Launch()
        {
            GameProcess = CurrentGame.Launch();
            await GameProcess.WaitForExitAsync();
            GameProcess = null;
        }

        public static void LoadGames()
        {
            GameDataList.Clear();
            Thumbnails.Clear();
            foreach (var d in Directory.GetDirectories("."))
            {
                var data = GameData.Load(d);
                if (data != null)
                {
                    data.Directory = d;
                    GameDataList.Add(data);
                    if (string.IsNullOrEmpty(data.Thumbnail))
                    {
                        Thumbnails.Add(null);
                    }
                    else
                    {
                        try
                        {
                            Thumbnails.Add(new Bitmap(Path.Combine(d, data.Thumbnail)));
                        }
                        catch
                        {
                            Thumbnails.Add(null);
                        }
                    }
                }
            }
        }
    }
}
