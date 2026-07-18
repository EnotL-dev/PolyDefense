using UnityEngine;

namespace Combat.Defense
{
    [CreateAssetMenu(fileName = "DefenseConfig", menuName = "Defense/DefenseConfig")]
    public class DefenseConfig : ScriptableObject
    {
        [Range(1, 10)] public int distance = 1;
        [Range(1, 20)] public int damage = 10;
        public float delay = 0.5f;
        [Space(5)]
        public GameObject projectilePrefab;
    }
}
