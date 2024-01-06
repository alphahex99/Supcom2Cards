#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Supcom2Cards.Cards;
using UnboundLib;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class HalfBakedEffect : MonoBehaviour, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public Player player;
        public Block block;

        private uint chance = (uint)(uint.MaxValue * HalfBaked.BLOCK_FAIL_CHANCE);

        private GeneralInput input;

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            block = player.GetComponent<Block>();

            input = (GeneralInput)block.GetFieldValue("input");
        }

        public void Update()
        {
            if(input.shieldWasPressed && !input.silencedInput)
            {
                if (RNG.NextUInt() / CardAmount < chance)
                {
                    // block failed, apply effects
                    player.data.silenceTime += HalfBaked.BLOCK_FAIL_SILENCE;

                    // cancel block
                    block.sinceBlock += 0.6f;
                }
            }
        }
    }
}
