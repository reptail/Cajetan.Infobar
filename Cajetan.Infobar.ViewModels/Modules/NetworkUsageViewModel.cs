using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;

namespace Cajetan.Infobar.ViewModels
{
    public class NetworkUsageViewModel : ModuleViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ISystemInfoService _systemInfoService;

        private string _upload;
        private string _download;

        public NetworkUsageViewModel(ISettingsService settings, ISystemInfoService systemInfo)
        {
            _settingsService = settings;
            _systemInfoService = systemInfo;
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
            if (_settingsService.Contains("Module_Network_IsEnabled"))
                IsEnabled = _settingsService.Get<bool>("Module_Network_IsEnabled");

            if (_settingsService.Contains("Module_Network_SortOrder"))
                SortOrder = _settingsService.Get<int>("Module_Network_SortOrder");

            //if (_settingsService.Contains("Module_Network_DisplayFormat"))
            //    _displayFormat = _settingsService.Get<NetworkDisplayFormat>("Module_Network_DisplayFormat");
        }

        public override void RefreshData()
        {
            Download = _systemInfoService.NetworkDownloadString;
            Upload = _systemInfoService.NetworkUploadString;
        }

    }
}
