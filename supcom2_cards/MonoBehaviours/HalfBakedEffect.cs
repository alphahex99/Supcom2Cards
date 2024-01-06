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
            if (input.shieldWasPressed && !input.silencedInput && CanBlock())
            {
                // I know this won't work on autoblock cards like ChromeShields
                // (in this mod I trigger it manually from ChromeShields),
                // but I haven't found a way to do this through OnBlock actions or
                // any other way I tested except checking player inputs and
                // cancelling the block before it happens as I do here
                _ = TryBlock(player);
            }
        }

        /// <summary>This can be called from autoblock cards to manually trigger</summary>
        /// <returns>False if Block failed</returns>
        public static bool TryBlock(Player player)
        {
            HalfBakedEffect effect = player.GetComponent<HalfBakedEffect>();
            if (effect == null)
            {
                // player doesn't have HalfBaked
                return true;
            }

            // roll the dice
            if (RNG.NextUInt() < effect.chance)
            {
                effect.FailBlock();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void FailBlock()
        {
            // block failed, apply effects
            player.data.silenceTime += HalfBaked.BLOCK_FAIL_SILENCE * CardAmount;

            // cancel block
            block.sinceBlock += 0.6f;

            // manually reset timer in case of autoblock cards
            block.counter = 0f;
        }

        private bool CanBlock()
        {
            // For the love of god DON'T touch this!
            // !block.IsOnCD() alone isn't enough because it's always on cooldown the same frame you right click
            // same problem with !block.blockedThisFrame
            // same problem with block.GetFieldValue("active")
            // can't (block.counter == 0f) because floats
            // This is the only solution I found that works after like 30 mins
            // TODO: Might break if you autoclicker RMB? Surely nobody would ever do that . . .
            return block.counter < 0.01f || !block.IsOnCD();
        }
    }
}
