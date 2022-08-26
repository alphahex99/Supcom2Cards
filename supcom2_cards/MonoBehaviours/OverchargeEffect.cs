using System;
using System.Linq;
using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class OverchargeEffect : CounterReversibleEffect
    {
        public int HowMany = 0;

        private bool active = false;
        private float counter = 0;

        private Action<BlockTrigger.BlockTriggerType>? blockAction;
        private readonly ObjectsToSpawn[] explosionToSpawn = new ObjectsToSpawn[1];

        public void Activate()
        {
            counter += Cards.Harden.HARDEN_SECONDS * HowMany;
        }

        public override CounterStatus UpdateCounter()
        {
            counter -= Time.deltaTime;
            if (!active && counter > 0)
            {
                active = true;
                return CounterStatus.Apply;
            }
            else if (counter <= 0)
            {
                Reset();
                return CounterStatus.Remove;
            }
            return CounterStatus.Wait;
        }

        public override void UpdateEffects()
        {
            // gun
            gunStatModifier.attackSpeed_mult = 0.25f;

            // projectile
            gunStatModifier.bulletDamageMultiplier_mult = 2f;
            gunStatModifier.projectileSize_add = 2.5f;

            // add explosion effect
            if (explosionToSpawn[0] == null)
            {
                // load explosion effect from Explosive Bullet card
                GameObject? explosiveBullet = (GameObject)Resources.Load("0 cards/Explosive bullet");
                GameObject A_ExplosionSpark = explosiveBullet.GetComponent<Gun>().objectsToSpawn[0].AddToProjectile;
                GameObject A_Explosion = explosiveBullet.GetComponent<Gun>().objectsToSpawn[0].effect;
                GameObject explosionOvercharge = Instantiate(A_Explosion);
                explosionOvercharge.transform.position = new Vector3(1000, 0, 0);
                explosionOvercharge.hideFlags = HideFlags.HideAndDontSave;
                explosionOvercharge.name = "explosionOvercharge";
                DestroyImmediate(explosionOvercharge.GetComponent<RemoveAfterSeconds>());

                explosionToSpawn[0] = new ObjectsToSpawn
                {
                    AddToProjectile = A_ExplosionSpark,
                    direction = ObjectsToSpawn.Direction.forward,
                    effect = explosionOvercharge,
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
        }

        public override void OnStart()
        {
            blockAction = GetBlockAction(player);
            block.BlockAction += blockAction;

            base.OnStart();
        }

        public override void OnOnDestroy()
        {
            if (blockAction != null)
            {
                block.BlockAction -= blockAction;
            }
        }

        private Action<BlockTrigger.BlockTriggerType> GetBlockAction(Player player)
        {
            return delegate (BlockTrigger.BlockTriggerType trigger)
            {
                if (trigger == BlockTrigger.BlockTriggerType.Default ||
                    trigger == BlockTrigger.BlockTriggerType.Echo ||
                    trigger == BlockTrigger.BlockTriggerType.ShieldCharge)
                {
                    Activate();
                }
            };
        }

        public override void Reset()
        {
            counter = 0;

            // remove explosion effect
            gun.objectsToSpawn = gun.objectsToSpawn.Except(explosionToSpawn).ToArray();

            active = false;
        }

        public override void OnApply()
        {
        }
    }
}
