using Cajetan.Infobar.Domain.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Cajetan.Infobar.ViewModels
{
    public class OptionsViewModel : ObservableObject
    {
        private enum MoveDirection { None, Up, Down }

        private readonly ISettingsService _settingsService;
        private readonly IWindowService _windowService;

        private readonly ObservableCollection<ModuleOptionsViewModelBase> _moduleOptions;

        private string _backgroundColor;
        private string _foregroundColor;
        private string _borderColor;

        private int _updateInterval;

        private ModuleOptionsViewModelBase _selectedModuleOption;

        public OptionsViewModel(ISettingsService settingsService, IWindowService windowService, ModuleOptionsViewModelBase[] availableModules)
        {
            //DisplayName = "Cajetan Infobar - Options";

            _settingsService = settingsService;
            _windowService = windowService;

            _moduleOptions = availableModules is null
                ? new ObservableCollection<ModuleOptionsViewModelBase>()
                : new ObservableCollection<ModuleOptionsViewModelBase>(availableModules.OrderBy(m => m.SortOrder));
            _selectedModuleOption = _moduleOptions.FirstOrDefault();

            MoveUpCommand = new RelayCommand<ModuleOptionsViewModelBase>(param => MoveElement(param, MoveDirection.Up));
            MoveDownCommand = new RelayCommand<ModuleOptionsViewModelBase>(param => MoveElement(param, MoveDirection.Down));

            SaveCommand = new RelayCommand(Save);
            ApplyCommand = new RelayCommand(Apply);
            CancelCommand = new RelayCommand(Cancel);

            BackgroundColor = "#FF3B3B3B";
            ForegroundColor = "#FFF5F5F5";
            BorderColor = "#FF5E6F7F";
            UpdateInterval = 500;
        }

        public ICommand MoveUpCommand { get; }
        public ICommand MoveDownCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ApplyCommand { get; }
        public ICommand CancelCommand { get; }

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

        public int UpdateInterval
        {
            get => _updateInterval;
            set => SetProperty(ref _updateInterval, value);
        }

        public ObservableCollection<ModuleOptionsViewModelBase> ModuleOptions => _moduleOptions;

        public ModuleOptionsViewModelBase SelectedModuleOption
        {
            get => _selectedModuleOption;
            set => SetProperty(ref _selectedModuleOption, value);
        }

        public void Update()
        {
            //modules.Add(_viewModelLocator.BatteryStatusOptionsViewModel);
            //modules.Add(_viewModelLocator.MemoryUsageOptionsViewModel);
            //modules.Add(_viewModelLocator.NetworkUsageOptionsViewModel);
            //modules.Add(_viewModelLocator.ProcessorUsageOptionsViewModel);
            //modules.Add(_viewModelLocator.UptimeOptionsViewModel);

            // Update modules
            foreach (ModuleOptionsViewModelBase module in ModuleOptions)
                module.Update();

            // Load general settings
            if (_settingsService.Contains("General_BackgroundColor"))
                BackgroundColor = _settingsService.Get<string>("General_BackgroundColor");

            if (_settingsService.Contains("General_ForegroundColor"))
                ForegroundColor = _settingsService.Get<string>("General_ForegroundColor");

            if (_settingsService.Contains("General_BorderColor"))
                BorderColor = _settingsService.Get<string>("General_BorderColor");

            if (_settingsService.Contains("General_RefreshInterval_Milliseconds"))
                UpdateInterval = _settingsService.Get<int>("General_RefreshInterval_Milliseconds");
        }

        private void MoveElement(ModuleOptionsViewModelBase element, MoveDirection direction)
        {
            if (element is null) return;

            int currentIndex = ModuleOptions.IndexOf(element);
            if (currentIndex == -1) return;

            bool movedAny = false;
            if (direction == MoveDirection.Up && currentIndex != 0)
            {
                ModuleOptions.Move(currentIndex, currentIndex - 1);
                movedAny = true;
            }
            else if (direction == MoveDirection.Down && currentIndex != (ModuleOptions.Count - 1))
            {
                ModuleOptions.Move(currentIndex, currentIndex + 1);
                movedAny = true;
            }

            if (movedAny)
            {
                for (int i = 0; i < ModuleOptions.Count; i++)
                {
                    ModuleOptions[i].SortOrder = i + 1;
                }
            }
        }

        private void Apply()
        {
            // Save any module option changes to settings service
            foreach (ModuleOptionsViewModelBase module in ModuleOptions)
                module.Save();

            // Save general settings
            _settingsService.Set("General_BackgroundColor", BackgroundColor);
            _settingsService.Set("General_ForegroundColor", ForegroundColor);
            _settingsService.Set("General_BorderColor", BorderColor);
            _settingsService.Set("General_RefreshInterval_Milliseconds", UpdateInterval);

            // Save changes
            _settingsService.SaveChanges();
        }

        private void Save()
        {
            Apply();

            // Close window
            _windowService.CloseWindow(this, true);
        }

        private void Cancel()
        {
            // Cancel 
            _windowService.CloseWindow(this, false);
        }
    }
}
