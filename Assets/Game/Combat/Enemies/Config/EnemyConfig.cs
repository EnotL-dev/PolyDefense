using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "EnemyConfig/Enemy")]
    public class EnemyConfig : ScriptableObject
    {
        public GameObject prefab;

        public float moveSpeed = 2f;
        public int damagePerSecond = 5;
    }
}