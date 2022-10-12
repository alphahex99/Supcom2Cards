#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.MonoBehaviours;
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

        public override CounterStatus UpdateCounter()
        {
            if (!Supcom2.PickPhase && !modifiersActive && standingStill)
            {
                cooldownRatio = block.CooldownRatio();

                return CounterStatus.Apply;
            }
            else if (modifiersActive)
            {
                if (Supcom2.PickPhase || !standingStill)
                {
                    cooldownRatio = block.CooldownRatio();

                    return CounterStatus.Remove;
                }
            }
            return CounterStatus.Wait;
        }

        public override void UpdateEffects()
        {
            blockModifier.cdMultiplier_mult = DynamicPowerShunt.CD_MULT;
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
            //active = Vector3.Distance(player.transform.position, lastPosition) < 1f;

            float dx = player.transform.position.x - lastPosition.x;
            dx = dx > 0 ? dx : -dx;
            float dy = player.transform.position.y - lastPosition.y;
            dy = dy > 0 ? dy : -dy;

            standingStill = dx * dx + dy * dy < DynamicPowerShunt.MAX_SPEED_POW;
            lastPosition = player.transform.position;
        }
    }
}
