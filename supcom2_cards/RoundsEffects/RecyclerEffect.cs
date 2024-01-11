#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.RoundsEffects;
using UnityEngine;
using Supcom2Cards.Cards;
using System;
using UnboundLib;

namespace Supcom2Cards.RoundsEffects
{
    public class RecyclerEffect : HitEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public Player Owner;

        public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer)
        {
            if (this == null)
            {
                return;
            }

            Gun gun = Owner.data.weaponHandler.gun;
            GunAmmo gunAmmo = (GunAmmo)gun.GetFieldValue("gunAmmo");
            GunAmmo damagedGunAmmo = (GunAmmo)damagedPlayer.data.weaponHandler.gun.GetFieldValue("gunAmmo");
            
            int damagedCurrentAmmo = (int)damagedGunAmmo.GetFieldValue("currentAmmo");
            if (damagedCurrentAmmo > 0)
            {
                // steal as many enemy bullets as possible
                int stolenAmmo = Recycler.AMMO_STEAL * CardAmount;
                damagedCurrentAmmo -= Recycler.AMMO_STEAL * CardAmount;
                if (damagedCurrentAmmo < 0)
                {
                    stolenAmmo += damagedCurrentAmmo;
                    damagedCurrentAmmo = 0;
                }
                damagedGunAmmo.SetFieldValue("currentAmmo", damagedCurrentAmmo);

                // give stolen ammo to owner
                int currentAmmo = (int)gunAmmo.GetFieldValue("currentAmmo");
                gunAmmo.SetFieldValue("currentAmmo", currentAmmo + stolenAmmo);
            }
        }
    }
}
