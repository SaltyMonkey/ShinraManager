using Microsoft.Win32;
using ShinraManager.Autorun;
using ShinraManager.Settings;
using ShinraManager.UI;
using System;
using System.Management;
using System.Windows;
using System.Windows.Input;

namespace ShinraManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            ManagerSettings.Instance.Refresh();
            InitializeComponent();
            ReadTaskSheduler();
            LogicInit();
        }

        public void ShowTaskbarIcon()
        {
            TaskbarIcon.Visibility = Visibility.Visible;
        }

        private static void ReadTaskSheduler()
        {
            ManagerSettings.Instance.ShinraFlagInTm = WindowsTaskShedulerWrapper.GetTask(ManagerSettings.Instance.ShinraManagerTaskName);
            ManagerSettings.Instance.TccFlagInTm = WindowsTaskShedulerWrapper.GetTask(ManagerSettings.Instance.ShinraManagerTaskName);
        }

        private WMIWrapper _wtch;
        private void Logo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GitHubComm();
        }

        private void GitHubComm(object sender, RoutedEventArgs e)
        {
            ProcessWorkWrapper.JustStartProcess("https://github.com/SaltyMonkey/ShinraManager");
        }
        private void GitHubComm()
        {
            ProcessWorkWrapper.JustStartProcess("https://github.com/SaltyMonkey/ShinraManager");
        }

        private void ClosingComm(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ShowWindowComm(object sender, RoutedEventArgs e)
        {
            TaskbarIcon.Visibility = Visibility.Hidden;
            Show();
        }
        private void HideWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Hide();
            TaskbarIcon.Visibility = Visibility.Visible;
        }

        private void ShinraChoosePathB_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = $"ShinraMeter binary|{ManagerSettings.Instance.ShinraMeterDefaultName}"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                ManagerSettings.Instance.ShinraMeterPath = openFileDialog.FileName;
            }
        
        }

        private void TccChoosePathB_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = $"Tcc binary|{ManagerSettings.Instance.TccDefaultName}"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                ManagerSettings.Instance.TccPath = openFileDialog.FileName;
            }
          
        }

        private void ShinraClosePrB_Click(object sender, RoutedEventArgs e)
        {
            ProcessWorkWrapper.KillProcess(ManagerSettings.Instance.ShinraMeterDefaultName);
        }

        private void TccClosePrB_Click(object sender, RoutedEventArgs e)
        {
            ProcessWorkWrapper.KillProcess(ManagerSettings.Instance.TccDefaultName);
        }

        private void ShinraMainButton_Click(object sender, RoutedEventArgs e)
        {
            ManagerSettings.Instance.ShinraMeterAutorunWithTera = !ManagerSettings.Instance.ShinraMeterAutorunWithTera;
            LogicInit();
        }

        private void LogicInit()
        {
          
            if (_wtch == null)
            {
                _wtch = new WMIWrapper(ManagerSettings.Instance.TeraProcessName);
            }
            else
            {
                _wtch.RemoveWatchCreateProcessEvent();
            }
            if (((!string.IsNullOrWhiteSpace(ManagerSettings.Instance.ShinraMeterPath)) && (ManagerSettings.Instance.ShinraMeterAutorunWithTera)) ||
                ((!string.IsNullOrWhiteSpace(ManagerSettings.Instance.TccPath)) && (ManagerSettings.Instance.TccAutorunWithTera)))
            {
                CleanUpTaskSheduler();
                AddWorksIntoTaskSheduler();
                try
                {
                    _wtch.AddWatchCreateProcessEvent(ProcessesStartBody);
                }
                catch (Exception ex)
                {
                    //log.Error(ex, "LogicInit -> AddWatchCreateProcessEvent");
                }
            }
            else
            {
                CleanUpTaskSheduler();
            }
            ReadTaskSheduler();
        }

        private void ProcessesStartBody(object sender, EventArrivedEventArgs e)
        {
            if (ManagerSettings.Instance.ShinraMeterAutorunWithTera &&
                !ProcessWorkWrapper.CheckProcessInMemory(ManagerSettings.Instance.ShinraMeterProcessName))
                try
                {
                    ProcessWorkWrapper.StartProcess(ManagerSettings.Instance.ShinraMeterPath);
                }
                catch (Exception ex)
                {
                    //log.Error(ex, "ShinraMeter process start exception");
                }
            if (ManagerSettings.Instance.TccAutorunWithTera &&
                !ProcessWorkWrapper.CheckProcessInMemory(ManagerSettings.Instance.TccProcessName))
                try
                {
                    ProcessWorkWrapper.StartProcess(ManagerSettings.Instance.TccPath);
                }
                catch (Exception ex)
                {
                    // log.Error(ex, "TCC process start exception");
                }
        }

        private static void CleanUpTaskSheduler()
        {
            try
            {
                if (WindowsTaskShedulerWrapper.GetTask(ManagerSettings.Instance.ShinraManagerTaskName))
                {
                    WindowsTaskShedulerWrapper.DeleteTaskByName(ManagerSettings.Instance.ShinraManagerTaskName);
                }
            }
            catch (Exception ex)
            {
                //log.Error(ex, "CleanUpTaskSheduler");
            }
        }
        private static void AddWorksIntoTaskSheduler()
        {
            try
            {
                WindowsTaskShedulerWrapper.CreateTaskForUserLogonWithAdminRights(System.Reflection.Assembly.GetEntryAssembly().Location,
                    ManagerSettings.Instance.ShinraManagerTaskName, ManagerSettings.Instance.ShinraManagerTaskName, "-minimized");
            }
            catch (Exception ex)
            {
                //log.Error(ex, "AddWorksIntoTaskSheduler");
            }
        }
        private void TccMainButton_Click(object sender, RoutedEventArgs e)
        {
            ManagerSettings.Instance.TccAutorunWithTera = !ManagerSettings.Instance.TccAutorunWithTera;
            LogicInit();
        }

        private void ShinraMeterDiscord_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ProcessWorkWrapper.JustStartProcess("https://discord.gg/anUXQTp");
        }

        private void ShinraMeterGithub_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            ProcessWorkWrapper.JustStartProcess("https://github.com/neowutran/ShinraMeter");
        }

        private void TccGithub_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ProcessWorkWrapper.JustStartProcess("https://github.com/Foglio1024/Tera-custom-cooldowns");
        }
    }
}
