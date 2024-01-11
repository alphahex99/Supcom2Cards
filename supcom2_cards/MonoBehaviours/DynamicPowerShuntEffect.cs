#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.MonoBehaviours;
using Sonigon;
using Supcom2Cards.Cards;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class DynamicPowerShuntEffect : CounterReversibleEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        // block.counter needs to be readjusted for new cooldown length after applying/removing effect
        private float cooldownRatio = 0;

        private bool modifiersActive = false;
        private bool standingStill = false;

        private Vector3 lastPosition = new Vector3(0, 0, 0);
        private float delay = DynamicPowerShunt.STAND_DELAY;

        public override CounterStatus UpdateCounter()
        {
            if (!Supcom2.PickPhase && !modifiersActive && standingStill)
            {
                delay -= Time.deltaTime;
                if (delay < 0)
                {
                    cooldownRatio = block.CooldownRatio();

                    return CounterStatus.Apply;
                }
            }
            else if (modifiersActive)
            {
                if (Supcom2.PickPhase || !standingStill)
                {
                    cooldownRatio = block.CooldownRatio();

                    delay = DynamicPowerShunt.STAND_DELAY;
                    return CounterStatus.Remove;
                }
            }
            return CounterStatus.Wait;
        }

        public override void UpdateEffects()
        {
            blockModifier.cdMultiplier_mult = DynamicPowerShunt.CD_MULT_STILL;
        }

        public override void OnApply()
        {
            modifiersActive = true;

            block.counter = block.Cooldown() * cooldownRatio;
        }
        public override void OnRemove()
        {
            modifiersActive = false;

            block.counter = block.Cooldown() * cooldownRatio;
        }
        public override void Reset()
        {
            modifiersActive = false;
        }

        public override void OnStart()
        {
        }

        public override void OnFixedUpdate()
        {
            standingStill = player.StandingStill(ref lastPosition);
        }
    }
}
