using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;

namespace Cajetan.Infobar.ViewModels
{
    public class UptimeViewModel : ModuleViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ISystemInfoService _systemInfoService;

        private bool _showDays;
        private string _uptime;

        public UptimeViewModel(ISettingsService settings, ISystemInfoService systemInfo)
        {
            _settingsService = settings;
            _systemInfoService = systemInfo;

            ShowDays = false;
        }

        public override EModuleType ModuleType => EModuleType.Uptime;

        public bool ShowDays
        {
            get => _showDays;
            set => SetProperty(ref _showDays, value);
        }

        public string Uptime
        {
            get => _uptime;
            set => SetProperty(ref _uptime, value);
        }

        public override void Update()
        {
            if (_settingsService.Contains("Module_Uptime_IsEnabled"))
                IsEnabled = _settingsService.Get<bool>("Module_Uptime_IsEnabled");

            if (_settingsService.Contains("Module_Uptime_SortOrder"))
                SortOrder = _settingsService.Get<int>("Module_Uptime_SortOrder");

            if (_settingsService.Contains("Module_Uptime_ShowText"))
                ShowText = _settingsService.Get<bool>("Module_Uptime_ShowText");

            if (_settingsService.Contains("Module_Uptime_ShowDays"))
                ShowDays = _settingsService.Get<bool>("Module_Uptime_ShowDays");
        }

        public override void RefreshData()
        {
            Uptime = _systemInfoService.UptimeString;
        }
    }
}
