using Cajetan.Infobar.Domain.Models;
using System;

namespace Cajetan.Infobar.Domain.Services
{
    public interface ISystemMonitorService : IDisposable
    {
        ISysInfo Info { get; }
        IBatteryInfo Battery { get; }
        IProcessorInfo Processor { get; }
        IMemoryInfo Memory { get; }
        INetworkInfo Network { get; }

        void Update();
    }
}
