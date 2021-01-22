using Cajetan.Infobar.Domain.Models;

namespace Cajetan.Infobar.Domain.Services
{
    public interface ISystemMonitorService
    {
        ISysInfo Info { get; }
        IBatteryInfo Battery { get; }
        IProcessorInfo Processor { get; }
        IMemoryInfo Memory { get; }
        INetworkInfo Network { get; }

        void Update();
    }
}
