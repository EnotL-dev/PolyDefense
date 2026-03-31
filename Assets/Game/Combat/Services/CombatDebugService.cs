using UnityEngine;
using Zenject;

namespace Combat
{
    public class CombatDebugService : ITickable
    {
        private readonly EnemyFactory factory;
        private readonly EnemyConfig config;

        public CombatDebugService(
            EnemyFactory factory,
            EnemyConfig config)
        {
            this.factory = factory;
            this.config = config;
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SpawnEnemy();
            }
        }

        void SpawnEnemy()
        {
            Vector3 spawnPos = new Vector3(0, 0, -5);

            factory.Create(config, spawnPos);

            Debug.Log("Enemy spawned");
        }
    }
}