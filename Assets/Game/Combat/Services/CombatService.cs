using Construction.Config;
using DG.Tweening;
using UnityEngine;

namespace Combat
{
    public class CombatService
    {
        public void Attack(Enemy enemy, Building target)
        {
            Debug.Log("Atack");

            enemy.transform
                .DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f);

            target.transform
                .DOPunchPosition(Vector3.up * 0.3f, 0.3f);
        }
    }
}
