using Construction.Config;
using Cysharp.Threading.Tasks;
using Map.Domain;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Enemó
{
    public interface IEnemy
    {
        int _id { get; set; }
        void Initialize(int id, EnemyConfig config, Action<int> OnDeath, Action<Hex> DestroyBuilding, Action<int, Hex> InvokePosition);
        UniTaskVoid Attack(Building building, Hex hex);
        UniTaskVoid Moving(List<Hex> path);
        void TakeDamage(int damage);
        void Die();
    }
}
