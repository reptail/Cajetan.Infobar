using Cajetan.Infobar.Domain.Services;
using si = StarshipInfo;
using System;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Cajetan.Infobar.Domain.Models;

namespace Cajetan.Infobar.Services
{
    public class StarshipInfoService : IStarshipInfoService
    {
        private readonly ISettingsService _settingsService;

        private HttpClient _httpClient;
        private si.IStarshipInfoApiClient _apiClient;

        private bool _isDisposed;
        private DateTime _lastUpdateUtc;
        private Task _updateTask;

        private int? _nextSpaceflightLaunchId;
        private si.Overview _lastOverview;

        private static readonly TimeSpan _updateInterval = TimeSpan.FromHours(1);

        public StarshipInfoService(ISettingsService settingsService)
        {
            _httpClient = new HttpClient();
            _apiClient = new si.StarshipInfoApiClient("https://cajetan-starshipinfo.azurewebsites.net", _httpClient);
            _lastUpdateUtc = DateTime.MinValue;
            _settingsService = settingsService;
        }

        public bool HasOverview()
        {
            return _lastOverview is not null;
        }

        public string GetNextFlightRestriction()
        {
            si.FlightRestrictionDetails next = _lastOverview?.FlightRestrictions?
                .OrderBy(r => r.FromUtc)
                .FirstOrDefault();

            if (next is null)
                return $"No pending TFRs";

            return FormatFlightRestriction(next);
        }

        public string[] GetFlightRestrictions()
        {
            return _lastOverview?.FlightRestrictions?
                   .OrderBy(r => r.FromUtc)
                   .Select(FormatFlightRestriction)
                   .ToArray() ?? Array.Empty<string>();
        }

        public string GetNextRoadClosure()
        {
            si.RoadClosure next = _lastOverview?.RoadClosures?
                .OrderBy(c => c.FromUtc)
                .FirstOrDefault();

            if (next is null)
                return $"No upcoming road closures";

            return FormatRoadClosure(next);
        }

        public string[] GetRoadClosures()
        {
            return _lastOverview?.RoadClosures?
                .OrderBy(c => c.FromUtc)
                .Select(FormatRoadClosure)
                .ToArray() ?? Array.Empty<string>();
        }

        public string GetLaunchDetails()
        {
            if (_lastOverview?.LaunchDetails is null)
                return "No launch details available!";

            if (string.IsNullOrWhiteSpace(_lastOverview?.LaunchDetails?.Time))
                return "Unknown launch time!";

            string strVehicle = _lastOverview?.LaunchDetails?.Vehicle
                .Split("(").FirstOrDefault() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(strVehicle))
                strVehicle = $"{strVehicle.Trim()}: ";

            return $"{strVehicle}{_lastOverview.LaunchDetails.Time}";
        }


        public void Update()
        {
            if (_isDisposed) return;
            if (_updateTask is not null) return;

            if (!_settingsService.TryGet(SettingsKeys.STARSHIP_INFO_IS_ENABLED, out bool isEnabled) || !isEnabled)
                return;

            DateTime nextUpdateUtc = _lastUpdateUtc.Add(_updateInterval);

            if (nextUpdateUtc >= DateTime.UtcNow)
                return;

            if (!_settingsService.TryGet(SettingsKeys.STARSHIP_INFO_LAUNCH_ID, out _nextSpaceflightLaunchId))
                _nextSpaceflightLaunchId = null;

            _updateTask = InternalUpdateAsync();
        }

        private async Task InternalUpdateAsync()
        {
            if (_isDisposed) return;

            try
            {
                _lastOverview = await _apiClient.GetOverviewAsync(_nextSpaceflightLaunchId)
                    .ConfigureAwait(false);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _lastUpdateUtc = DateTime.UtcNow;
                _updateTask = null;
            }
        }

        private static string FormatFlightRestriction(si.FlightRestrictionDetails flightRestriction)
        {
            if (flightRestriction is null) return string.Empty;

            string periodFrom = $"{flightRestriction.FromUtc.LocalDateTime:MMM dd, HH:mm}";
            string periodTo = $"{flightRestriction.ToUtc.LocalDateTime:HH:mm}";

            return $"{periodFrom} to {periodTo} - {flightRestriction.Altitude}";
        }
        private static string FormatRoadClosure(si.RoadClosure roadClosure)
        {
            string periodFrom = $"{roadClosure.FromUtc.LocalDateTime:MMM dd, HH:mm}";
            string periodTo = $"{roadClosure.ToUtc.LocalDateTime:HH:mm}";

            return $"{periodFrom} to {periodTo} ({roadClosure.Status})";
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _apiClient = null;
                    _httpClient?.Dispose();
                    _httpClient = null;
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
