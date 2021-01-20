using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;

namespace Cajetan.Infobar.ViewModels
{
    public class BatteryStatusOptionsViewModel : ModuleOptionsViewModelBase
    {
        private readonly ISettingsService _settingsService;

        private bool _showTime;

        public BatteryStatusOptionsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            DisplayName = "Battery Status";
            Description = "Shows the battery charge in percentage.";


            ShowTime = true;
        }

        public override EModuleType ModuleType => EModuleType.BatteryStatus;
        
        public bool ShowTime
        {
            get => _showTime;
            set => SetProperty(ref _showTime, value);
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

        public override void Save()
        {
            _settingsService.Set("Module_Battery_IsEnabled", IsEnabled);
            _settingsService.Set("Module_Battery_SortOrder", SortOrder);
            _settingsService.Set("Module_Battery_ShowText", ShowText);
            _settingsService.Set("Module_Battery_ShowTime", ShowTime);
        }
    }
}
