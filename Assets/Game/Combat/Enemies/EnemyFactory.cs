using Map.Domain;
using System;
using UnityEngine;
using Zenject;

namespace Combat.Enemó
{
    public class EnemyFactory
    {
        private readonly DiContainer container;

        public EnemyFactory(DiContainer container)
        {
            this.container = container;
        }

        public IEnemy Create(int id, EnemyConfig config, Action<int> OnDeath, Action<Hex> DestroyBuilding, Action<int, Hex> InvokePosition)
        {
            EnemyConfig newConfig = ScriptableObject.CreateInstance<EnemyConfig>();
            newConfig.cost = config.cost;
            newConfig.enemyName = config.enemyName;
            newConfig.prefab = config.prefab;
            newConfig.health = config.health;
            newConfig.damage = config.damage;
            newConfig.speed = config.speed;

            GameObject obj = container.InstantiatePrefab(
                newConfig.prefab,
                Vector3.zero,
                Quaternion.identity,
                null
            );

            IEnemy enemy = obj.GetComponent<IEnemy>();
            enemy.Initialize(id, newConfig, OnDeath, DestroyBuilding, InvokePosition);

            return enemy;
        }
    }
}
