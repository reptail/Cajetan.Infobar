using System;

namespace Cajetan.Infobar.Domain.Services
{
    public interface IStarshipInfoService : IDisposable
    {
        bool HasOverview();

        string GetNextFlightRestriction();
        string[] GetFlightRestrictions();

        string GetNextRoadClosure();
        string[] GetRoadClosures();

        string GetLaunchDetails();

        void Update();
    }
}
