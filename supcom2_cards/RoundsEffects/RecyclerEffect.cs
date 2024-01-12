#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.RoundsEffects;
using UnityEngine;
using Supcom2Cards.Cards;
using System.Reflection;

namespace Supcom2Cards.RoundsEffects
{
    public class RecyclerEffect : HitEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public Player Owner;

        public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer)
        {
            if (this == null || selfDamage)
            {
                return;
            }

            if (damagedPlayer.Gun().GunAmmo().CurrentAmmo() > 0)
            {
                // steal as many enemy bullets as possible
                int stolen = -damagedPlayer.Gun().GunAmmo().CurrentAmmoAdd(-Recycler.AMMO_STEAL * CardAmount);
                damagedPlayer.Gun().UpdateAmmo();

                // give stolen ammo to owner
                Owner.Gun().GunAmmo().CurrentAmmoAdd(stolen);
                Owner.Gun().UpdateAmmo();
            }
        }
    }
}
