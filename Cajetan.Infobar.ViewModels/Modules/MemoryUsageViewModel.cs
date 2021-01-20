using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;
using System.Collections.ObjectModel;

namespace Cajetan.Infobar.ViewModels
{
    public class MemoryUsageViewModel : ModuleViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ISystemInfoService _systemInfoService;

        private bool _showGraph;
        private string _usage;
        private readonly ObservableCollection<int> _values = new ObservableCollection<int>();

        public override EModuleType ModuleType => EModuleType.MemoryUsage;

        public ObservableCollection<int> Values => _values;

        public bool ShowGraph
        {
            get => _showGraph;
            set => SetProperty(ref _showGraph, value);
        }

        public string Usage
        {
            get => _usage;
            set => SetProperty(ref _usage, value);
        }

        public MemoryUsageViewModel(ISettingsService settings, ISystemInfoService systemInfo)
        {
            _settingsService = settings;
            _systemInfoService = systemInfo;

            ShowGraph = true;
        }

        public override void Update()
        {
            if (_settingsService.Contains("Module_Memory_IsEnabled"))
                IsEnabled = _settingsService.Get<bool>("Module_Memory_IsEnabled");

            if (_settingsService.Contains("Module_Memory_SortOrder"))
                SortOrder = _settingsService.Get<int>("Module_Memory_SortOrder");

            if (_settingsService.Contains("Module_Memory_ShowText"))
                ShowText = _settingsService.Get<bool>("Module_Memory_ShowText");

            if (_settingsService.Contains("Module_Memory_ShowGraph"))
                ShowGraph = _settingsService.Get<bool>("Module_Memory_ShowGraph");
        }

        public override void RefreshData()
        {
            Usage = _systemInfoService.MemoryUsageString;
            Values.Add(_systemInfoService.MemoryUsedPercentage);
        }

    }
}
