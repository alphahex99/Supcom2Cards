using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using ModdingUtils.MonoBehaviours;

namespace Supcom2Cards.MonoBehaviours
{
    public class HardenEffect : CounterReversibleEffect
    {
        public int HowMany = 0;

        private bool active = false;
        private float counter = 0;

        public void Activate(float seconds)
        {
            active = false;
            counter += HowMany * seconds;
        }

        public override CounterStatus UpdateCounter()
        {
            counter -= Time.deltaTime;

            if (!active && counter > 0)
            {
                active = true;
                return CounterStatus.Apply;
            }
            else if (counter <= 0)
            {
                active = false;
                Reset();
                return CounterStatus.Remove;
            }
            return CounterStatus.Wait;
        }

        public override void UpdateEffects()
        {
            // gun
            gunStatModifier.attackSpeed_mult = 0.5f;

            // projectile
            gunStatModifier.projectileSpeed_mult *= 2f;
        }

        public override void OnApply()
        {
            
        }

        public override void Reset()
        {
            counter = 0;
            active = false;
        }
    }
}