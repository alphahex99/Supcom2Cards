using UnityEngine;
using ModdingUtils.MonoBehaviours;

namespace Supcom2Cards.MonoBehaviours
{
    public class HardenEffect : CounterReversibleEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        private float counter = 0;
        private bool modifiersActive = false;

        public void Activate()
        {
            counter += Cards.Harden.HARDEN_SECONDS * CardAmount;
        }

        public override CounterStatus UpdateCounter()
        {
            counter -= TimeHandler.deltaTime;
            if (!modifiersActive && counter > 0)
            {
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
            gunStatModifier.projectileSpeed_mult = 2f;
        }

        public override void OnApply()
        {
            modifiersActive = true;
        }
        public override void OnRemove()
        {
            modifiersActive = false;
        }
        public override void Reset()
        {
            counter = 0;
            modifiersActive = false;
        }

        public override void OnStart()
        {
            applyImmediately = false;
            SetLivesToEffect(int.MaxValue);

            block.BlockAction += OnBlock;
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
    }
}