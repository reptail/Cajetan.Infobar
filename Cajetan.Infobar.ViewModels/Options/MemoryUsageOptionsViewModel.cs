using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;

namespace Cajetan.Infobar.ViewModels
{
    public class MemoryUsageOptionsViewModel : ModuleOptionsViewModelBase
    {
        private readonly ISettingsService _settingsService;

        private bool _showGraph;

        public MemoryUsageOptionsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            DisplayName = "Memory Usage";
            Description = "Shows the the currently used memory and the total installed.";

            ShowGraph = true;
        }

        public override EModuleType ModuleType => EModuleType.MemoryUsage;

        public bool ShowGraph
        {
            get => _showGraph;
            set => SetProperty(ref _showGraph, value);
        }

        public override void Update()
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

        public override void Save()
        {
            _settingsService.Set(SettingsKeys.MEMORY_IS_ENABLED, IsEnabled);
            _settingsService.Set(SettingsKeys.MEMORY_SORT_ORDER, SortOrder);
            _settingsService.Set(SettingsKeys.MEMORY_SHOW_TEXT, ShowText);
            _settingsService.Set(SettingsKeys.MEMORY_SHOW_GRAPH, ShowGraph);
        }
    }
}
