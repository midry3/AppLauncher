using Avalonia.Controls.Platform;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
