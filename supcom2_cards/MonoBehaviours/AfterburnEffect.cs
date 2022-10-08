using UnityEngine;
using ModdingUtils.MonoBehaviours;

namespace Supcom2Cards.MonoBehaviours
{
    public class AfterburnEffect : CounterReversibleEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        private bool active = false;
        private float counter = 0;

        public void Activate()
        {
            counter += Cards.Afterburn.AB_SECONDS * CardAmount;
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
            characterStatModifiersModifier.movementSpeed_mult = 2f;

        }

        public override void OnStart()
        {
            block.BlockAction += OnBlock;

            base.OnStart();
        }

        public override void OnOnDestroy()
        {
            block.BlockAction -= OnBlock;
        }

        private void OnBlock(BlockTrigger.BlockTriggerType trigger)
        {
            if (trigger == BlockTrigger.BlockTriggerType.Default ||
                trigger == BlockTrigger.BlockTriggerType.Echo ||
                trigger == BlockTrigger.BlockTriggerType.ShieldCharge)
            {
                Activate();
            }
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