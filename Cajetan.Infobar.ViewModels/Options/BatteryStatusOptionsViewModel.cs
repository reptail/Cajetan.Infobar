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
            if (_settingsService.TryGet(SettingsKeys.BATTERY_IS_ENABLED, out bool isEnabled))
                IsEnabled = isEnabled;

            if (_settingsService.TryGet(SettingsKeys.BATTERY_SORT_ORDER, out int sortOrder))
                SortOrder = sortOrder;

            if (_settingsService.TryGet(SettingsKeys.BATTERY_SHOW_TEXT, out bool showText))
                ShowText = showText;

            if (_settingsService.TryGet(SettingsKeys.BATTERY_SHOW_TIME, out bool showTime))
                ShowTime = showTime;
        }

        public override void Save()
        {
            _settingsService.Set(SettingsKeys.BATTERY_IS_ENABLED, IsEnabled);
            _settingsService.Set(SettingsKeys.BATTERY_SORT_ORDER, SortOrder);
            _settingsService.Set(SettingsKeys.BATTERY_SHOW_TEXT, ShowText);
            _settingsService.Set(SettingsKeys.BATTERY_SHOW_TIME, ShowTime);
        }
    }
}
