using Cajetan.Infobar.Domain.AppBar;
using Cajetan.Infobar.Domain.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows.Input;

namespace Cajetan.Infobar.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly IAppBarController _appBar;
        private readonly ISettingsService _settingsService;
        private readonly IWindowService _windowService;
        private readonly ISystemMonitorService _systemInfoService;
        private readonly OptionsViewModel _optionsViewModel;

        private readonly Timer _timer;

        private string _backgroundColor = "#000";
        private string _foregroundColor = "#FFF";
        private string _borderColor = "#000";

        private readonly ModuleViewModelBase[] _availableModules;
        private ObservableCollection<ModuleViewModelBase> _activeModules;


        public MainViewModel(IAppBarController appBar, ISettingsService settings, IWindowService windowService, ISystemMonitorService systemInfoService,
                             OptionsViewModel optionsViewModel, ModuleViewModelBase[] availableModules)
        {
            _appBar = appBar ?? throw new ArgumentNullException(nameof(appBar));
            _settingsService = settings ?? throw new ArgumentNullException(nameof(settings));
            _windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
            _systemInfoService = systemInfoService ?? throw new ArgumentNullException(nameof(systemInfoService));
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

                LoadFromSettings();
            }

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
        {
            // Update system info
            _systemInfoService.Update();

            // Refresh modules
            foreach (ModuleViewModelBase m in ActiveModules)
                m.RefreshData();
        }

        private void LoadFromSettings()
        {
            if (_settingsService.Contains("General_BackgroundColor"))
                BackgroundColor = _settingsService.Get<string>("General_BackgroundColor");

            if (_settingsService.Contains("General_ForegroundColor"))
                ForegroundColor = _settingsService.Get<string>("General_ForegroundColor");

            if (_settingsService.Contains("General_BorderColor"))
                BorderColor = _settingsService.Get<string>("General_BorderColor");

            if (_settingsService.Contains("General_RefreshInterval_Milliseconds"))
                _timer.Interval = _settingsService.Get<int>("General_RefreshInterval_Milliseconds");

            List<ModuleViewModelBase> modulesToActivate = new List<ModuleViewModelBase>();

            foreach (ModuleViewModelBase item in _availableModules)
            {
                // Load module settings
                item.Update();

                // Add to active list if enabled
                if (item.IsEnabled)
                    modulesToActivate.Add(item);
            }

            // Add sorted modules
            ActiveModules = new ObservableCollection<ModuleViewModelBase>(modulesToActivate.OrderBy(m => m.SortOrder));
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
