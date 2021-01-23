using Autofac;
using Cajetan.Infobar.Domain.AppBar;
using Cajetan.Infobar.Domain.Services;
using Cajetan.Infobar.Services;
using Cajetan.Infobar.ViewModels;

namespace Cajetan.Infobar.Config
{
    public static class AutofacConfig
    {
        private static IContainer _container;

        public static void Initialize(MainWindow mainWindow)
        {
            ContainerBuilder cb = new ContainerBuilder();
            
            RegisterMisc(cb, mainWindow);
            RegisterViewModels(cb);
            RegisterServices(cb);
            
            _container = cb.Build();
        }

        public static void Dispose()
        {
            _container?.Dispose();
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        private static void RegisterMisc(ContainerBuilder cb, MainWindow mainWindow)
        {
            cb.RegisterInstance(mainWindow)
                .As<IAppBarController>()
                .ExternallyOwned();
        }

        private static void RegisterViewModels(ContainerBuilder cb)
        {
            cb.RegisterType<MainViewModel>()
                .SingleInstance();

            cb.RegisterType<BatteryStatusViewModel>()
                .As<ModuleViewModelBase>()
                .SingleInstance();
            cb.RegisterType<MemoryUsageViewModel>()
                .As<ModuleViewModelBase>()
                .SingleInstance();
            cb.RegisterType<NetworkUsageViewModel>()
                .As<ModuleViewModelBase>()
                .SingleInstance();
            cb.RegisterType<ProcessorUsageViewModel>()
                .As<ModuleViewModelBase>()
                .SingleInstance();
            cb.RegisterType<UptimeViewModel>()
                .As<ModuleViewModelBase>()
                .SingleInstance();

            cb.RegisterType<OptionsViewModel>();
            cb.RegisterType<UptimeOptionsViewModel>()
                .As<ModuleOptionsViewModelBase>();
            cb.RegisterType<BatteryStatusOptionsViewModel>()
                .As<ModuleOptionsViewModelBase>();
            cb.RegisterType<ProcessorUsageOptionsViewModel>()
                .As<ModuleOptionsViewModelBase>();
            cb.RegisterType<MemoryUsageOptionsViewModel>()
                .As<ModuleOptionsViewModelBase>();
            cb.RegisterType<NetworkUsageOptionsViewModel>()
                .As<ModuleOptionsViewModelBase>();
        }

        private static void RegisterServices(ContainerBuilder cb)
        {
            cb.RegisterType<FileSystemService>()
                .As<IFileSystemService>();
            cb.RegisterType<CfgSettingsService>()
                .As<ISettingsService>()
                .SingleInstance();
            cb.RegisterType<WindowService>()
                .As<IWindowService>()
                .SingleInstance();
            cb.RegisterType<SystemMonitorService>()
                .As<ISystemMonitorService>()
                .SingleInstance();
        }

    }
}
