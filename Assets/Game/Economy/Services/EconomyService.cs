using Economy.Domain;
using Economy.Presentation;
using Map.Domain;

namespace Economy.Services
{
    public class EconomyService : IEconomyService
    {
        private readonly ResourceBase resourceBase;
        private readonly EconomyView economyView;

        public EconomyService(ResourceBase resourceBase, EconomyView economyView)
        {
            this.resourceBase = resourceBase;
            this.economyView = economyView;

            economyView.UpdateTexts(this.resourceBase);
        }

        public void CalculateCycle(GridData gridData)
        {
            economyView.UpdateTexts(resourceBase);
        }
    }
}
