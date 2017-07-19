using Microsoft.Win32;
using ShinraManager.Autorun;
using ShinraManager.Helpers;
using ShinraManager.Helpers.Settings;
using ShinraManager.Helpers.Settings.Enums;
using System;
using System.ComponentModel;
using System.Management;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ShinraManager.ModelViews
{
    internal class MainWindowViewModel : INotifyPropertyChanged, IDisposable, IClosingHack
    {
        public MainWindowViewModel()
        {
            mngr.Refresh();
            ShinraPath = mngr.GetSettings(ShinraManagerSetting.ShinraMeterPath);
            RunWithTera = bool.Parse(mngr.GetSettings(ShinraManagerSetting.AutorunWithTera));
            ShinraManagerTaskName = mngr.GetSettings(ShinraManagerSetting.ShinraManagerTaskName);
            ShinraManagereTaskDescription = mngr.GetSettings(ShinraManagerSetting.ShinraManagerTaskName);
            ShinraDefaultName = mngr.GetSettings(ShinraManagerSetting.ShinraMeterDefaultName);
            ChooseShinraMeterExePath = new DelegateCommand(chooseShinraPathBody);
            MainButtonCommand = new DelegateCommand(mainButtonCommBody);
            ClosingComm = new DelegateCommand(closeComm);
            ShowWindowComm = new DelegateCommand(showWindowComm);
            Init();
        }

        private string _windowVisibility;

        public string WindowVisibility
        {
            get { return _windowVisibility; }
            set
            {
                _windowVisibility = value;
                RaisePropertyChanged();
            }
        }

        private SettingsManager mngr = new SettingsManager();
        public DelegateCommand ChooseShinraMeterExePath { get; set; }
        public DelegateCommand MainButtonCommand { get; set; }
        public DelegateCommand ClosingComm { get; set; }
        public DelegateCommand ShowWindowComm { get; set; }

        private WMIProcessWatcher wtch;
        private string _shinraPath;

        public string ShinraPath
        {
            get { return _shinraPath; }
            set
            {
                _shinraPath = value;
                RaisePropertyChanged();
            }
        }

        public string ShinraManagereTaskDescription { get; set; }
        public string ShinraManagerTaskName { get; set; }
        public string ShinraDefaultName { get; set; }
        public string TeraProcessName { get; set; } = "Tera.exe";
        private bool _runWithTera;

        public bool RunWithTera
        {
            get { return _runWithTera; }
            set
            {
                _runWithTera = value;
                RaisePropertyChanged();
            }
        }

        private void showWindowComm()
        {
            WindowVisibility = "Visible";
        }

        private void closeComm()
        {
            if (wtch != null)
            {
                wtch.RemoveWatchCreateProcessEvent();
            }
            CloseAllowed = true;
            Application.Current.Shutdown(0);
        }

        private bool CloseAllowed = false;

        private void Init()
        {
            wtch = new WMIProcessWatcher(TeraProcessName);
            if ((!string.IsNullOrEmpty(ShinraPath)) && (RunWithTera))
            {
                wtch.AddWatchCreateProcessEvent(shinraProcessStartBody);
            }
        }

        private void shinraProcessStartBody(object sender, EventArrivedEventArgs e)
        {
            if (!ProcessCommandsWrapper.CheckProcessInMemory(ShinraDefaultName))
            {
                ProcessCommandsWrapper.StartProcess(ShinraPath);
            }
        }

        private void mainButtonCommBody()
        {
            if (RunWithTera)
            {
                CleanUpTaskSheduler();
                wtch.RemoveWatchCreateProcessEvent();
            }
            else
            {
                CleanUpTaskSheduler();
                AddWorksIntoTaskSheduler();
                wtch.AddWatchCreateProcessEvent(shinraProcessStartBody);
            }
            RunWithTera = !RunWithTera;
        }

        private void CleanUpTaskSheduler()
        {
            if (WindowsTaskShedulerWrapper.GetTask(ShinraManagerTaskName))
            {
                WindowsTaskShedulerWrapper.DeleteTaskByName(ShinraManagerTaskName);
            }
        }

        private void AddWorksIntoTaskSheduler()
        {
            WindowsTaskShedulerWrapper.CreateTaskForUserLogonWithAdminRights(System.Reflection.Assembly.GetEntryAssembly().Location,
                ShinraManagerTaskName, ShinraManagereTaskDescription, "-minimized");
        }

        private void chooseShinraPathBody()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = $"ShinraMeter binary|{ShinraDefaultName}";
            if (openFileDialog.ShowDialog() == true)
            {
                ShinraPath = openFileDialog.FileName;
            }
            mngr.SetValue(ShinraManagerSetting.ShinraMeterPath, ShinraPath);
        }

        public void Dispose()
        {
            wtch.Dispose();
            wtch = null;
        }

        #region MVVM related

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool OnClosing()
        {
            if (!CloseAllowed)
                WindowVisibility = "Hidden";

            return CloseAllowed;
        }

        #endregion MVVM related
    }
}