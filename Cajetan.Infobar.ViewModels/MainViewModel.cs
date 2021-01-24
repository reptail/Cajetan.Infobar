using Cajetan.Infobar.Domain.AppBar;
using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace Cajetan.Infobar.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly IAppBarController _appBar;
        private readonly ISettingsService _settingsService;
        private readonly IWindowService _windowService;
        private readonly ISystemMonitorService _systemMonitorService;
        private readonly OptionsViewModel _optionsViewModel;

        private readonly Timer _timer;

        private string _backgroundColor = "#000";
        private string _foregroundColor = "#FFF";
        private string _borderColor = "#000";

        private readonly ModuleViewModelBase[] _availableModules;
        private ObservableCollection<ModuleViewModelBase> _activeModules;

        public MainViewModel(IAppBarController appBar, ISettingsService settings, IWindowService windowService, ISystemMonitorService systemMonitorService,
                             OptionsViewModel optionsViewModel, ModuleViewModelBase[] availableModules)
        {
            _appBar = appBar ?? throw new ArgumentNullException(nameof(appBar));
            _settingsService = settings ?? throw new ArgumentNullException(nameof(settings));
            _windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
            _systemMonitorService = systemMonitorService ?? throw new ArgumentNullException(nameof(systemMonitorService));
            _optionsViewModel = optionsViewModel ?? throw new ArgumentNullException(nameof(optionsViewModel));
            _availableModules = availableModules ?? Array.Empty<ModuleViewModelBase>();

            ResetCommand = new RelayCommand(Reset);
            DockCommand = new RelayCommand(Dock);
            CloseCommand = new RelayCommand(Close);
            OpenSettingsCommand = new RelayCommand(OpenSettings);

            TimeSpan timerInterval = TimeSpan.FromSeconds(1);
            _timer = new Timer(timerInterval.TotalMilliseconds);
            _timer.Elapsed += Timer_Elapsed;
        }

        public ICommand ResetCommand { get; }
        public ICommand DockCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand OpenSettingsCommand { get; }

        public ObservableCollection<ModuleViewModelBase> ActiveModules
        {
            get => _activeModules;
            set => SetProperty(ref _activeModules, value);
        }

        public string BackgroundColor
        {
            get => _backgroundColor;
            set => SetProperty(ref _backgroundColor, value);
        }

        public string ForegroundColor
        {
            get => _foregroundColor;
            set => SetProperty(ref _foregroundColor, value);
        }

        public string BorderColor
        {
            get => _borderColor;
            set => SetProperty(ref _borderColor, value);
        }

        public void Initialize()
        {
            // Load settings
            if (_settingsService != null)
            {
                _settingsService.SettingsUpdated -= SettingsService_SettingsUpdated;
                _settingsService.SettingsUpdated += SettingsService_SettingsUpdated;

                foreach (ModuleViewModelBase m in _availableModules)
                    m.Update();

                LoadFromSettings();
            }

            // Initial refresh of values and modules
            UpdateDataAndRefreshModules();

            // Start update timer
            _timer.Start();
        }


        public void Reset()
        {
            _appBar.Reset();
        }

        public void Dock()
        {
            _appBar.DockBottom();
        }

        public void Close()
        {
            // Stop timer
            _timer.Stop();
            _timer.Elapsed -= Timer_Elapsed;
            _timer.Dispose();

            // Unregister AppBar
            _appBar.Undock();

            // Shutdown application
            _appBar.Shutdown();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
            => UpdateDataAndRefreshModules();

        private void UpdateDataAndRefreshModules()
        {
            // Update system info
            _systemMonitorService.Update();

            // Refresh modules
            foreach (ModuleViewModelBase m in ActiveModules)
                m.RefreshData();
        }

        private void LoadFromSettings()
        {
            if (_settingsService.TryGet(SettingsKeys.GENERAL_BACKGROUND_COLOR, out string backgroundColor))
                BackgroundColor = backgroundColor;

            if (_settingsService.TryGet(SettingsKeys.GENERAL_FOREGROUND_COLOR, out string foregroundColor))
                ForegroundColor = foregroundColor;

            if (_settingsService.TryGet(SettingsKeys.GENERAL_BORDER_COLOR, out string borderColor))
                BorderColor = borderColor;

            if (_settingsService.TryGet(SettingsKeys.GENERAL_REFRESH_INTERVAL, out int refreshInterval))
                _timer.Interval = refreshInterval;

            // Add sorted modules
            ModuleViewModelBase[] modulesToActivate = _availableModules.Where(m => m.IsEnabled).OrderBy(m => m.SortOrder).ToArray();
            ActiveModules = new ObservableCollection<ModuleViewModelBase>(modulesToActivate);
        }

        private void SettingsService_SettingsUpdated(object sender, IEnumerable<string> e)
        {
            LoadFromSettings();
        }

        private void OpenSettings()
        {
            _optionsViewModel.Update();
            _windowService.OpenDialog(_optionsViewModel, false, 600, 620);
        }

    }
}
