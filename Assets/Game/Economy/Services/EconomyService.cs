using Economy.Domain;
using Economy.Presentation;
using Map.Domain;
using System.Collections.Generic;

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

        public void AddGold(int value) // Для киллов
        {
            resourceBase.unitGold.AddValue(value);
        }

        public void IncomeCycle(GridData gridData)
        {
            foreach(Hex hex in gridData.hexagons)
            {
                if(hex.building)
                {
                    if(hex.building.resourcesIncome != null)
                    {
                        foreach(ResourceUnit resourceUnit in hex.building.resourcesIncome)
                        {
                            if (resourceUnit is UnitGold)
                                resourceBase.unitGold.AddValue(resourceUnit.value);
                            if (resourceUnit is UnitIron)
                                resourceBase.unitIron.AddValue(resourceUnit.value);
                            if (resourceUnit is UnitWood)
                                resourceBase.unitWood.AddValue(resourceUnit.value);
                            if (resourceUnit is UnitFood)
                                resourceBase.unitFood.AddValue(resourceUnit.value);
                            if (resourceUnit is UnitEnergy)
                                resourceBase.unitEnergy.AddValue(resourceUnit.value);
                        }
                    }
                }
            }

            economyView.UpdateTexts(resourceBase);
        }

        public void Reduce(List<ResourceUnit> resourceUnits)
        {
            foreach (ResourceUnit resourceUnit in resourceUnits)
            {
                if (resourceUnit is UnitGold)
                    resourceBase.unitGold.ReduceValue(resourceUnit.value);
                if (resourceUnit is UnitIron)
                    resourceBase.unitIron.ReduceValue(resourceUnit.value);
                if (resourceUnit is UnitWood)
                    resourceBase.unitWood.ReduceValue(resourceUnit.value);
                if (resourceUnit is UnitFood)
                    resourceBase.unitFood.ReduceValue(resourceUnit.value);
                if (resourceUnit is UnitEnergy)
                    resourceBase.unitEnergy.ReduceValue(resourceUnit.value);
            }

            economyView.UpdateTexts(resourceBase);
        }

        public void AddLimit(List<ResourceUnit> resourceUnits)
        {
            foreach (ResourceUnit resourceUnit in resourceUnits)
            {
                if (resourceUnit is UnitGold)
                    resourceBase.unitGold.AddLimit(resourceUnit.limit);
                if (resourceUnit is UnitIron)
                    resourceBase.unitIron.AddLimit(resourceUnit.limit);
                if (resourceUnit is UnitWood)
                    resourceBase.unitWood.AddLimit(resourceUnit.limit);
                if (resourceUnit is UnitFood)
                    resourceBase.unitFood.AddLimit(resourceUnit.limit);
                if (resourceUnit is UnitEnergy)
                    resourceBase.unitEnergy.AddLimit(resourceUnit.limit);
            }

            economyView.UpdateTexts(resourceBase);
        }
        public void ReduceLimit(List<ResourceUnit> resourceUnits)
        {
            foreach (ResourceUnit resourceUnit in resourceUnits)
            {
                if (resourceUnit is UnitGold)
                    resourceBase.unitGold.ReduceLimit(resourceUnit.limit);
                if (resourceUnit is UnitIron)
                    resourceBase.unitIron.ReduceLimit(resourceUnit.limit);
                if (resourceUnit is UnitWood)
                    resourceBase.unitWood.ReduceLimit(resourceUnit.limit);
                if (resourceUnit is UnitFood)
                    resourceBase.unitFood.ReduceLimit(resourceUnit.limit);
                if (resourceUnit is UnitEnergy)
                    resourceBase.unitEnergy.ReduceLimit(resourceUnit.limit);
            }

            economyView.UpdateTexts(resourceBase);
        }

        public bool CheckSum(ResourceUnit resourceUnit)
        {
            if (resourceUnit is UnitGold)
                return resourceBase.unitGold.value >= resourceUnit.value;
            if (resourceUnit is UnitIron)
                return resourceBase.unitIron.value >= resourceUnit.value;
            if (resourceUnit is UnitWood)
                return resourceBase.unitWood.value >= resourceUnit.value;
            if (resourceUnit is UnitFood)
                return resourceBase.unitFood.value >= resourceUnit.value;
            if (resourceUnit is UnitEnergy)
                return resourceBase.unitEnergy.value >= resourceUnit.value;

            return false;
        }
    }
}
