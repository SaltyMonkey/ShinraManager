using Microsoft.Win32.TaskScheduler;
using System;

namespace ShinraManager.Autorun
{
    public static class WindowsTaskShedulerWrapper
    {
        public static void CreateTaskForUserLogonWithAdminRights(string fileFullPath, string taskName, string description, string args)
        {
            try
            {
                // Get the service on the local machine
                using (var ts = new TaskService())
                {
                    // Create a new task definition and assign properties
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Description = description;
                    td.Principal.LogonType = TaskLogonType.InteractiveToken;
                    //Run with Admin rights
                    td.Principal.RunLevel = TaskRunLevel.Highest;
                    // Add a trigger that will fire after user logon
                    td.Triggers.Add(new LogonTrigger
                    {
                        UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,

                        Delay = TimeSpan.FromMilliseconds(1000)
                    });

                    // Add an action that will launch Notepad whenever the trigger fires
                    td.Actions.Add(new ExecAction(fileFullPath, args));

                    // Register the task in the root folder
                    ts.RootFolder.RegisterTaskDefinition(taskName, td);
                }
            }
            catch
            {
                //TODO
            }
        }

        public static void DeleteTaskByName(string taskName)
        {
            // Get the service on the local machine
            try
            {
                using (var ts = new TaskService())
                {
                    ts.RootFolder.DeleteTask(taskName);
                }
            }
            catch
            {
                //TODO
            }
        }

        public static bool GetTask(string taskName)
        {
            using (var ts = new TaskService())
            {
                Task t = ts.GetTask(taskName);
                return t != null;
            }
        }
    }
}