using System.Diagnostics;
using System.IO;

namespace ShinraManager.Autorun
{
    public static class ProcessCommandsWrapper
    {
        public static bool CheckProcessInMemory(string process)
        {
            return Process.GetProcessesByName(process).Length > 0;
        }

        public static void StartProcess(string fullPath)
        {
            ProcessStartInfo prc = new ProcessStartInfo(fullPath);
            prc.WorkingDirectory = Path.GetDirectoryName(fullPath);
            Process.Start(prc);
        }
    }
}