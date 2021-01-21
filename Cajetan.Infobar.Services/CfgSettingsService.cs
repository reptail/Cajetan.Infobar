using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Cajetan.Infobar.Services
{
    public class CfgSettingsService : ISettingsService
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly IWindowService _windowService;

        private readonly string _settingsFilePath;

        private IDictionary<string, object> _settings;

        public event EventHandler<IEnumerable<string>> SettingsUpdated;

        public CfgSettingsService(IFileSystemService fileSystemService, IWindowService windowService)
        {
            _fileSystemService = fileSystemService;
            _windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));

            _settingsFilePath = _fileSystemService.PathCombineWithAppData("Settings.cfg");

            Initialize();
        }

        private void Initialize()
        {
            // Create new settings container
            _settings = new Dictionary<string, object>();

            // Load from disk
            LoadFromDisk();

            // Set default values, if none exists
            SupplimentWithDefaults();

            // Debug, Save to disk after init with default values
            //SaveToDisk();
        }

        private void SupplimentWithDefaults()
        {
            // General
            SetDefault("General_BackgroundColor", "#FF3B3B3B");
            SetDefault("General_ForegroundColor", "#FFF5F5F5");
            SetDefault("General_BorderColor", "#FF5E6F7F");
            SetDefault("General_RefreshInterval_Milliseconds", 500);

            // Modules
            SetDefault("Module_Uptime_IsEnabled", true);
            SetDefault("Module_Uptime_SortOrder", 1);
            SetDefault("Module_Uptime_ShowText", true);
            SetDefault("Module_Uptime_ShowDays", false);

            SetDefault("Module_Battery_IsEnabled", false);
            SetDefault("Module_Battery_SortOrder", 2);
            SetDefault("Module_Battery_ShowText", true);
            SetDefault("Module_Battery_ShowTime", true);

            SetDefault("Module_Processor_IsEnabled", true);
            SetDefault("Module_Processor_SortOrder", 3);
            SetDefault("Module_Processor_ShowText", true);
            SetDefault("Module_Processor_ShowGraph", true);

            SetDefault("Module_Memory_IsEnabled", true);
            SetDefault("Module_Memory_SortOrder", 4);
            SetDefault("Module_Memory_ShowText", true);
            SetDefault("Module_Memory_ShowGraph", true);

            SetDefault("Module_Network_IsEnabled", true);
            SetDefault("Module_Network_SortOrder", 5);
            SetDefault("Module_Network_DisplayFormat", ENetworkDisplayFormat.Auto);
        }

        private void SetDefault<T>(string key, T value)
        {
            if (!Contains(key))
                Set(key, value);
        }

        private void SaveToDisk()
        {
            try
            {
                IEnumerable<string> settings = _settings.Select(s => string.Format("{0} {1}", s.Key, s.Value));
                string text = string.Join("\n", settings);

                string backupFile = $"{_settingsFilePath}.bak";
                _fileSystemService.Copy(_settingsFilePath, backupFile);

                _fileSystemService.Write(_settingsFilePath, text);
            }
            catch (Exception ex)
            {
                _windowService.Alert("Failed to Save settings!", ex.ToString());
            }
        }

        private void LoadFromDisk()
        {
            try
            {
                string text = _fileSystemService.Read(_settingsFilePath);

                if (string.IsNullOrWhiteSpace(text))
                    return;

                string[] lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string l in lines)
                {
                    string[] data = l.Split(' ');
                    string key = data[0];
                    string value = data[1];

                    _settings.Add(key, value);
                }
            }
            catch (Exception ex)
            {
                _windowService.Alert("Failed to Load settings!", ex.ToString());
            }
        }

        public bool Contains(string key)
        {
            return _settings.ContainsKey(key);
        }

        public T Get<T>(string key)
        {
            if (!Contains(key))
                return default;

            if (typeof(T).IsEnum)
                return (T)Enum.Parse(typeof(T), _settings[key].ToString());

            return (T)Convert.ChangeType(_settings[key], typeof(T));
        }

        public void Set<T>(string key, T value)
        {
            _settings[key] = value;
        }

        public void SaveChanges()
        {
            try
            {
                // Save to disk
                SaveToDisk();

                // Raise event
                RaiseSettingsUpdated(null);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to save changes to disk!", ex);
            }
        }

        public void RaiseSettingsUpdated(IEnumerable<string> updatedKeys)
        {
            if (updatedKeys == null || !updatedKeys.Any())
                updatedKeys = _settings.Select(s => s.Key).ToList();

            SettingsUpdated?.Invoke(this, updatedKeys);
        }

    }
}
