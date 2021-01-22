using System;

namespace Cajetan.Infobar.Domain.Models
{
    public interface ISysInfo : IUpdatableInfo
    {
        TimeSpan Uptime { get; }
    }
}
