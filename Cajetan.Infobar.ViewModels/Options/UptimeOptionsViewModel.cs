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
            if (_settingsService.Contains("Module_Uptime_IsEnabled"))
                IsEnabled = _settingsService.Get<bool>("Module_Uptime_IsEnabled");

            if (_settingsService.Contains("Module_Uptime_SortOrder"))
                SortOrder = _settingsService.Get<int>("Module_Uptime_SortOrder");

            if (_settingsService.Contains("Module_Uptime_ShowText"))
                ShowText = _settingsService.Get<bool>("Module_Uptime_ShowText");

            if (_settingsService.Contains("Module_Uptime_ShowDays"))
                ShowDays = _settingsService.Get<bool>("Module_Uptime_ShowDays");
        }

        public override void Save()
        {
            _settingsService.Set("Module_Uptime_IsEnabled", IsEnabled);
            _settingsService.Set("Module_Uptime_SortOrder", SortOrder);
            _settingsService.Set("Module_Uptime_ShowText", ShowText);
            _settingsService.Set("Module_Uptime_ShowDays", ShowDays);
        }
    }
}
