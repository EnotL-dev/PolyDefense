using Construction.Config;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Combat
{
    public class Enemy : MonoBehaviour
    {
        private NavigationService navigation;
        private CombatService combat;
        private TargetingService targeting;

        private EnemyConfig config;

        [Inject]
        public void Construct(
            NavigationService navigation,
            CombatService combat,
            TargetingService targeting)
        {
            this.navigation = navigation;
            this.combat = combat;
            this.targeting = targeting;
        }

        public void Init(EnemyConfig config)
        {
            this.config = config;

            Run().Forget();
        }

        async UniTaskVoid Run()
        {
            var target = targeting.GetTarget(transform.position);

            if (target == null)
            {
                Debug.Log("No target");
                return;
            }

            var path = navigation.GetPath(transform.position, target.transform.position);

            foreach (var point in path)
            {
                RotateTo(point);

                await transform
                    .DOMove(point, config.moveSpeed)
                    .SetEase(Ease.Linear)
                    .AsyncWaitForCompletion();
            }

            Attack(target);
        }

        void RotateTo(Vector3 point)
        {
            Vector3 dir = (point - transform.position).normalized;

            if (dir != Vector3.zero)
            {
                transform.DORotateQuaternion(
                    Quaternion.LookRotation(dir),
                    0.2f
                );
            }
        }

        void Attack(Building target)
        {
            combat.Attack(this, target);
        }
    }
}
