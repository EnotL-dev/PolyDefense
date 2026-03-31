using Construction.Presentation;
using DG.Tweening;
using UnityEngine;

namespace Combat
{
    public class CombatService
    {
        public void DealDamage(BuildingView target, int dps)
        {
            target.Building.TakeDamage(dps);

            target.transform
                .DOPunchPosition(Vector3.up * 0.3f, 0.2f);

            Debug.Log($"Building HP: {target.Building.currentHp}");

            if (!target.Building.IsAlive)
            {
                Object.Destroy(target.gameObject);
            }
        }
    }
}
