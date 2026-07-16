using Construction.Config;
using Economy.Domain;
using Economy.Services;
using Map.Domain;
using Map.Presentation;
using UnityEngine;

namespace Construction.Services
{
    public class BuildService : IBuildService
    {
        private readonly MapView mapView;
        private readonly IEconomyService economyService;
        private readonly BuildingFactory buildingFactory;

        public BuildService(MapView mapView, IEconomyService economyService, BuildingFactory buildingFactory)
        {
            this.mapView = mapView;
            this.economyService = economyService;
            this.buildingFactory = buildingFactory;
        }

        public bool CheckBuild(Building building)
        {
            foreach (ResourceUnit resource in building.resourcesCost)
            {
                if (!economyService.CheckSum(resource))
                    return false;
            }

            return true;
        }

        public void Build(Building building, Hex hex)
        {
            economyService.Reduce(building.resourcesCost);
            economyService.AddLimit(building.resourcesAddLimit);

            Building newBuilding = buildingFactory.Create(building);
            hex.SetBuilding(newBuilding); 

            GameObject cell = building.prefabCell;
            mapView.ChangeCell(hex, cell);
        }
    }
}