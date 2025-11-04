namespace Cajetan.Infobar.ViewModels;

public class ProcessorUsageViewModel : ModuleViewModelBase
{
    private readonly ISettingsService _settingsService;
    private readonly ISystemMonitorService _systemMonitorService;

    private bool _showGraph;
    private string _usage;

    public ProcessorUsageViewModel(ISettingsService settings, ISystemMonitorService systemMonitorService)
        : base(settings)
    {
        _settingsService = settings;
        _systemMonitorService = systemMonitorService;
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

    public ObservableCollection<int> Values { get; } = [];

    protected override void InternalUpdate()
    {
        if (_settingsService.TryGet(SettingsKeys.PROCESSOR_IS_ENABLED, out bool isEnabled))
            IsEnabled = isEnabled;

        if (_settingsService.TryGet(SettingsKeys.PROCESSOR_SORT_ORDER, out int sortOrder))
            SortOrder = sortOrder;

        if (_settingsService.TryGet(SettingsKeys.PROCESSOR_SHOW_TEXT, out bool showText))
            ShowText = showText;

        if (_settingsService.TryGet(SettingsKeys.PROCESSOR_SHOW_GRAPH, out bool showGraph))
            ShowGraph = showGraph;
    }

    public override void RefreshData()
    {
        int cpuPercentage = Convert.ToInt32(_systemMonitorService.Processor.Percentage);

        Usage = $"{cpuPercentage} %";
        Values.Add(cpuPercentage);
    }
}
