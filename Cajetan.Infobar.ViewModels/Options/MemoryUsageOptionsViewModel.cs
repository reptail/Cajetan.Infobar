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
            if (_settingsService.Contains("Module_Memory_IsEnabled"))
                IsEnabled = _settingsService.Get<bool>("Module_Memory_IsEnabled");

            if (_settingsService.Contains("Module_Memory_SortOrder"))
                SortOrder = _settingsService.Get<int>("Module_Memory_SortOrder");

            if (_settingsService.Contains("Module_Memory_ShowText"))
                ShowText = _settingsService.Get<bool>("Module_Memory_ShowText");

            if (_settingsService.Contains("Module_Memory_ShowGraph"))
                ShowGraph = _settingsService.Get<bool>("Module_Memory_ShowGraph");
        }

        public override void Save()
        {
            _settingsService.Set("Module_Memory_IsEnabled", IsEnabled);
            _settingsService.Set("Module_Memory_SortOrder", SortOrder);
            _settingsService.Set("Module_Memory_ShowText", ShowText);
            _settingsService.Set("Module_Memory_ShowGraph", ShowGraph);
        }
    }
}
