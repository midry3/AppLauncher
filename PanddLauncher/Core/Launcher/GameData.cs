using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PanddLauncher.Core.Launcher
{
    public class GameData
    {
        private const string FILE = "game.json";

        public required string GameTitle { get; set; }
        public required string Description { get; set; }
        public required string Thumbnail {  get; set; }
        public string GameWindows { get; set; } = "game.exe";
        public string GameMac { get; set; } = "game";
        public string GameLinux { get; set; } = "game";

        public string Directory = string.Empty;

        public static GameData? Load(string path)
        {
            var filePath = Path.Combine(path, FILE);
            if (!File.Exists(filePath))
                return null;
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<GameData>(json);
        }

        public async Task Launch()
        {
            string exe;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                exe = GameWindows;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                exe = GameMac;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                exe = GameLinux;
            else
                throw new PlatformNotSupportedException("Unsupported OS platform.");
            await Launcher.Launch.Start(Path.Combine(Directory, exe));
        }
    }
}
