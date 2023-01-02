#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class FatboyEffect : CounterReversibleEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        private bool modifiersActive = false;
        private bool standingStill = false;

        private Vector3 lastPosition = new Vector3(0, 0, 0);

        public override CounterStatus UpdateCounter()
        {
            if (!Supcom2.PickPhase && !modifiersActive && !standingStill)
            {
                return CounterStatus.Apply;
            }
            else if (modifiersActive)
            {
                if (Supcom2.PickPhase || standingStill)
                {
                    return CounterStatus.Remove;
                }
            }
            return CounterStatus.Wait;
        }

        public override void UpdateEffects()
        {

        }

        public override void OnApply()
        {
            modifiersActive = true;

            gunAmmo.enabled = false;
            gun.enabled = false;
        }
        public override void OnRemove()
        {
            modifiersActive = false;

            gunAmmo.enabled = true;
            gun.enabled = true;
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

            standingStill = dx * dx + dy * dy < Fatboy.MAX_SPEED_POW;
            lastPosition = player.transform.position;
        }
    }
}
