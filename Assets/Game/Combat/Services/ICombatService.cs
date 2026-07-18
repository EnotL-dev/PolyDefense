using Combat.Enemó;
using Cysharp.Threading.Tasks;
using Map.Domain;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Services
{
    public interface ICombatService
    {
        void StartWawe();
        void EndWawe();
        void SpawnEnemies();
        void RemoveEnemyFromPool(int id);
        void SetPositionToEnemy(int id, Hex hex);
        UniTask WaweInProcess();
        IEnemy GetFirstEnemyInMyZone(Hex hex);
    }
}
