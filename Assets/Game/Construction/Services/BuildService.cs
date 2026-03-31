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

        public BuildService(MapView mapView, IEconomyService economyService)
        {
            this.mapView = mapView;
            this.economyService = economyService;
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

            hex.building = building;

            GameObject cell = building.prefabCell;

            mapView.ChangeCell(hex, cell);
        }
    }
}
