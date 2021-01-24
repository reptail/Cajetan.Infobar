using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;
using System.Collections.Generic;

namespace Cajetan.Infobar.ViewModels
{
    public class NetworkUsageOptionsViewModel : ModuleOptionsViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private IDictionary<string, ENetworkDisplayFormat> _displayFormats;
        private ENetworkDisplayFormat _selectedDisplayFormat;

        public NetworkUsageOptionsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            DisplayName = "Network Usage";
            Description = "Shows the current download and upload.";

            IsEnabled = true;
            SelectedDisplayFormat = ENetworkDisplayFormat.Auto;
        }

        public override EModuleType ModuleType => EModuleType.NetworkUsage;

        public IDictionary<string, ENetworkDisplayFormat> DisplayFormats
        {
            get => _displayFormats;
            set => SetProperty(ref _displayFormats, value);
        }

        public ENetworkDisplayFormat SelectedDisplayFormat
        {
            get => _selectedDisplayFormat;
            set => SetProperty(ref _selectedDisplayFormat, value);
        }

        public override void Update()
        {
            if (_settingsService.TryGet(SettingsKeys.NETWORK_IS_ENABLED, out bool isEnabled))
                IsEnabled = isEnabled;

            if (_settingsService.TryGet(SettingsKeys.NETWORK_SORT_ORDER, out int sortOrder))
                SortOrder = sortOrder;

            DisplayFormats = new Dictionary<string, ENetworkDisplayFormat>
            {
                { "Auto", ENetworkDisplayFormat.Auto },
                { "Bytes (B/s)", ENetworkDisplayFormat.Bytes },
                { "Kilobytes (kB/s)", ENetworkDisplayFormat.Kilobytes },
                { "Megabytes (MB/s)", ENetworkDisplayFormat.Megabytes },
                { "Gigabytes (GB/s)", ENetworkDisplayFormat.Gigabytes }
            };

            if (_settingsService.TryGet(SettingsKeys.NETWORK_DISPLAY_FORMAT, out ENetworkDisplayFormat displayFormat))
                SelectedDisplayFormat = displayFormat;
        }

        public override void Save()
        {
            _settingsService.Set(SettingsKeys.NETWORK_IS_ENABLED, IsEnabled);
            _settingsService.Set(SettingsKeys.NETWORK_SORT_ORDER, SortOrder);
            _settingsService.Set(SettingsKeys.NETWORK_DISPLAY_FORMAT, SelectedDisplayFormat);
        }
    }
}
