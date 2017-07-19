using System.Diagnostics;

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
            Process.Start(fullPath);
        }
    }
}