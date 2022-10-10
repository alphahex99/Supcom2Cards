#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class HunkerEffect : CounterReversibleEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        bool active = false;

        public override CounterStatus UpdateCounter()
        {
            if (!active && block.IsBlocking())
            {
                return CounterStatus.Apply;
            }
            else if (active)
            {
                return CounterStatus.Remove;
            }
            return CounterStatus.Wait;
        }

        public override void UpdateEffects()
        {
            characterStatModifiersModifier.movementSpeed_mult = 0f;
            characterStatModifiersModifier.jump_mult = 0f;
        }

        public override void OnApply()
        {
            active = true;
        }
        public override void OnRemove()
        {
            active = false;
        }

        public override void OnUpdate()
        {
            if (block.IsBlocking())
            {
                UnityEngine.Debug.Log(block.sinceBlock * Hunker.DURATION_MULT);
            }

            // reverse this frame's increment
            block.sinceBlock -= TimeHandler.deltaTime;

            // increment slower
            block.sinceBlock += TimeHandler.deltaTime / Hunker.DURATION_MULT / CardAmount;
        }

        public override void OnStart()
        {
            applyImmediately = false;
        }

        public override void Reset()
        {
        }
    }
}
