#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;

namespace Supcom2Cards.MonoBehaviours
{
    public class HunkerEffect : CounterReversibleEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        bool modifiersActive = false;

        public override void OnUpdate()
        {
            // DO. NOT. DO. THIS.
            // Seriously, if more than 1 card does it this way it will break the game in all sorts of ways
            // This is temporary until I find out a better way to do it xd
            #region BAD
                // reverse this frame's increment
                block.sinceBlock -= TimeHandler.deltaTime;

                // increment slower
                block.sinceBlock += TimeHandler.deltaTime / Hunker.DURATION_MULT / CardAmount;
            #endregion

            // necessary for methods like UpdateCounter() to work
            base.OnUpdate();
        }

        public override CounterStatus UpdateCounter()
        {
            if (!modifiersActive && block.IsBlocking())
            {
                return CounterStatus.Apply;
            }
            else if (!block.IsBlocking())
            {
                Reset();
                return CounterStatus.Remove;
            }
            return CounterStatus.Wait;
        }

        public override void UpdateEffects()
        {
            characterStatModifiersModifier.movementSpeed_mult = Hunker.MOVESPEED_MULT;
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
            applyImmediately = false;
        }
    }
}
