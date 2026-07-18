using Combat.Enemу;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Economy.Services;
using Map.Domain;
using Map.Presentation;
using Map.Services;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Combat.Services
{
    public class CombatService : ICombatService
    {
        private readonly IGameStateMachine stateMachine;
        private readonly IEconomyService economyService;
        private readonly IMapService mapService;

        private int day = 1;

        [Inject] EnemyFactory enemyFactory;
        [Inject] MapView mapView;

        private int next_id = 0;
        private Dictionary<int, Hex> hexPool = new Dictionary<int, Hex>();
        private Dictionary<int, IEnemy> enemyPool = new Dictionary<int, IEnemy>();

        private EnemiesData enemiesData;
        private bool GameOver = false;
        public CombatService(IGameStateMachine stateMachine, IEconomyService economyService, IMapService mapService)
        {
            this.stateMachine = stateMachine;
            this.economyService = economyService;
            this.mapService = mapService;

            enemiesData = Resources.Load<EnemiesData>("Combat/Enemies/EnemiesData");
        }

        public void StartWawe()
        {
            ClearPools();
            SpawnEnemies();
            WaweInProcess().Forget();
        }

        public void SpawnEnemies()
        {
            enemyPool = new Dictionary<int, IEnemy>();

            int scoreForSpawn = ScoreForSpawn();

            while (scoreForSpawn >= 2)
            {
                foreach (EnemyConfig config in enemiesData.enemies)
                {
                    if (scoreForSpawn < config.cost)
                        continue;

                    IEnemy newEnemy = enemyFactory.Create(next_id, config, RemoveEnemyFromPool, DestroyBuilding, SetPositionToEnemy);
                    enemyPool.Add(next_id, newEnemy);

                    next_id++;
                    scoreForSpawn -= config.cost;
                }

                scoreForSpawn--;
            }
        }

        private int ScoreForSpawn()
        {
            if (day < 8)
                return day * 3;
            else
                return day * 4;
        }

        public void RemoveEnemyFromPool(int id)
        {
            economyService.AddGold(2); // Добавляет золото за убитоо противника
            enemyPool.Remove(id);
            hexPool.Remove(id);
        }

        public void SetPositionToEnemy(int id, Hex hex) => hexPool[id] = hex;

        public async UniTask WaweInProcess()
        {
            while (enemyPool.Count > 0)
            {
                await UniTask.Yield();
            }

            EndWawe();
        }

        public void EndWawe()
        {
            ClearPools();

            day++;
            stateMachine.Enter<DayState>();
        }

        private void ClearPools()
        {
            if (enemyPool != null && enemyPool.Count > 0)
            {
                foreach (var pair in enemyPool)
                {
                    IEnemy enemy = pair.Value;

                    enemy.Die(); //Чтобы не только вышло из пулла, но и вызвались токены отмены
                }
            }

            hexPool.Clear();
        }

        private void DestroyBuilding(Hex hex)
        {
            if (GameOver)
                return;

            if (hex.building.biome == BiomeType.TownHall)
            {
                GameOver = true;
                Debug.Log("<color=red>End Game</color>");

                stateMachine.Enter<LoseState>();
                return;
            }

            if (hex.building.resourcesAddLimit != null && hex.building.resourcesAddLimit.Count > 0)
                economyService.ReduceLimit(hex.building.resourcesAddLimit);

            hex.DestroyBuilding();
            mapView.ChangeCellByBiome(hex);
        }

        private List<Hex> GetNeighborsInRadius(Hex center, int radius)
        {
            List<Hex> result = new List<Hex>();
            result.Add(center);

            if (radius <= 0)
                return result;

            for (int dq = -radius; dq <= radius; dq++)
            {
                for (int dr = Mathf.Max(-radius, -dq - radius); dr <= Mathf.Min(radius, -dq + radius); dr++)
                {
                    int ds = -dq - dr;

                    if (dq == 0 && dr == 0 && ds == 0)
                        continue;

                    Hex neighbor = mapService.CurrentMap.GetHexAt(center.q + dq, center.r + dr, center.s + ds);

                    if (neighbor != null)
                        result.Add(neighbor);
                }
            }

            return result;
        }

        public IEnemy GetFirstEnemyInMyZone(Hex hex)
        {
            if (hex.building == null || !hex.building.defenseConfig || GameOver)
                return null;

            List<Hex> neighbors = GetNeighborsInRadius(hex, hex.building.defenseConfig.distance);
            foreach (var pair in hexPool)
            {
                foreach (Hex checkHex in neighbors)
                    if (checkHex == pair.Value)
                    {
                        if (enemyPool.ContainsKey(pair.Key))
                            return enemyPool[pair.Key];
                    }
            }

            return null;
        }
    }
}
