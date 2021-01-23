using System;

namespace Cajetan.Infobar.Domain.Models
{
    public interface IUpdatableInfo : IDisposable
    {
        void Update();
    }
}
