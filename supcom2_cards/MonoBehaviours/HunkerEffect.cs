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
            // reverse this frame's increment
            block.sinceBlock -= TimeHandler.deltaTime;

            // increment slower
            block.sinceBlock += TimeHandler.deltaTime / Hunker.DURATION_MULT / CardAmount;

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
            characterStatModifiersModifier.movementSpeed_mult = 0.25f;
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
