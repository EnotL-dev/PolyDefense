using UnityEngine;
using Zenject;

namespace Combat
{
    public class EnemyFactory
    {
        private readonly DiContainer container;

        public EnemyFactory(DiContainer container)
        {
            this.container = container;
        }

        public Enemy Create(EnemyConfig config, Vector3 position)
        {
            var obj = container.InstantiatePrefab(
               config.prefab,
               position,
               Quaternion.identity,
               null
               );

            var enemy = obj.GetComponent<Enemy>();
            enemy.Init(config);

            return enemy;
        }
    }
}
