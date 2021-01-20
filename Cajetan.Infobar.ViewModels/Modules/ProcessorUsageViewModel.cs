using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;
using System.Collections.ObjectModel;

namespace Cajetan.Infobar.ViewModels
{
    public class ProcessorUsageViewModel : ModuleViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ISystemInfoService _systemInfoService;

        private bool _showGraph;
        private string _usage;
        private readonly ObservableCollection<int> _values;

        public ProcessorUsageViewModel(ISettingsService settings, ISystemInfoService systemInfo)
        {
            _settingsService = settings;
            _systemInfoService = systemInfo;
            _values = new ObservableCollection<int>();
        }

        public override EModuleType ModuleType => EModuleType.ProcessorUsage;

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

        public ObservableCollection<int> Values { get { return _values; } }

        public override void Update()
        {
            if (_settingsService.Contains("Module_Processor_IsEnabled"))
                IsEnabled = _settingsService.Get<bool>("Module_Processor_IsEnabled");

            if (_settingsService.Contains("Module_Processor_SortOrder"))
                SortOrder = _settingsService.Get<int>("Module_Processor_SortOrder");

            if (_settingsService.Contains("Module_Processor_ShowText"))
                ShowText = _settingsService.Get<bool>("Module_Processor_ShowText");

            if (_settingsService.Contains("Module_Processor_ShowGraph"))
                ShowGraph = _settingsService.Get<bool>("Module_Processor_ShowGraph");
        }

        public override void RefreshData()
        {
            Usage = _systemInfoService.ProcessorUsageString;
            Values.Add(_systemInfoService.ProcessorUsage);
        }
    }
}
