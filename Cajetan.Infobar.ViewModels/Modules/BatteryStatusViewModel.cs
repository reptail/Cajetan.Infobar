using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;
using System;

namespace Cajetan.Infobar.ViewModels
{
    public class BatteryStatusViewModel : ModuleViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ISystemMonitorService _systemMonitorService;

        private bool _showTime;
        private string _status;

        public BatteryStatusViewModel(ISettingsService settings, ISystemMonitorService systemMonitorService)
            : base(settings)
        {
            _settingsService = settings;
            _systemMonitorService = systemMonitorService;

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

        protected override void InternalUpdate()
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

        public override void RefreshData()
        {
            IBatteryInfo info = _systemMonitorService.Battery;

            string percentage = $"{info.Percentage,3:##0}%";

            switch (info.State)
            {
                case EBatteryChargeState.NoBattery:
                    Status = "No Battery";
                    break;

                case EBatteryChargeState.FullyCharged:
                    Status = percentage;
                    break;

                case EBatteryChargeState.Charging:
                    Status = $"{percentage} (Charging)";
                    break;

                case EBatteryChargeState.Discharging:
                    string text = ShowTime
                        ? GenerateTimeRemaining(info.TimeToDepleted)
                        : "Discharging";
                    Status = $"{percentage} ({text})";
                    break;

                default:
                    Status = "Unknown";
                    break;
            }
        }

        private static string GenerateTimeRemaining(TimeSpan timeRemaining)
        {
            if (timeRemaining > TimeSpan.FromDays(5))
                return "Calculating";

            string time = $"{timeRemaining.Hours:00}h {timeRemaining.Minutes:00}m";

            if (timeRemaining > TimeSpan.FromDays(1))
                time = $"{timeRemaining.Days:0}d {time}";

            return time;
        }
    }
}
