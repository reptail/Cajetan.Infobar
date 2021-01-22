using System;

namespace Cajetan.Infobar.Domain.Models
{
    public interface IBatteryInfo : IUpdatableInfo
    {
        double Percentage { get; }
        TimeSpan TimeToDepleted { get; }
        TimeSpan TimeToFullCharge { get; }
        EBatteryChargeState State { get; }
    }
}
