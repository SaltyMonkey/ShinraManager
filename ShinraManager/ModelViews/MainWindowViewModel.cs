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
using ShinraManager.Model;
using NLog;

namespace ShinraManager.ModelViews
{
    internal class MainWindowViewModel : INotifyPropertyChanged, IDisposable, IClosingHack
    {
        //private bool DebugLog = false;

        private readonly SettingsManager mngr = new SettingsManager();
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        public DelegateCommand ChooseShinraMeterExePath { get; set; }
        public DelegateCommand ShinraButtonCommand { get; set; }
        public DelegateCommand ChooseTccExePath { get; set; }
        public DelegateCommand TccButtonCommand { get; set; }
        public DelegateCommand ClosingComm { get; set; }
        public DelegateCommand ShowWindowComm { get; set; }

        private WMIProcessWatcher wtch;

        public string TeraProcessName { get; } = "Tera.exe";

        //Program settings
        private TccInfo TccConfiguration = new TccInfo();

        private ShinraMeterInfo ShinraMeterConfiguration = new ShinraMeterInfo();
        //----------------

        public MainWindowViewModel()
        {
            //Settings init
            RefreshSettingsSection();
            LoadShinraMeterConfig();
            LoadTccConfig();
            LoadManagerSettings();
            //Init UI commands
            UiDelegateCommandsInit();
            //Init program
            LogicInit();
        }

        private void LoadManagerSettings()
        {
            ShinraManagerTaskName = mngr.GetSettings(ShinraManagerSetting.ShinraManagerTaskName);
            ShinraManagerTaskDescription = mngr.GetSettings(ShinraManagerSetting.ShinraManagerTaskName);
        }

        private void UiDelegateCommandsInit()
        {
            ChooseShinraMeterExePath = new DelegateCommand(chooseShinraPathBody);
            ChooseTccExePath = new DelegateCommand(chooseTccPathBody);
            ShinraButtonCommand = new DelegateCommand(shinraButtonCommBody);
            TccButtonCommand = new DelegateCommand(tccButtonCommBody);
            ClosingComm = new DelegateCommand(closeComm);
            ShowWindowComm = new DelegateCommand(showWindowComm);
        }

        private void LoadShinraMeterConfig()
        {
            try
            {
                ShinraPath = mngr.GetSettings(ShinraManagerSetting.ShinraMeterPath);
                ShinraMeterRunWithTera = bool.Parse(mngr.GetSettings(ShinraManagerSetting.ShinraMeterAutorunWithTera));
                ShinraMeterConfiguration.ShinraDefaultName = mngr.GetSettings(ShinraManagerSetting.ShinraMeterDefaultName);
                ShinraMeterConfiguration.ShinraProcessName = mngr.GetSettings(ShinraManagerSetting.ShinraMeterProcessName);
            }
            catch (Exception ex)
            {
                log.Error(ex, "LoadShinraMeterConfig");
            }
        }

        private void LoadTccConfig()
        {
            try
            {
                TccPath = mngr.GetSettings(ShinraManagerSetting.TccPath);
                TccRunWithTera = bool.Parse(mngr.GetSettings(ShinraManagerSetting.TccAutorunWithTera));
                TccConfiguration.TccDefaultName = mngr.GetSettings(ShinraManagerSetting.TccDefaultName);
                TccConfiguration.TccProcessName = mngr.GetSettings(ShinraManagerSetting.TccProcessName);
            }
            catch (Exception ex)
            {
                log.Error(ex, "LoadTccConfig");
            }
        }

        private void RefreshSettingsSection()
        {
            mngr.Refresh();
        }

        public string ShinraPath
        {
            get { return ShinraMeterConfiguration.ShinraPath; }
            set
            {
                ShinraMeterConfiguration.ShinraPath = value;
                RaisePropertyChanged();
            }
        }

        public bool ShinraMeterRunWithTera
        {
            get { return ShinraMeterConfiguration.ShinraMeterRunWithTera; }
            set
            {
                ShinraMeterConfiguration.ShinraMeterRunWithTera = value;
                RaisePropertyChanged();
            }
        }

        public string TccPath
        {
            get { return TccConfiguration.TccPath; }
            set
            {
                TccConfiguration.TccPath = value;
                RaisePropertyChanged();
            }
        }

        public bool TccRunWithTera
        {
            get { return TccConfiguration.TccRunWithTera; }
            set
            {
                TccConfiguration.TccRunWithTera = value;
                RaisePropertyChanged();
            }
        }

        public string ShinraManagerTaskDescription { get; set; }
        public string ShinraManagerTaskName { get; set; }

