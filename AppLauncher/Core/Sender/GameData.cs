using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppLauncher.Core.Sender
{
    public class GameData
    {
        private const string FILE = "game.json";

        public string GameTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = string.Empty;
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

        public Process Launch()
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
            return Launcher.Launch.Start(Path.Combine(Directory, exe));
        }
    }
}
