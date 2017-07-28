using ShinraManager.Helpers.Settings.Enums;
using System.Configuration;
using System.Reflection;

namespace ShinraManager.Helpers.Settings.Enums
{
    public enum ShinraManagerSetting
    {
        ShinraMeterPath,
        AutorunWithTera,
        ShinraManagerTaskName,
        ShinraManagerTaskDescription,
        ShinraMeterDefaultName,
        ShinraProcessName
    }
}

namespace ShinraManager.Helpers.Settings
{
    internal class SettingsManager
    {
        private readonly Configuration config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

        public string GetSettings(ShinraManagerSetting key)
        {
            return config.AppSettings.Settings[key.ToString()].Value;
        }

        public void Refresh()
        {
            ConfigurationManager.RefreshSection("appSettings");
        }

        public void SetValue(ShinraManagerSetting key, string value)
        {
            config.AppSettings.Settings[key.ToString()].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}