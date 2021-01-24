using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace Cajetan.Infobar.ViewModels
{
    public abstract class ModuleViewModelBase : ObservableObject, IDisposable
    {
        private readonly ISettingsService _settingsService;

        private bool _isDisposed;
        private bool _showText;
        private bool _isEnabled;
        private string _backgroundColor;
        private string _foregroundColor;
        private string _borderColor;

        public ModuleViewModelBase(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _settingsService.SettingsUpdated += SettingsService_SettingsUpdated;
        }

        public abstract EModuleType ModuleType { get; }

        public string BackgroundColor
        {
            get => _backgroundColor;
            private set => SetProperty(ref _backgroundColor, value);
        }
        public string ForegroundColor
        {
            get => _foregroundColor;
            private set => SetProperty(ref _foregroundColor, value);
        }
        public string BorderColor
        {
            get => _borderColor;
            private set => SetProperty(ref _borderColor, value);
        }

        public bool ShowText
        {
            get => _showText;
            set => SetProperty(ref _showText, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public int SortOrder { get; set; }

        public void Update()
        {
            if (_settingsService.TryGet(SettingsKeys.GENERAL_BACKGROUND_COLOR, out string backgroundColor))
                BackgroundColor = backgroundColor;

            if (_settingsService.TryGet(SettingsKeys.GENERAL_FOREGROUND_COLOR, out string foregroundColor))
                ForegroundColor = foregroundColor;

            if (_settingsService.TryGet(SettingsKeys.GENERAL_BORDER_COLOR, out string borderColor))
                BorderColor = borderColor;

            InternalUpdate();
        }
        protected abstract void InternalUpdate();
        public abstract void RefreshData();

        private void SettingsService_SettingsUpdated(object sender, IEnumerable<string> _)
            => Update();

        protected virtual void Dispose(bool disposing)
        {
        }

        private void InternalDispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _settingsService.SettingsUpdated -= SettingsService_SettingsUpdated;
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            InternalDispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public override string ToString()
        {
            return $"[{GetType().Name}] [Enabled: {IsEnabled}] [Sort: {SortOrder}] [Text: {ShowText}]";
        }
    }
}
