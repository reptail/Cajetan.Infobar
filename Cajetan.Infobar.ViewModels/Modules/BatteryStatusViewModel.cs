using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;

namespace Cajetan.Infobar.ViewModels
{
    public class BatteryStatusViewModel : ModuleViewModelBase
    {
        private ISettingsService _settingsService;
        private ISystemInfoService _systemInfoService;

        private bool _showTime;
        private string _status;

        public BatteryStatusViewModel(ISettingsService settings, ISystemInfoService systemInfo)
        {
            _settingsService = settings;
            _systemInfoService = systemInfo;

            ShowTime = true;
        }
        
        public override EModuleType ModuleType => EModuleType.BatteryStatus;

        public bool ShowTime
        {
            get => _showTime;
            set => SetProperty(ref _showTime, value);
        }

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public override void Update()
        {
            if (_settingsService.Contains("Module_Battery_IsEnabled"))
                IsEnabled = _settingsService.Get<bool>("Module_Battery_IsEnabled");

            if (_settingsService.Contains("Module_Battery_SortOrder"))
                SortOrder = _settingsService.Get<int>("Module_Battery_SortOrder");

            if (_settingsService.Contains("Module_Battery_ShowText"))
                ShowText = _settingsService.Get<bool>("Module_Battery_ShowText");

            if (_settingsService.Contains("Module_Battery_ShowTime"))
                ShowTime = _settingsService.Get<bool>("Module_Battery_ShowTime");
        }

        public override void RefreshData()
        {
            Status = _systemInfoService.BatteryStatusString;
        }
    }
}
