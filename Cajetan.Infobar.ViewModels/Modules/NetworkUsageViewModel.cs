using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;

namespace Cajetan.Infobar.ViewModels
{
    public class NetworkUsageViewModel : ModuleViewModelBase
    {
        private const string IS_ENABLED_KEY = "Module_Network_IsEnabled";
        private const string SORT_ORDER_KEY = "Module_Network_SortOrder";
        private const string DISPLAY_FORMAT_KEY = "Module_Network_DisplayFormat";

        private readonly ISettingsService _settingsService;
        private readonly ISystemMonitorService _systemMonitorService;

        private string _upload;
        private string _download;
        private ENetworkDisplayFormat _displayFormat;

        public NetworkUsageViewModel(ISettingsService settings, ISystemMonitorService systemMonitorService)
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

        public override void Update()
        {
            if (_settingsService.Contains(IS_ENABLED_KEY))
                IsEnabled = _settingsService.Get<bool>(IS_ENABLED_KEY);

            if (_settingsService.Contains(SORT_ORDER_KEY))
                SortOrder = _settingsService.Get<int>(SORT_ORDER_KEY);

            if (_settingsService.Contains(DISPLAY_FORMAT_KEY))
                _displayFormat = _settingsService.Get<ENetworkDisplayFormat>(DISPLAY_FORMAT_KEY);
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
