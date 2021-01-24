using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;

namespace Cajetan.Infobar.ViewModels
{
    public class UptimeOptionsViewModel : ModuleOptionsViewModelBase
    {
        private readonly ISettingsService _settingsService;

        private bool _showDays;

        public UptimeOptionsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            DisplayName = "Uptime";
            Description = "Shows the current system uptime.";

            ShowDays = false;
        }

        public override EModuleType ModuleType => EModuleType.Uptime;

        public bool ShowDays
        {
            get => _showDays;
            set => SetProperty(ref _showDays, value);
        }

        public override void Update()
        {
            if (_settingsService.TryGet(SettingsKeys.UPTIME_IS_ENABLED, out bool isEnabled))
                IsEnabled = isEnabled;

            if (_settingsService.TryGet(SettingsKeys.UPTIME_SORT_ORDER, out int sortOrder))
                SortOrder = sortOrder;

            if (_settingsService.TryGet(SettingsKeys.UPTIME_SHOW_TEXT, out bool showText))
                ShowText = showText;

            if (_settingsService.TryGet(SettingsKeys.UPTIME_SHOW_DAYS, out bool showDays))
                ShowDays = showDays;
        }

        public override void Save()
        {
            _settingsService.Set(SettingsKeys.UPTIME_IS_ENABLED, IsEnabled);
            _settingsService.Set(SettingsKeys.UPTIME_SORT_ORDER, SortOrder);
            _settingsService.Set(SettingsKeys.UPTIME_SHOW_TEXT, ShowText);
            _settingsService.Set(SettingsKeys.UPTIME_SHOW_DAYS, ShowDays);
        }
    }
}
