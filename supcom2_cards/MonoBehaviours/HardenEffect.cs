using UnityEngine;
using ModdingUtils.MonoBehaviours;
using System;

namespace Supcom2Cards.MonoBehaviours
{
    public class HardenEffect : CounterReversibleEffect
    {
        public int HowMany = 0;

        private bool active = false;
        private float counter = 0;

        private Action<BlockTrigger.BlockTriggerType>? blockAction;

        public void Activate()
        {
            counter += Cards.Harden.HARDEN_SECONDS * HowMany;
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

        public override void OnStart()
        {
            blockAction = GetBlockAction(player);
            block.BlockAction += blockAction;

            base.OnStart();
        }

        public override void OnOnDestroy()
        {
            if (blockAction != null)
            {
                block.BlockAction -= blockAction;
            }
        }

        private Action<BlockTrigger.BlockTriggerType> GetBlockAction(Player player)
        {
            return delegate (BlockTrigger.BlockTriggerType trigger)
            {
                if (trigger == BlockTrigger.BlockTriggerType.Default ||
                    trigger == BlockTrigger.BlockTriggerType.Echo ||
                    trigger == BlockTrigger.BlockTriggerType.ShieldCharge)
                {
                    Activate();
                }
            };
        }

        public override void Reset()
        {
            counter = 0;

            active = false;
        }

        public override void OnApply()
        {
        }
    }
}