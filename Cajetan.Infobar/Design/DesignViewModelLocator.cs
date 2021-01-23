using Cajetan.Infobar.Domain.AppBar;
using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;
using Cajetan.Infobar.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cajetan.Infobar.Design
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
    public partial class DesignViewModelLocator
    {
        private static readonly Random _random = new Random();

        private static IFileSystemService FileSystemService { get; } = new DesignFileSystemService();
        private static ISettingsService SettingsService { get; } = new DesignSettingsService();
        private static ISystemMonitorService SystemMonitorService { get; } = new DesignSystemMonitorService();
        private static IWindowService WindowService { get; } = new DesignWindowService();
        private static IAppBarController AppBarController { get; } = new DesignAppBarController();

        public MainViewModel MainViewModel
        {
            get
            {
                MainViewModel vm = new MainViewModel(AppBarController, SettingsService, WindowService, SystemMonitorService, OptionsViewModel, Modules)
                {
                    BackgroundColor = "#FF3B3B3B",
                    ForegroundColor = "#FFF5F5F5",
                    BorderColor = "#FF5E6F7F",

                    ActiveModules = new ObservableCollection<ModuleViewModelBase>(Modules),
                };

                return vm;
            }
        }

        public ModuleViewModelBase[] Modules => new ModuleViewModelBase[]
        {
            UptimeViewModel,
            BatteryStatusViewModel,
            ProcessorUsageViewModel,
            MemoryUsageViewModel,
            NetworkUsageViewModel,
        };

        public BatteryStatusViewModel BatteryStatusViewModel
        {
            get
            {
                BatteryStatusViewModel vm = new BatteryStatusViewModel(SettingsService, SystemMonitorService)
                {
                    Status = "56% (1h 40m)"
                };
                return vm;
            }
        }

        public UptimeViewModel UptimeViewModel
        {
            get
            {
                UptimeViewModel vm = new UptimeViewModel(SettingsService, SystemMonitorService)
                {
                    Uptime = "0d 01:18:15"
                };

                return vm;
            }
        }

        public ProcessorUsageViewModel ProcessorUsageViewModel
        {
            get
            {
                ProcessorUsageViewModel vm = new ProcessorUsageViewModel(SettingsService, SystemMonitorService)
                {
                    Usage = "7 %"
                };

                for (int i = 0; i < 100; i++)
                    vm.Values.Add(50 + _random.Next(-48, 5));

                return vm;
            }
        }

        public MemoryUsageViewModel MemoryUsageViewModel
        {
            get
            {
                MemoryUsageViewModel vm = new MemoryUsageViewModel(SettingsService, SystemMonitorService)
                {
                    Usage = "13712MB / 32323MB"
                };

                for (int i = 0; i < 100; i++)
                    vm.Values.Add(50 + _random.Next(-48, 5));

                return vm;
            }
        }

        public NetworkUsageViewModel NetworkUsageViewModel
        {
            get
            {
                NetworkUsageViewModel vm = new NetworkUsageViewModel(SettingsService, SystemMonitorService)
                {
                    Download = "81.2M",
                    Upload = "657.3K"
                };

                return vm;
            }
        }


        public OptionsViewModel OptionsViewModel
        {
            get
            {
                ModuleOptionsViewModelBase[] modules = OptionsModules;
                OptionsViewModel vm = new OptionsViewModel(SettingsService, WindowService, modules)
                {
                    ModuleOptions = new ObservableCollection<ModuleOptionsViewModelBase>(modules),
                    SelectedModuleOption = modules[2]
                };

                return vm;
            }
        }

        public ModuleOptionsViewModelBase[] OptionsModules
        {
            get => new ModuleOptionsViewModelBase[]
            {
                UptimeOptionsViewModel,
                BatteryStatusOptionsViewModel,
                MemoryUsageOptionsViewModel,
                NetworkUsageOptionsViewModel,
                ProcessorUsageOptionsViewModel,
            };
        }

        public BatteryStatusOptionsViewModel BatteryStatusOptionsViewModel
        {
            get
            {
                BatteryStatusOptionsViewModel vm = new BatteryStatusOptionsViewModel(SettingsService);

                return vm;
            }
        }

        public MemoryUsageOptionsViewModel MemoryUsageOptionsViewModel
        {
            get
            {
                MemoryUsageOptionsViewModel vm = new MemoryUsageOptionsViewModel(SettingsService);

                return vm;
            }
        }

        public NetworkUsageOptionsViewModel NetworkUsageOptionsViewModel
        {
            get
            {
                NetworkUsageOptionsViewModel vm = new NetworkUsageOptionsViewModel(SettingsService)
                {
                    DisplayFormats = new Dictionary<string, ENetworkDisplayFormat>
                    {
                        { "Auto", ENetworkDisplayFormat.Auto },
                        { "Kilobytes", ENetworkDisplayFormat.Kilobytes }
                    },
                    SelectedDisplayFormat = ENetworkDisplayFormat.Kilobytes
                };
                return vm;
            }
        }

        public ProcessorUsageOptionsViewModel ProcessorUsageOptionsViewModel
        {
            get
            {
                ProcessorUsageOptionsViewModel vm = new ProcessorUsageOptionsViewModel(SettingsService);

                return vm;
            }
        }

        public UptimeOptionsViewModel UptimeOptionsViewModel
        {
            get
            {
                UptimeOptionsViewModel vm = new UptimeOptionsViewModel(SettingsService);

                return vm;
            }
        }

    }
}
