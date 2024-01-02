#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.RoundsEffects;
using UnityEngine;
using Supcom2Cards.Cards;
using System;

namespace Supcom2Cards.RoundsEffects
{
    public class CrahdowEffect : HitEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public Player Owner;

        public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer)
        {
            if (this == null)
            {
                return;
            }

            if (!damagedPlayer.data.isGrounded && !damagedPlayer.data.isWallGrab)
            {
                damagedPlayer.TakeDamage((float)(Math.Pow(Crahdow.DMG_BOOST, CardAmount) - 1d) * damage.magnitude);
            }
        }
    }
}
