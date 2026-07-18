using Combat.Enemó;
using Combat.Services;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Map.Domain;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Construction.Presentation
{
    public class DefenseBuildingView : MonoBehaviour
    {
        [Inject] SignalBus signalBus;
        [Inject] ICombatService combatService;

        private Transform _projectileParent;
        private List<GameObject> projectilePool = new List<GameObject>();
        private Hex hex;

        public void Initialize(Hex hex)
        {
            this.hex = hex;
        }

        private CancellationTokenSource _cts;

        private void Start()
        {
            signalBus.Subscribe<StateChangedSignal>(ChangeFlag);

            _projectileParent = new GameObject("_Projectiles").transform;
            _projectileParent.SetParent(transform);

            _cts = new CancellationTokenSource();
            Defending(_cts.Token).Forget();
        }

        private async UniTaskVoid Defending(CancellationToken token)
        {
            int delay = (int)(hex.building.defenseConfig.delay * 1000);
            while (!token.IsCancellationRequested)
            {
                await UniTask.WaitWhile(() => isPaused, cancellationToken: token);
                await UniTask.Delay(delay, cancellationToken: token);

                Shoot();
            }
        }

        private void Shoot()
        {
            IEnemy enemy = combatService.GetFirstEnemyInMyZone(hex);
            if (enemy == null)
                return;

            GameObject projectile;
            if (projectilePool.Count < 1)
            {
                projectile = Instantiate(hex.building.defenseConfig.projectilePrefab);
                projectile.transform.SetParent(_projectileParent);
                projectilePool.Add(projectile);
            }
            else
            {
                projectile = projectilePool.Find(p => !p.activeSelf);
                if (!projectile)
                {
                    projectile = Instantiate(hex.building.defenseConfig.projectilePrefab);
                    projectile.transform.SetParent(_projectileParent);
                    projectilePool.Add(projectile);
                }
            }

            projectile.transform.position = gameObject.transform.position + Vector3.up;
            projectile.SetActive(true);

            FollowTarget(projectile, enemy, _cts.Token).Forget();
        }

        private async UniTaskVoid FollowTarget(GameObject projectile, IEnemy enemyInt, CancellationToken token)
        {
            if (enemyInt is EnemyView enemyView)
            {
                try
                {
                    float speed = 3f;
                    while (!token.IsCancellationRequested && enemyView != null)
                    {
                        Vector3 targetPos = enemyView.transform.position;
                        Vector3 direction = (targetPos - projectile.transform.position).normalized;

                        projectile.transform.position += direction * speed * Time.deltaTime;

                        if (Vector3.Distance(projectile.transform.position, targetPos) < 0.1f)
                        {
                            enemyView.TakeDamage(hex.building.defenseConfig.damage);

                            projectile.SetActive(false);
                            return;
                        }

                        await UniTask.Yield(token);
                    }
                }
                catch { }
            }

            projectile.SetActive(false);
        }

        public void OnDestroy()
        {
            signalBus.Unsubscribe<StateChangedSignal>(ChangeFlag);

            _cts?.Cancel();
            _cts?.Dispose();
        }

        private bool isPaused = true;
        private void ChangeFlag(StateChangedSignal stateChangedSignal)
        {
            if (stateChangedSignal.gameState is DayState)
                isPaused = true;
            if (stateChangedSignal.gameState is NightState)
                isPaused = false;
        }
    }
}
