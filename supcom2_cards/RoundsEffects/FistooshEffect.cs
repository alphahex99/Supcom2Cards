#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.RoundsEffects;
using UnityEngine;
using Supcom2Cards.Cards;
using System;
using Supcom2Cards.MonoBehaviours;

namespace Supcom2Cards.RoundsEffects
{
    public class FistooshEffect : HitEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public Player Owner;

        public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer)
        {
            if (this == null)
            {
                return;
            }

            float blockDuration = 0.3f;

            HunkerEffect hunker = damagedPlayer.gameObject.GetComponent<HunkerEffect>();
            if (hunker != null)
            {
                blockDuration *= Hunker.DURATION_MULT;
            }

            // fix Chrome Shield ignore
            Block block = damagedPlayer.data.block;
            if (block.IsBlocking() || block.sinceBlock < blockDuration)
            {
                return;
            }

            if (damagedPlayer.data.isGrounded || damagedPlayer.data.isWallGrab)
            {
                damagedPlayer.TakeDamage((float)(Math.Pow(Fistoosh.DMG_BOOST, CardAmount) - 1d) * damage.magnitude);
            }
        }
    }
}
