using Cajetan.Infobar.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace Cajetan.Infobar.Design
{
    public class DesignViewModelLocator
    {
        private static readonly Random _random = new Random();

        public MainViewModel MainViewModel
        {
            get
            {
                MainViewModel vm = new MainViewModel(null, null, null, null, null, Modules)
                {
                    BackgroundColor = "#FF3B3B3B",
                    ForegroundColor = "#FFF5F5F5",
                    BorderColor = "#FF5E6F7F",

                    ActiveModules = new ObservableCollection<ModuleViewModelBase>()
                    {
                        UptimeViewModel,
                        ProcessorUsageViewModel,
                        MemoryUsageViewModel,
                        //NetworkUsageViewModel
                    }
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
                BatteryStatusViewModel vm = new BatteryStatusViewModel(null, null)
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
                UptimeViewModel vm = new UptimeViewModel(null, null)
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
                ProcessorUsageViewModel vm = new ProcessorUsageViewModel(null, null)
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
                MemoryUsageViewModel vm = new MemoryUsageViewModel(null, null)
                {
                    Usage = "3712MB / 12323MB"
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
                NetworkUsageViewModel vm = new NetworkUsageViewModel(null, null)
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
                OptionsViewModel vm = new OptionsViewModel(null, null, OptionsModules);

                vm.SelectedModuleOption = vm.ModuleOptions[2];

                return vm;
            }
        }

        public ModuleOptionsViewModelBase[] OptionsModules
        {
            get => new ModuleOptionsViewModelBase[]
            {
                BatteryStatusOptionsViewModel,
                MemoryUsageOptionsViewModel,
                //NetworkUsageOptionsViewModel,
                ProcessorUsageOptionsViewModel,
                UptimeOptionsViewModel
            };
        }

        public BatteryStatusOptionsViewModel BatteryStatusOptionsViewModel
        {
            get
            {
                BatteryStatusOptionsViewModel vm = new BatteryStatusOptionsViewModel(null);

                return vm;
            }
        }

        public MemoryUsageOptionsViewModel MemoryUsageOptionsViewModel
        {
            get
            {
                MemoryUsageOptionsViewModel vm = new MemoryUsageOptionsViewModel(null);

                return vm;
            }
        }

        //public NetworkUsageOptionsViewModel NetworkUsageOptionsViewModel
        //{
        //    get
        //    {
        //        var vm = new NetworkUsageOptionsViewModel(null)
        //        {
        //            DisplayFormats = new Dictionary<string, NetworkDisplayFormat>()
        //        };
        //        vm.DisplayFormats.Add("Auto", NetworkDisplayFormat.Auto);
        //        vm.DisplayFormats.Add("Kilobytes", NetworkDisplayFormat.Kilobytes);
        //        vm.SelectedDisplayFormat = NetworkDisplayFormat.Kilobytes;

        //        return vm;
        //    }
        //}

        public ProcessorUsageOptionsViewModel ProcessorUsageOptionsViewModel
        {
            get
            {
                ProcessorUsageOptionsViewModel vm = new ProcessorUsageOptionsViewModel(null);

                return vm;
            }
        }

        public UptimeOptionsViewModel UptimeOptionsViewModel
        {
            get
            {
                UptimeOptionsViewModel vm = new UptimeOptionsViewModel(null);

                return vm;
            }
        }

    }
}
