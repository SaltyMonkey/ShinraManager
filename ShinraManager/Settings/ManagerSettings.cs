using System.ComponentModel;
using System.Configuration;
using System.Reflection;
using System.Runtime.CompilerServices;

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
            get => _tccFlagInTm;
            set { _tccFlagInTm = value; RaisePropertyChanged(); }
        }
        private bool _shinraFlagInTm;
        public bool ShinraFlagInTm
        {
            get => _shinraFlagInTm;
            set { _shinraFlagInTm = value; RaisePropertyChanged(); }
        }

        private string _shinraMeterPath;
        public string ShinraMeterPath
        {
            get => _shinraMeterPath;
            set { _shinraMeterPath = value; RaisePropertyChanged(); SetValue(nameof(ShinraMeterPath), _shinraMeterPath); }
        }
        private bool _shinraMeterAutorunWithTera;
        public bool ShinraMeterAutorunWithTera
        {
            get => _shinraMeterAutorunWithTera;
            set { _shinraMeterAutorunWithTera = value; RaisePropertyChanged(); SetValue(nameof(ShinraMeterAutorunWithTera), _shinraMeterAutorunWithTera); }
        }
      
        public string ShinraMeterDefaultName
        {
            get;set;
        }
        public string ShinraMeterProcessName
        {
            get;set;
        }
        private string _tccPath;
        public string TccPath
        {
            get => _tccPath;
            set { _tccPath = value; RaisePropertyChanged(); SetValue(nameof(TccPath), _tccPath); }
        }
        private bool _tccStartWithTera;
        public bool TccAutorunWithTera
        {
            get => _tccStartWithTera;
            set { _tccStartWithTera = value; RaisePropertyChanged(); SetValue(nameof(TccAutorunWithTera), _tccStartWithTera); }
        }
    
        public string TccDefaultName
        {
            get; set;
        }
        public string TccProcessName { get; set; }
        public string ShinraManagerTaskName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly Configuration _config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

        private string GetSettings(string key)
        {
            return _config.AppSettings.Settings[key].Value;
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
            _config.AppSettings.Settings[key].Value = value;
            _config.Save(ConfigurationSaveMode.Modified);
        }
        private void SetValue(string key, bool value)
        {
            _config.AppSettings.Settings[key].Value = value.ToString();
            _config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
