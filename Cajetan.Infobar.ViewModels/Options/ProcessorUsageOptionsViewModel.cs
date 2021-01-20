using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;

namespace Cajetan.Infobar.ViewModels
{
    public class ProcessorUsageOptionsViewModel : ModuleOptionsViewModelBase
    {
        private readonly ISettingsService _settingsService;

        private bool _showGraph;

        public ProcessorUsageOptionsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            DisplayName = "CPU Usage";
            Description = "Shows the current CPU usage in percent.";

            ShowGraph = true;
        }

        public override EModuleType ModuleType => EModuleType.ProcessorUsage;

        public bool ShowGraph
        {
            get => _showGraph;
            set => SetProperty(ref _showGraph, value);
        }

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

        public override void Save()
        {
            _settingsService.Set("Module_Processor_IsEnabled", IsEnabled);
            _settingsService.Set("Module_Processor_SortOrder", SortOrder);
            _settingsService.Set("Module_Processor_ShowText", ShowText);
            _settingsService.Set("Module_Processor_ShowGraph", ShowGraph);
        }
    }
}
