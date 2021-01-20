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
            if (_settingsService.Contains("Module_Network_IsEnabled"))
                IsEnabled = _settingsService.Get<bool>("Module_Network_IsEnabled");

            if (_settingsService.Contains("Module_Network_SortOrder"))
                SortOrder = _settingsService.Get<int>("Module_Network_SortOrder");

            DisplayFormats = new Dictionary<string, ENetworkDisplayFormat>
            {
                { "Auto", ENetworkDisplayFormat.Auto },
                { "Bytes (B/s)", ENetworkDisplayFormat.Bytes },
                { "Kilobytes (kB/s)", ENetworkDisplayFormat.Kilobytes },
                { "Megabytes (MB/s)", ENetworkDisplayFormat.Megabytes },
                { "Gigabytes (GB/s)", ENetworkDisplayFormat.Gigabytes }
            };

            if (_settingsService.Contains("Module_Network_DisplayFormat"))
                SelectedDisplayFormat = _settingsService.Get<ENetworkDisplayFormat>("Module_Network_DisplayFormat");
        }

        public override void Save()
        {
            _settingsService.Set("Module_Network_SortOrder", SortOrder);
            _settingsService.Set("Module_Network_DisplayFormat", SelectedDisplayFormat);
        }
    }
}
