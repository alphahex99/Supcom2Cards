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

        private readonly ObjectsToSpawn[] explosionToSpawn = new ObjectsToSpawn[1];

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
                Reset();
                return CounterStatus.Remove;
            }
            return CounterStatus.Wait;
        }

        public override void UpdateEffects()
        {
            // gun
            gunStatModifier.attackSpeed_mult = 0.5f;
            gunAmmo.ReloadAmmo(false);

            // projectile
            gunStatModifier.bulletDamageMultiplier_mult = 1.5f;
            gunStatModifier.projectileSize_add = 3f;
            gunStatModifier.knockback_mult = 2f;
            gunStatModifier.recoilMuiltiplier_mult = 2f;

            // add explosion effect
            if (explosionToSpawn[0] == null)
            {
                // load explosion effect from Explosive Bullet card
                GameObject? explosiveBullet = (GameObject)Resources.Load("0 cards/Explosive bullet");
                GameObject A_ExplosionSpark = explosiveBullet.GetComponent<Gun>().objectsToSpawn[0].AddToProjectile;
                GameObject A_Explosion = explosiveBullet.GetComponent<Gun>().objectsToSpawn[0].effect;
                Explosion explosion = A_Explosion.GetComponent<Explosion>();

                explosionToSpawn[0] = new ObjectsToSpawn
                {
                    AddToProjectile = A_ExplosionSpark,
                    direction = ObjectsToSpawn.Direction.forward,
                    effect = A_Explosion,
                    normalOffset = 0.1f,
                    scaleFromDamage = 0.5f,
                    scaleStackM = 0.7f,
                    scaleStacks = true,
                    spawnAsChild = false,
                    spawnOn = ObjectsToSpawn.SpawnOn.all,
                    stacks = 0,
                    stickToAllTargets = false,
                    stickToBigTargets = false,
                    zeroZ = false
                };
            }
            gun.objectsToSpawn = gun.objectsToSpawn.Concat(explosionToSpawn).ToArray();

            /* breaks if player is alive and overcharged when round ends
            // check current max ammo, increase to 5 if necessary while OC
            if (gunAmmo.maxAmmo < 5)
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
            
        }

        public override void Reset()
        {
            shotsLeft = 0;
            active = false;

            // remove explosion effect
            gun.objectsToSpawn = gun.objectsToSpawn.Except(explosionToSpawn).ToArray();
        }
    }
}