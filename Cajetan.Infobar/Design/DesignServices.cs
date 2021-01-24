using Cajetan.Infobar.Domain.AppBar;
using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;
using Cajetan.Infobar.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cajetan.Infobar.Design
{
    public partial class DesignViewModelLocator
    {
        private class DesignFileSystemService : IFileSystemService
        {
            public void Copy(string originalPath, string targetPath) { }

            public string GetAppDataPath() => string.Empty;

            public string GetAssemblyPath() => string.Empty;

            public string PathCombine(params string[] segments) => string.Empty;

            public string PathCombineWithAppData(string segment) => string.Empty;

            public string Read(string path) => string.Empty;

            public void Write(string path, string content) { }
        }

        private class DesignSettingsService : ISettingsService
        {
            public event EventHandler<IEnumerable<string>> SettingsUpdated;

            public T Get<T>(string key) => default;

            public bool TryGet<T>(string key, out T value)
            {
                value = default;
                return false;
            }

            public void RaiseSettingsUpdated(IEnumerable<string> updatedKeys)
                => SettingsUpdated?.Invoke(this, updatedKeys);

            public void SaveChanges() { }

            public void Set<T>(string key, T value) { }
        }

        private class DesignSystemMonitorService : ISystemMonitorService
        {
            public ISysInfo Info { get; } = new DesignSysInfo();
            public IBatteryInfo Battery { get; } = new DesignBatteryInfo();
            public IProcessorInfo Processor { get; } = new DesignProcessorInfo();
            public IMemoryInfo Memory { get; } = new DesignMemoryInfo();
            public INetworkInfo Network { get; } = new DesignNetworkInfo();

            public void Update() { }
            public void Dispose() { }

            private class DesignSysInfo : ISysInfo
            {
                public TimeSpan Uptime { get; } = TimeSpan.Parse("03:12:23");

                public void Dispose() { }
                public void Update() { }
            }

            private class DesignBatteryInfo : IBatteryInfo
            {
                public double Percentage { get; } = 87.0;
                public TimeSpan TimeToDepleted { get; } = TimeSpan.Parse("04:32:21");
                public TimeSpan TimeToFullCharge { get; } = TimeSpan.Parse("01:21:54");
                public EBatteryChargeState State { get; } = EBatteryChargeState.Discharging;

                public void Dispose() { }
                public void Update() { }
            }

            private class DesignProcessorInfo : IProcessorInfo
            {
                public double Percentage { get; } = 47.0;

                public void Dispose() { }
                public void Update() { }
            }

            private class DesignMemoryInfo : IMemoryInfo
            {
                public double Total { get; } = 15923.0;
                public double Used { get; } = 9634.0;
                public double Available => Total - Used;
                public string Unit { get; } = "MB";
                public double Percentage => Used * 100 / Total;

                public void Dispose() { }
                public void Update() { }
            }

            private class DesignNetworkInfo : INetworkInfo
            {
                public double DownloadRate { get; } = 81.2;
                public double UploadRate { get; } = 657.3;

                public void Dispose() { }
                public void Update() { }

                public (double rate, string unit) GetDownloadRate(ENetworkDisplayFormat displayFormat)
                    => (DownloadRate, "M");

                public (double rate, string unit) GetUploadRate(ENetworkDisplayFormat displayFormat)
                    => (UploadRate, "K");

            }
        }

        private class DesignWindowService : IWindowService
        {
            public bool Alert(string title, string message) => false;

            public void CloseWindow(IWindowViewModel viewModel) { }

            public void CloseWindow(IWindowViewModel viewModel, bool result) { }

            public bool? OpenDialog(IWindowViewModel viewModel) => false;

            public bool? OpenDialog(IWindowViewModel viewModel, bool allowResize) => false;

            public bool? OpenDialog(IWindowViewModel viewModel, bool allowResize, double? width, double? height) => false;

            public void OpenWindow(IWindowViewModel viewModel) { }

            public void OpenWindow(IWindowViewModel viewModel, bool allowResize) { }

            public void OpenWindow(IWindowViewModel viewModel, bool allowResize, double? width, double? height) { }

            public string ShowColorDialog(string title, string currentColorHex) => string.Empty;

            public void Invoke(Action act) => act();
            public Task InvokeAsync(Func<Task> asyncFunc) => asyncFunc();
            public void Dispose() { }
        }

        private class DesignAppBarController : IAppBarController
        {
            public void DockBottom() { }
            public void Reset() { }
            public void Shutdown() { }
            public void Undock() { }
        }
    }
}
