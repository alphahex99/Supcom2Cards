#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.RoundsEffects;
using UnityEngine;

namespace Supcom2Cards.RoundsEffects
{
    public class BuhbledowEffect : HitEffect
    {
        public Player Owner;

        public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer)
        {
            if (this != null && Owner.teamID != damagedPlayer.teamID)
            {
                Block block = damagedPlayer.data.block;

                // reset block cooldown
                block.counter = 0f;
            }
        }
    }
}
