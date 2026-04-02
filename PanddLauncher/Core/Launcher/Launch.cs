using Avalonia.Controls.Platform;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanddLauncher.Core.Launcher
{
    public class Launch
    {
        public static async Task Start(string exe)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = exe,
                }
            };
            process.Start();
            await process.WaitForExitAsync();
        } 
    }
}
