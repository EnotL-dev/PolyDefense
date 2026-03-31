using System.Linq;
using UnityEngine;
using Construction.Services;
using Construction.Presentation;

namespace Combat
{
    public class TargetingService
    {
        private readonly BuildingRegistry registry;

        public TargetingService(BuildingRegistry registry)
        {
            this.registry = registry;
        }

        public BuildingView GetTarget(Vector3 from)
        {
            var all = registry.GetAll();

            if (all.Count == 0)
                return null;

            return all
                .OrderBy(b => Vector3.Distance(from, b.transform.position))
                .First();
        }
    }
}