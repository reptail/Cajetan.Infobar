using Cajetan.Infobar.Domain.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Cajetan.Infobar.ViewModels
{
    public abstract class ModuleViewModelBase : ObservableObject
    {
        private bool _showText;
        private bool _isEnabled;

        public abstract EModuleType ModuleType { get; }

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
        
        public abstract void Update();
        public abstract void RefreshData();

    }
}
