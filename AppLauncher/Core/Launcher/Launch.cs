using System.Diagnostics;
using System.IO;

namespace AppLauncher.Core.Launcher
{
    public class Launch
    {
        public static Process Start(string exe)
        {
            if (!File.Exists(exe))
            {
                throw new FileNotFoundException();
            }
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = exe,
                }
            };
            process.Start();
            return process;
        } 
    }
}
