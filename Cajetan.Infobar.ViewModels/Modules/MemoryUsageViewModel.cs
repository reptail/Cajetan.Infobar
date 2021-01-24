using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;
using System;
using System.Collections.ObjectModel;

namespace Cajetan.Infobar.ViewModels
{
    public class MemoryUsageViewModel : ModuleViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ISystemMonitorService _systemMonitorService;

        private bool _showGraph;
        private string _usage;
        private readonly ObservableCollection<int> _values = new ObservableCollection<int>();

        public MemoryUsageViewModel(ISettingsService settings, ISystemMonitorService systemMonitorService)
            : base(settings)
        {
            _settingsService = settings;
            _systemMonitorService = systemMonitorService;

            ShowGraph = true;
        }

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


        protected override void InternalUpdate()
        {
            if (_settingsService.TryGet(SettingsKeys.MEMORY_IS_ENABLED, out bool isEnabled))
                IsEnabled = isEnabled;

            if (_settingsService.TryGet(SettingsKeys.MEMORY_SORT_ORDER, out int sortOrder))
                SortOrder = sortOrder;

            if (_settingsService.TryGet(SettingsKeys.MEMORY_SHOW_TEXT, out bool showText))
                ShowText = showText;

            if (_settingsService.TryGet(SettingsKeys.MEMORY_SHOW_GRAPH, out bool showGraph))
                ShowGraph = showGraph;
        }

        public override void RefreshData()
        {
            IMemoryInfo info = _systemMonitorService.Memory;

            string memUsed = $"{info.Used:0} {info.Unit}";
            string memTotal = $"{info.Total:0} {info.Unit}";
            int memPercentage = Convert.ToInt32(info.Percentage);

            Usage = $"{memUsed} / {memTotal}";
            Values.Add(memPercentage);
        }

    }
}
