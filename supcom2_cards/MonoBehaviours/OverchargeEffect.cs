using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using ModdingUtils.MonoBehaviours;

namespace Supcom2Cards.MonoBehaviours
{
    public class OverchargeEffect : CounterReversibleEffect
    {
        public int shotsLeft = 0;
        public bool active = false;

        public override CounterStatus UpdateCounter()
        {
            if (!active && shotsLeft > 0)
            {
                return CounterStatus.Apply;
            }
            else if (shotsLeft <= 0)
            {
                return CounterStatus.Remove;
            }
            return CounterStatus.Wait;
        }

        public override void UpdateEffects()
        {
            gunStatModifier.attackSpeed_mult = 0.5f;
            gunStatModifier.bulletDamageMultiplier_mult = 2f;

            gunStatModifier.projectileSize_add = 3f;
            gunStatModifier.projectileSize_mult = 5f;
            gunStatModifier.projectileSpeed_add = 2f;

            gunStatModifier.recoilMuiltiplier_add = 2f;
            gunStatModifier.explodeNearEnemyRange_add = 1f;
        }

        public void OnShootProjectileAction(GameObject obj)
        {
            if (active)
            {
                shotsLeft--;
            }
        }

        public override void OnApply()
        {
            Reset();
        }

        public override void Reset()
        {
        }

        public override void OnOnEnable()
        {
            shotsLeft = 0;
            active = false;
            Reset();
            ClearModifiers();
            OnRemove();
        }

        public override void OnOnDisable()
        {
            Reset();
            ClearModifiers();
            OnRemove();
        }
    }
}