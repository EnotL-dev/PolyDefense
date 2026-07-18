using Economy.Domain;
using System.Collections.Generic;
using UnityEngine;

namespace Construction.Config
{
    public class BuildingFactory
    {
        public Building Create(Building original)
        {
            Building copy = ScriptableObject.CreateInstance<Building>();

            copy.buildingName = original.buildingName;
            copy.description = original.description;
            copy.currentHp = original.maxHp;
            copy.maxHp = original.maxHp;
            copy.biome = original.biome;
            copy.prefabCell = original.prefabCell;
            copy.defenseConfig = original.defenseConfig;

            copy.resourcesCost = new List<ResourceUnit>(original.resourcesCost);
            copy.resourcesIncome = new List<ResourceUnit>(original.resourcesIncome);
            copy.resourcesAddLimit = new List<ResourceUnit>(original.resourcesAddLimit);

            return copy;
        }
    }
}
