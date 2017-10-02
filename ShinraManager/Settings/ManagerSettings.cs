using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShinraManager.Settings
{
    public class ManagerSettings: INotifyPropertyChanged
    {
        private static ManagerSettings _instance;
        public static ManagerSettings Instance => _instance ?? (_instance = new ManagerSettings());
        public string TeraProcessName = "Tera.exe";
        private bool _tccFlagInTm;
        public bool TccFlagInTm
        {
            get { return _tccFlagInTm; }
            set { _tccFlagInTm = value; RaisePropertyChanged(); }
        }
        private bool _shinraFlagInTm;
        public bool ShinraFlagInTm
        {
            get { return _shinraFlagInTm; }
            set { _shinraFlagInTm = value; RaisePropertyChanged(); }
        }

        private string _shinraMeterPath;
        public string ShinraMeterPath
        {
            get { return _shinraMeterPath; }
            set { _shinraMeterPath = value; RaisePropertyChanged(); SetValue(nameof(ShinraMeterPath), _shinraMeterPath); }
        }
        private bool _shinraMeterAutorunWithTera;
        public bool ShinraMeterAutorunWithTera
        {
            get { return _shinraMeterAutorunWithTera; }
            set { _shinraMeterAutorunWithTera = value; RaisePropertyChanged(); SetValue(nameof(ShinraMeterAutorunWithTera), _shinraMeterAutorunWithTera); }
        }
        private string _shinraMeterDefaultName;
        public string ShinraMeterDefaultName
        {
            get { return _shinraMeterDefaultName; }
            set { _shinraMeterDefaultName = value; RaisePropertyChanged(); SetValue(nameof(ShinraMeterDefaultName), _shinraMeterDefaultName); }
        }
        private string _shinraMeterProcessName;
        public string ShinraMeterProcessName
        {
            get { return _shinraMeterProcessName; }
            set { _shinraMeterProcessName = value; RaisePropertyChanged(); SetValue(nameof(ShinraMeterProcessName), _shinraMeterProcessName); }
        }
        private string _tccPath;
        public string TccPath
        {
            get { return _tccPath; }
            set { _tccPath = value; RaisePropertyChanged(); SetValue(nameof(TccPath), _tccPath); }
        }
        private bool _tccStartWithTera;
        public bool TccAutorunWithTera
        {
            get { return _tccStartWithTera; }
            set { _tccStartWithTera = value; RaisePropertyChanged(); SetValue(nameof(TccAutorunWithTera), _tccStartWithTera); }
        }
        private string _tccDefaultName;
        public string TccDefaultName
        {
            get { return _tccDefaultName; }
            set { _tccDefaultName = value; RaisePropertyChanged(); SetValue(nameof(TccDefaultName), _tccDefaultName); }
        }

        public string TccProcessName { get; set; }

        public string ShinraManagerTaskName { get; set; }

        public ManagerSettings()
        {
           
         
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly Configuration config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

        private string GetSettings(string key)
        {
            return config.AppSettings.Settings[key].Value;
        }

        public void Refresh()
        {
            ConfigurationManager.RefreshSection("appSettings");
            ShinraMeterPath = GetSettings(nameof(ShinraMeterPath));
            ShinraMeterAutorunWithTera = bool.Parse(GetSettings(nameof(ShinraMeterAutorunWithTera)));
            ShinraMeterDefaultName = GetSettings(nameof(ShinraMeterDefaultName));
            ShinraMeterProcessName = GetSettings(nameof(ShinraMeterProcessName));
            TccPath = GetSettings(nameof(TccPath));
            TccAutorunWithTera = bool.Parse(GetSettings(nameof(TccAutorunWithTera)));
            TccDefaultName = GetSettings(nameof(TccDefaultName));
            TccProcessName = GetSettings(nameof(TccProcessName));
            ShinraManagerTaskName = GetSettings(nameof(ShinraManagerTaskName));
        }

        private void SetValue(string key, string value)
        {
            config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
        }
        private void SetValue(string key, bool value)
        {
            config.AppSettings.Settings[key].Value = value.ToString();
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
