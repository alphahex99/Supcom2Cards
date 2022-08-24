using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using ModdingUtils.MonoBehaviours;

namespace Supcom2Cards.MonoBehaviours
{
    public class HardenEffect : CounterReversibleEffect
    {
        public bool Active = false;
        public float Counter = 0;

        public override CounterStatus UpdateCounter()
        {
            Counter -= Time.deltaTime;

            if (!Active && Counter > 0)
            {
                Active = true;
                return CounterStatus.Apply;
            }
            else if (Counter <= 0)
            {
                Active = false;
                Reset();
                return CounterStatus.Remove;
            }
            return CounterStatus.Wait;
        }

        public override void UpdateEffects()
        {
            // gun
            gunStatModifier.attackSpeed_mult = 0.5f;
            gunAmmo.ReloadAmmo(false);

            // projectile
            gunStatModifier.projectileSpeed_mult *= 2f;
        }

        public override void OnApply()
        {
            
        }

        public override void Reset()
        {
            Counter = 0;
            Active = false;
        }
    }
}