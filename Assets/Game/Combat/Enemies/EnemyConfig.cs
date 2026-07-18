using UnityEngine;

namespace Combat.Enemó
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Enemy/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        public int cost = 1;
        [Space(5)]
        public string enemyName;
        public GameObject prefab;
        public int health;
        public int damage;
        public float speed;
    }
}