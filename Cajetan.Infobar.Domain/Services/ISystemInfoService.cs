using System;

namespace Cajetan.Infobar.Domain.Services
{
    public interface ISystemInfoService
    {
        // Battery
        int BatteryPercentage { get; }
        bool BatteryShowTime { get; set; }
        string BatteryStatusString { get; }
        TimeSpan BatteryTimeRemaining { get; }

        // Memory
        int MemoryTotal { get; }
        string MemoryUsageString { get; }
        int MemoryUsed { get; }
        int MemoryUsedPercentage { get; }

        // Network
        double NetworkDownload { get; }
        string NetworkDownloadString { get; }
        double NetworkUpload { get; }
        string NetworkUploadString { get; }

        // Processor
        int ProcessorUsage { get; }
        string ProcessorUsageString { get; }

        // Uptime
        TimeSpan Uptime { get; }
        string UptimeString { get; }

        // Update
        void UpdateInfo();
    }
}
