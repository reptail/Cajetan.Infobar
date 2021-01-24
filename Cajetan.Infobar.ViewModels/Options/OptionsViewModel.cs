using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;
using Cajetan.Infobar.Domain.ViewModels;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Cajetan.Infobar.ViewModels
{
    public class OptionsViewModel : ObservableObject, IWindowViewModel
    {
        private enum EMoveDirection { None, Up, Down }

        private readonly ISettingsService _settingsService;
        private readonly IWindowService _windowService;

        private readonly ModuleOptionsViewModelBase[] _availableModules;
        private ObservableCollection<ModuleOptionsViewModelBase> _moduleOptions;

        private string _backgroundColor;
        private string _foregroundColor;
        private string _borderColor;

        private int _updateInterval;

        private ModuleOptionsViewModelBase _selectedModuleOption;

        public OptionsViewModel(ISettingsService settingsService, IWindowService windowService, ModuleOptionsViewModelBase[] availableModules)
        {
            _settingsService = settingsService;
            _windowService = windowService;

            _availableModules = availableModules ?? Array.Empty<ModuleOptionsViewModelBase>();
            _selectedModuleOption = availableModules?.FirstOrDefault();

            SelectBackgroundColorCommand = new RelayCommand(() => SelectColor("Select Background Color", BackgroundColor, c => BackgroundColor = c));
            SelectForegroundColorCommand = new RelayCommand(() => SelectColor("Select Foreground Color", ForegroundColor, c => ForegroundColor = c));
            SelectBorderColorCommand = new RelayCommand(() => SelectColor("Select Border Color", BorderColor, c => BorderColor = c));

            MoveUpCommand = new RelayCommand<ModuleOptionsViewModelBase>(param => MoveElement(param, EMoveDirection.Up));
            MoveDownCommand = new RelayCommand<ModuleOptionsViewModelBase>(param => MoveElement(param, EMoveDirection.Down));

            SaveCommand = new RelayCommand(Save);
            ApplyCommand = new RelayCommand(Apply);
            CancelCommand = new RelayCommand(Cancel);

            BackgroundColor = "#FF3B3B3B";
            ForegroundColor = "#FFF5F5F5";
            BorderColor = "#FF5E6F7F";
            UpdateInterval = 1000;
        }

        public string DisplayName { get; } = "Cajetan Infobar - Options";

        public ICommand SelectBackgroundColorCommand { get; }
        public ICommand SelectForegroundColorCommand { get; }
        public ICommand SelectBorderColorCommand { get; }

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

        public ObservableCollection<ModuleOptionsViewModelBase> ModuleOptions
        {
            get => _moduleOptions;
            set => SetProperty(ref _moduleOptions, value);
        }

        public ModuleOptionsViewModelBase SelectedModuleOption
        {
            get => _selectedModuleOption;
            set => SetProperty(ref _selectedModuleOption, value);
        }

        public void Update()
        {
            // Update modules
            foreach (ModuleOptionsViewModelBase module in _availableModules)
                module.Update();

            ModuleOptions = new ObservableCollection<ModuleOptionsViewModelBase>(_availableModules.OrderBy(m => m.SortOrder));
            SelectedModuleOption = ModuleOptions?.FirstOrDefault();

            // Load general settings
            if (_settingsService.TryGet(SettingsKeys.GENERAL_BACKGROUND_COLOR, out string backgroundColor))
                BackgroundColor = backgroundColor;

            if (_settingsService.TryGet(SettingsKeys.GENERAL_FOREGROUND_COLOR, out string foregroundColor))
                ForegroundColor = foregroundColor;

            if (_settingsService.TryGet(SettingsKeys.GENERAL_BORDER_COLOR, out string borderColor))
                BorderColor = borderColor;

            if (_settingsService.TryGet(SettingsKeys.GENERAL_REFRESH_INTERVAL, out int refreshInterval))
                UpdateInterval = refreshInterval;
        }

        private void SelectColor(string title, string currentColor, Action<string> fieldAssignment)
        {
            string newColor = _windowService.ShowColorDialog(title, currentColor);
            fieldAssignment(newColor);
        }

        private void MoveElement(ModuleOptionsViewModelBase element, EMoveDirection direction)
        {
            if (element is null) return;

            int currentIndex = ModuleOptions.IndexOf(element);
            if (currentIndex == -1) return;

            bool movedAny = false;
            if (direction == EMoveDirection.Up && currentIndex != 0)
            {
                ModuleOptions.Move(currentIndex, currentIndex - 1);
                movedAny = true;
            }
            else if (direction == EMoveDirection.Down && currentIndex != (ModuleOptions.Count - 1))
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
            _settingsService.Set(SettingsKeys.GENERAL_BACKGROUND_COLOR, BackgroundColor);
            _settingsService.Set(SettingsKeys.GENERAL_FOREGROUND_COLOR, ForegroundColor);
            _settingsService.Set(SettingsKeys.GENERAL_BORDER_COLOR, BorderColor);
            _settingsService.Set(SettingsKeys.GENERAL_REFRESH_INTERVAL, UpdateInterval);

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
