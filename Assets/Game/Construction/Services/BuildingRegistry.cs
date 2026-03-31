using Construction.Presentation;
using System.Collections.Generic;

namespace Construction.Services
{
    public class BuildingRegistry
    {
        private readonly List<BuildingView> buildings = new();

        public void Register(BuildingView view)
        {
            buildings.Add(view);
        }

        public void Unregister(BuildingView view)
        {
            buildings.Remove(view);
        }

        public List<BuildingView> GetAll() => buildings;
    }
}