using Combat.Services;
using Construction.Config;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Map.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Combat.Enemу
{
    public class EnemyView : MonoBehaviour, IEnemy
    {
        [Inject] INavigationService navigationService;
        public int _id { get; set; }
        private EnemyConfig config;
        private event Action<int> OnDeath;
        private event Action<Hex> DestroyBuilding;
        private event Action<int, Hex> InvokePosition;

        public Slider hpSlider;

        private CancellationTokenSource cts;

        public void Initialize(int _id, EnemyConfig config, Action<int> OnDeath, Action<Hex> DestroyBuilding, Action<int, Hex> InvokePosition)
        {
            this._id = _id;
            this.config = config;

            hpSlider.minValue = 0;
            hpSlider.maxValue = config.health;

            this.OnDeath = OnDeath;
            this.DestroyBuilding = DestroyBuilding;
            this.InvokePosition = InvokePosition;

            cts = new CancellationTokenSource();
            List<Hex> path = navigationService.FindPathFromEdgeToBuilding();
            transform.position = navigationService.GetHexWorldPosition(path[0]) + new Vector3(0, 0.3f, 0);
            Moving(path).Forget();
        }

        private Tween currentTween;
        public async UniTaskVoid Moving(List<Hex> path)
        {
            var token = cts.Token;

            try
            {
                while (!token.IsCancellationRequested)
                {
                    currentTween?.Kill();
                    transform.DOKill();

                    if (path.Count < 1)
                        return;

                    Building targetBuilding = null;
                    for (int i = 0; i < path.Count; i++)
                    {
                        InvokePosition?.Invoke(_id, path[i]); // Уведомление о текущей точке
                        token.ThrowIfCancellationRequested();

                        float deviationX = UnityEngine.Random.Range(-0.1f, 0.1f);
                        float deviationZ = UnityEngine.Random.Range(-0.1f, 0.1f);
                        Vector3 targetPos = navigationService.GetHexWorldPosition(path[i]) + new Vector3(deviationX, 0.3f, deviationZ);
                        if (i == path.Count - 1)
                        {
                            Vector3 direction = (targetPos - transform.position).normalized;
                            targetPos = targetPos - direction * 0.5f;

                            targetBuilding = path[i].building;
                        }

                        currentTween = transform.DOMove(targetPos, config.speed).SetEase(Ease.Linear);
                        await currentTween.AsyncWaitForCompletion();
                    }
                    token.ThrowIfCancellationRequested();

                    Attack(targetBuilding, path[path.Count - 1]).Forget();
                    break;
                }
            }
            catch (OperationCanceledException) { }
        }

        public async UniTaskVoid Attack(Building building, Hex hex)
        {
            var token = cts.Token;

            try
            {
                currentTween?.Kill();
                transform.DOKill();

                Vector3 cellCenter = navigationService.GetHexWorldPosition(hex);
                Vector3 direction = (cellCenter - transform.position).normalized;
                direction.y = 0;
                transform.rotation = Quaternion.LookRotation(direction);

                while (building.currentHp > 0)
                {
                    token.ThrowIfCancellationRequested();

                    hex.TakeBuildingDamage(config.damage);
                    await transform.DOPunchPosition(Vector3.forward * 0.2f, 2, 1, 0).AsyncWaitForCompletion();

                    int delay = UnityEngine.Random.Range(500, 1000);
                    await UniTask.Delay(delay, cancellationToken: token);
                }
                token.ThrowIfCancellationRequested();

                //Из-за асинхронности замену запускаем через проверку
                if (hex.building != null)
                    DestroyBuilding?.Invoke(hex);

                List<Hex> path = navigationService.FindPathFromCurrentHexToBuilding(hex);
                Moving(path).Forget();
            }
            catch (OperationCanceledException) { }
        }

        public void TakeDamage(int damage)
        {
            config.health -= damage;
            hpSlider.value = config.health;

            if (config.health <= 0)
            {
                hpSlider.value = 0;
                Die();
            }
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        void OnDestroy()
        {
            cts?.Cancel();
            currentTween?.Kill();
            OnDeath?.Invoke(_id);
        }
    }
}