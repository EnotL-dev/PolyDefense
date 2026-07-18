using System.Collections.Generic;
using UnityEngine;

namespace Combat.Enemó
{
    [CreateAssetMenu(fileName = "EnemiesData", menuName = "Enemy/EnemiesData")]
    public class EnemiesData : ScriptableObject
    {
        public List<EnemyConfig> enemies;
    }
}
