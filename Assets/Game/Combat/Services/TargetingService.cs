using System.Linq;
using UnityEngine;
using Construction.Config;

namespace Combat
{
    public class TargetingService
    {
        public Building GetTarget(Vector3 from)
        {
            var buildings = Object.FindObjectsOfType<Building>();

            if (buildings.Length == 0)
                return null;

            return buildings
                .OrderBy(b => Vector3.Distance(from, b.transform.position))
                .First();
        }
    }
}