#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.MonoBehaviours;
using Sonigon;
using Supcom2Cards.Cards;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    //TODO: does this fuck up pick phase stats?
    public class JackhammerEffect : CounterReversibleEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        private bool modifiersActive = false;
        private bool standingStill = false;

        private Vector3 lastPosition = new Vector3(0, 0, 0);
        private float delay = Jackhammer.STAND_DELAY;

        public override CounterStatus UpdateCounter()
        {
            if (!Supcom2.PickPhase && !modifiersActive && standingStill)
            {
                delay -= Time.deltaTime;
                if (delay < 0)
                {
                    SoundManager.Instance.Play(player.data.playerSounds.soundCharacterLandBig, player.transform);
                    SoundManager.Instance.Play(player.data.healthHandler.soundDamagePassive, player.transform);
                    return CounterStatus.Apply;
                }
            }
            else if (modifiersActive)
            {
                if (Supcom2.PickPhase || !standingStill)
                {
                    delay = Jackhammer.STAND_DELAY;
                    return CounterStatus.Remove;
                }
            }
            return CounterStatus.Wait;
        }

        public override void UpdateEffects()
        {
            gunStatModifier.damage_mult = 3f;
            characterDataModifier.maxHealth_mult = 3f;
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
