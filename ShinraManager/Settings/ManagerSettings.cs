using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShinraManager.Settings
{
    public class ManagerSettings
    {
        private static ManagerSettings _instance;
        public static ManagerSettings Instance => _instance ?? (_instance = new ManagerSettings());

        public string ShinraMeterPath { get; set; }
        public string ShinraMeterAutorunWithTera { get; set; }
        public string ShinraMeterDefaultName { get; set; }
        public string ShinraMeterProcessName { get; set; }
        public string TccPath { get; set; }
        public string TccAutorunWithTera { get; set; }
        public string TccDefaultName { get; set; }
        public string TccProcessName { get; set; }
        public string ShinraManagerTaskName { get; set; }

        public ManagerSettings()
        {
            Refresh();
            ShinraMeterPath = GetSettings(nameof(ShinraMeterPath));
            ShinraMeterAutorunWithTera = GetSettings(nameof(ShinraMeterAutorunWithTera));
            ShinraMeterDefaultName = GetSettings(nameof(ShinraMeterDefaultName));
            ShinraMeterProcessName = GetSettings(nameof(ShinraMeterProcessName));
            TccPath = GetSettings(nameof(TccPath));
            TccAutorunWithTera = GetSettings(nameof(TccAutorunWithTera));
            TccDefaultName = GetSettings(nameof(TccDefaultName));
            TccProcessName = GetSettings(nameof(TccProcessName));
            ShinraManagerTaskName = GetSettings(nameof(ShinraManagerTaskName));
        }

        private readonly Configuration config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

        private string GetSettings(string key)
        {
            return config.AppSettings.Settings[key].Value;
        }

        private void Refresh()
        {
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void SetValue(string key, string value)
        {
            config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
