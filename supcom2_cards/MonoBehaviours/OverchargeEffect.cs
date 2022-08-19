using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using ModdingUtils.MonoBehaviours;

namespace Supcom2Cards.MonoBehaviours
{
    public class OverchargeEffect : CounterReversibleEffect
    {
        public int shotsLeft = 0;
        public bool active = false;

        public override CounterStatus UpdateCounter()
        {
            if (!active && shotsLeft > 0)
            {
                active = true;
                return CounterStatus.Apply;
            }
            else if (shotsLeft <= 0)
            {
                active = false;
                return CounterStatus.Remove;
            }
            return CounterStatus.Wait;
        }

        public override void UpdateEffects()
        {
            // gun
            gunStatModifier.attackSpeed_mult = 0.5f;
            gunAmmoStatModifier.reloadTimeMultiplier_mult = 0.01f;

            // projectile
            gunStatModifier.bulletDamageMultiplier_mult = 2f;
            gunStatModifier.size_add = 1f;
            gunStatModifier.projectileSize_add = 3f;
            gunStatModifier.knockback_mult = 2f;
            gunStatModifier.recoilMuiltiplier_mult = 2f;

            /* doesnt work
            gunStatModifier.explodeNearEnemyRange_add = 10f;
            gunStatModifier.explodeNearEnemyDamage_add = gun.damage;
            */


            // check current max ammo, increase to 5 if necessary while OC
            /*if (gunAmmo.maxAmmo < 5)
            {
                int missing = 5 - gunAmmo.maxAmmo;
                gunAmmoStatModifier.maxAmmo_add = missing;
            }*/
        }

        public void OnShootProjectileAction(GameObject obj)
        {
            if (active)
            {
                shotsLeft--;
            }
        }

        public override void OnApply()
        {
            Reset();
        }

        public override void Reset()
        {
        }

        public override void OnOnEnable()
        {
            shotsLeft = 0;
            active = false;
            Reset();
            ClearModifiers();
            OnRemove();
        }

        public override void OnOnDisable()
        {
            shotsLeft = 0;
            active = false;
            Reset();
            ClearModifiers();
            OnRemove();
        }
    }
}