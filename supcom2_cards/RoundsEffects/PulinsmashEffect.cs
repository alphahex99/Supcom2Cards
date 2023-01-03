#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.RoundsEffects;
using UnityEngine;
using Supcom2Cards.Cards;
using System;
using UnboundLib;

namespace Supcom2Cards.RoundsEffects
{
    public class PulinsmashEffect : HitEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public Player Owner;

        public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer)
        {
            if (this == null)
            {
                return;
            }

            if (!Owner.data.dead)
            {
                Vector2 dir = Owner.transform.position - damagedPlayer.transform.position;

                damagedPlayer.data.healthHandler.TakeForce(10 * damage.magnitude * dir);

                dir *= 0.5f;

                damagedPlayer.transform.AddXPosition(dir.x);
                damagedPlayer.transform.AddYPosition(dir.y);
            }
        }
    }
}