        private void LogicInit()
        {
            if (wtch == null)
            {
                wtch = new WMIProcessWatcher(TeraProcessName);
            }
            else
            {
                wtch.RemoveWatchCreateProcessEvent();
            }
            if (((!string.IsNullOrWhiteSpace(ShinraMeterConfiguration.ShinraPath)) && (ShinraMeterConfiguration.ShinraMeterRunWithTera)) ||
                ((!string.IsNullOrWhiteSpace(TccConfiguration.TccPath)) && (TccConfiguration.TccRunWithTera)))
            {
                CleanUpTaskSheduler();
                AddWorksIntoTaskSheduler();
                try
                {
                    wtch.AddWatchCreateProcessEvent(processesStartBody);
                }
                catch (Exception ex)
                {
                    log.Error(ex, "LogicInit -> AddWatchCreateProcessEvent");
                }
            }
            else
            {
                CleanUpTaskSheduler();
            }
        }

        private void processesStartBody(object sender, EventArrivedEventArgs e)
        {
            if (ShinraMeterRunWithTera)
            {
                if (!ProcessCommandsWrapper.CheckProcessInMemory(ShinraMeterConfiguration.ShinraProcessName))
                {
                    try
                    {
                        ProcessCommandsWrapper.StartProcess(ShinraMeterConfiguration.ShinraPath);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex, "ShinraMeter process start exception");
                    }
                }
            }
            if (TccRunWithTera)
            {
                if (!ProcessCommandsWrapper.CheckProcessInMemory(TccConfiguration.TccProcessName))
                {
                    try
                    {
                        ProcessCommandsWrapper.StartProcess(TccConfiguration.TccPath);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex, "TCC process start exception");
                    }
                }
            }
        }

        private void shinraButtonCommBody()
        {
            ShinraMeterRunWithTera = !ShinraMeterRunWithTera;
            try
            {
                mngr.SetValue(ShinraManagerSetting.ShinraMeterAutorunWithTera, ShinraMeterRunWithTera.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex, "shinraButtonCommBody");
            }
            LogicInit();
        }

        private void tccButtonCommBody()
        {
            TccRunWithTera = !TccRunWithTera;
            try
            {
                mngr.SetValue(ShinraManagerSetting.TccAutorunWithTera, TccRunWithTera.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex, "tccButtonCommBody");
            }
            LogicInit();
        }

        private void CleanUpTaskSheduler()
        {
            try
            {
                if (WindowsTaskShedulerWrapper.GetTask(ShinraManagerTaskName))
                {
                    WindowsTaskShedulerWrapper.DeleteTaskByName(ShinraManagerTaskName);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "CleanUpTaskSheduler");
            }
        }

        private void AddWorksIntoTaskSheduler()
        {
            try
            {
                WindowsTaskShedulerWrapper.CreateTaskForUserLogonWithAdminRights(System.Reflection.Assembly.GetEntryAssembly().Location,
                    ShinraManagerTaskName, ShinraManagerTaskDescription, "-minimized");
            }
            catch (Exception ex)
            {
                log.Error(ex, "AddWorksIntoTaskSheduler");
            }
        }

        private void chooseShinraPathBody()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = $"ShinraMeter binary|{ShinraMeterConfiguration.ShinraDefaultName}";
            if (openFileDialog.ShowDialog() == true)
            {
                ShinraPath = openFileDialog.FileName;
            }
            try
            {
                mngr.SetValue(ShinraManagerSetting.ShinraMeterPath, ShinraMeterConfiguration.ShinraPath);
            }
            catch (Exception ex)
            {
                log.Error(ex, "chooseShinraPathBody");
            }
        }

        private void chooseTccPathBody()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = $"Tcc binary|{TccConfiguration.TccDefaultName}";
            if (openFileDialog.ShowDialog() == true)
            {
                TccPath = openFileDialog.FileName;
            }
            try
            {
                mngr.SetValue(ShinraManagerSetting.TccPath, TccConfiguration.TccPath);
            }
            catch (Exception ex)
            {
                log.Error(ex, "chooseTccPathBody");
            }
        }

        #region MVVM related

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion MVVM related

        public void Dispose()
        {
            wtch.Dispose();
            wtch = null;
        }

        private void showWindowComm()
        {
            WindowVisibility = "Visible";
        }

        private bool CloseAllowed = false;

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

        public bool OnClosing()
        {
            if (!CloseAllowed)
                WindowVisibility = "Hidden";

            return CloseAllowed;
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
    }
}