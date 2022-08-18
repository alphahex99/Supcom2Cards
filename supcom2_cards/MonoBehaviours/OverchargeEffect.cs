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
                active = true;
                return CounterStatus.Apply;
            }
            else if (shotsLeft <= 0)
            {
                active = false;
                return CounterStatus.Remove;
            }
            return CounterStatus.Wait;
        }

        public override void UpdateEffects()
        {
            gunStatModifier.attackSpeed_mult = 0.5f;
            gunStatModifier.bulletDamageMultiplier_mult = 2f;
            gunStatModifier.size_add = 10f;
            gunStatModifier.explodeNearEnemyRange_add = 5f;
        }

        public override void OnApply()
        {
            Reset();
        }

        public override void Reset()
        {
        }
    }
}