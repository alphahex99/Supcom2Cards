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

        private RemoveAfterSeconds? explosionRemove;

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
            //gunAmmoStatModifier.reloadTimeMultiplier_mult = 0.01f;

            // projectile
            gunStatModifier.bulletDamageMultiplier_mult = 1.5f;
            gunStatModifier.size_add = 1f;
            gunStatModifier.projectileSize_add = 3f;
            gunStatModifier.knockback_mult = 2f;
            gunStatModifier.recoilMuiltiplier_mult = 2f;

            // add explosion effect
            GameObject? explosiveBullet = (GameObject)Resources.Load("0 cards/Explosive bullet");
            GameObject explosion = Instantiate(explosiveBullet.GetComponent<Gun>().objectsToSpawn[0].effect);
            explosion.transform.position = new Vector3(1000, 0, 0);
            explosion.hideFlags = HideFlags.HideAndDontSave;
            explosionRemove = explosion.GetComponent<RemoveAfterSeconds>();
            explosionRemove.seconds = 1000000f;
            explosion.GetComponent<Explosion>().force = 100000;
            gun.objectsToSpawn = new[]
            {
                new ObjectsToSpawn
                {
                    effect = explosion,
                    normalOffset = 0.1f,
                    numberOfSpawns = 1,
                    scaleFromDamage = .5f,
                }
            };

            // check current max ammo, increase to 5 if necessary while OC
            if (gunAmmo.maxAmmo < 5)
            {
                int missing = 5 - gunAmmo.maxAmmo;
                gunAmmoStatModifier.maxAmmo_add = missing;
            }
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
            // remove explosion effect
            if (explosionRemove != null)
            {
                explosionRemove.seconds = 0f;
            }

            // TODO: fix bullets still exploding for a couple seconds after OC expired
        }

        public override void OnOnEnable()
        {
            shotsLeft = 0;
            active = false;
            base.OnOnEnable();
        }

        public override void OnOnDisable()
        {
            shotsLeft = 0;
            active = false;
            base.OnOnDisable();
        }
    }
}