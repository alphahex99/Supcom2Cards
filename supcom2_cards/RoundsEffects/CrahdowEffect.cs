#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.RoundsEffects;
using UnityEngine;
using Supcom2Cards.Cards;
using System;
using Supcom2Cards.MonoBehaviours;

namespace Supcom2Cards.RoundsEffects
{
    public class CrahdowEffect : HitEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public Player player;

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
        }

        public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer)
        {
            if (!player.data.view.IsMine)
            {
                return;
            }

            // fix Chrome Shield ignore
            //TODO: rewrite Chrome Shield & Hunker, remove this
            float blockDuration = 0.3f;
            HunkerEffect hunker = damagedPlayer.gameObject.GetComponent<HunkerEffect>();
            if (hunker != null)
            {
                blockDuration *= Hunker.DURATION_MULT;
            }
            Block block = damagedPlayer.data.block;
            if (block.IsBlocking() || block.sinceBlock < blockDuration)
            {
                return;
            }

            if (!damagedPlayer.data.isGrounded && !damagedPlayer.data.isWallGrab)
            {
                damagedPlayer.data.healthHandler.CallTakeDamage(
                    Vector2.up * (float)(Math.Pow(Crahdow.DMG_BOOST, CardAmount) - 1d) * damage.magnitude,
                    damagedPlayer.data.transform.position,
                    damagingPlayer: player
                );
            }
        }
    }
}
