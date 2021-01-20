using Cajetan.Infobar.Domain.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Cajetan.Infobar.ViewModels
{
    public abstract class ModuleOptionsViewModelBase : ObservableObject
    {
        private string _description;
        private bool _isEnabled;
        private bool _showText;

        public abstract EModuleType ModuleType { get; }
        public string DisplayName { get; protected set; }

        public string Description
        {
            get =>  _description;
            set => SetProperty(ref _description, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public bool ShowText
        {
            get => _showText;
            set => SetProperty(ref _showText, value);
        }

        public int SortOrder { get; set; }

        public abstract void Update();
        public abstract void Save();
    }
}
