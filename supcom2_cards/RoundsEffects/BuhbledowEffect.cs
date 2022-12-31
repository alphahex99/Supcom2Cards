#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.RoundsEffects;
using UnityEngine;

namespace Supcom2Cards.RoundsEffects
{
    public class BuhbledowEffect : HitEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public Player Owner;

        public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer)
        {
            if (this == null)
            {
                return;
            }

            Block block = damagedPlayer.data.block;

            if (!block.IsBlocking() && block.IsOnCD())
            {
                block.counter *= 0.5f;
            }
        }
    }
}
