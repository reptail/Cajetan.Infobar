using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;

namespace Cajetan.Infobar.ViewModels
{
    public class NetworkUsageViewModel : ModuleViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ISystemMonitorService _systemMonitorService;

        private string _upload;
        private string _download;
        private ENetworkDisplayFormat _displayFormat;

        public NetworkUsageViewModel(ISettingsService settings, ISystemMonitorService systemMonitorService)
            : base(settings)
        {
            _settingsService = settings;
            _systemMonitorService = systemMonitorService;
        }

        public override EModuleType ModuleType => EModuleType.NetworkUsage;

        public string Upload
        {
            get => _upload;
            set => SetProperty(ref _upload, value);
        }

        public string Download
        {
            get => _download;
            set => SetProperty(ref _download, value);
        }

        protected override void InternalUpdate()
        {
            if (_settingsService.TryGet(SettingsKeys.NETWORK_IS_ENABLED, out bool isEnabled))
                IsEnabled = isEnabled;

            if (_settingsService.TryGet(SettingsKeys.NETWORK_SORT_ORDER, out int sortOrder))
                SortOrder = sortOrder;

            if (_settingsService.TryGet(SettingsKeys.NETWORK_DISPLAY_FORMAT, out ENetworkDisplayFormat displayFormat))
                _displayFormat = displayFormat;
        }

        public override void RefreshData()
        {
            INetworkInfo info = _systemMonitorService.Network;

            (double downRate, string downUnit) = info.GetDownloadRate(_displayFormat);
            (double upRate, string upUnit) = info.GetUploadRate(_displayFormat);

            Download = $"{downRate:0.0}{downUnit}";
            Upload = $"{upRate:0.0}{upUnit}";
        }
    }
}
