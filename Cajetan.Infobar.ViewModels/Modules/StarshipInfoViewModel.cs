using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;

namespace Cajetan.Infobar.ViewModels.Modules
{
    public class StarshipInfoViewModel : ModuleViewModelBase
    {
        private readonly IStarshipInfoService _starshipInfoService;

        public StarshipInfoViewModel(ISettingsService settingsService, IStarshipInfoService starshipInfoService)
            : base(settingsService)
        {
            _starshipInfoService = starshipInfoService;
        }

        public override EModuleType ModuleType { get; } = EModuleType.StarshipInfo;

        public override void RefreshData()
        {
            // Refresh data, but with internal limit to throttle API calls (e.g. every 10min).
        }

        protected override void InternalUpdate()
        {
            // Load Settings..
        }
    }
}
