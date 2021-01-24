using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;
using System;

namespace Cajetan.Infobar.ViewModels
{
    public class UptimeViewModel : ModuleViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ISystemMonitorService _systemMonitorService;

        private bool _showDays;
        private string _uptime;

        public UptimeViewModel(ISettingsService settings, ISystemMonitorService systemMonitorService)
            : base(settings)
        {
            _settingsService = settings;
            _systemMonitorService = systemMonitorService;

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

        protected override void InternalUpdate()
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

        public override void RefreshData()
        {
            TimeSpan uptime = _systemMonitorService.Info.Uptime;

            string strDays = ShowDays || uptime.Days > 0
                ? $"{uptime.Days:0}d "
                : "";
            string strTime = $"{uptime.Hours:00}:{uptime.Minutes:00}:{uptime.Seconds:00}";
            Uptime = $"{strDays}{strTime}";
        }
    }
}
