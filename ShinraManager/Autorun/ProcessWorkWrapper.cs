using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShinraManager.Autorun
{
    internal static class ProcessWorkWrapper
    {
        public static bool CheckProcessInMemory(string process)
        {
            return Process.GetProcessesByName(process).Length > 0;
        }

        public static void StartProcess(string fullPath)
        {
            ProcessStartInfo prc = new ProcessStartInfo(fullPath)
            {
                WorkingDirectory = Path.GetDirectoryName(fullPath)
            };
            Process.Start(prc);
        }

        public static void JustStartProcess(string fullPath)
        {
           Process.Start(fullPath);
        }
        public static void KillProcess(string name)
        {
            var processesInMemory = Process.GetProcesses().
                                   Where(pr => pr.ProcessName == name);

            foreach (var process in processesInMemory)
            {
                process.Kill();
            }
        }
    }
}
