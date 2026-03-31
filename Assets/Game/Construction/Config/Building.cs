using Economy.Domain;
using Map.Domain;
using System.Collections.Generic;
using UnityEngine;

namespace Construction.Config
{
    [CreateAssetMenu(fileName = "Building", menuName = "Buildings/Building")]
    public class Building : ScriptableObject
    {
        public string title;
        public int currentHp = 100;
        public int maxHp = 100;

        public BiomeType biome;
        public GameObject prefabCell;

        [Space(5)]
        [SerializeReference, SubclassSelector] public List<ResourceUnit> resourcesCost;
        [Space(5)]
        [SerializeReference, SubclassSelector] public List<ResourceUnit> resourcesIncome;
        [Space(5)]
        [SerializeReference, SubclassSelector] public List<ResourceUnit> resourcesAddLimit;

        public void TakeDamage(int damage)
        {
            currentHp -= damage;

            if (currentHp <= 0)
                currentHp = 0;
        }

        public bool IsAlive => currentHp > 0;
    }
}
