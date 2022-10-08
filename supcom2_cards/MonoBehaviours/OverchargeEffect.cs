using System.Linq;
using ModdingUtils.MonoBehaviours;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class OverchargeEffect : CounterReversibleEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        private bool active = false;
        private float counter = 0;

        private readonly ObjectsToSpawn[] explosionToSpawn = new ObjectsToSpawn[1];

        public void Activate()
        {
            counter += Cards.Overcharge.OC_SECONDS * CardAmount;
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
                (GameObject AddToProjectile, GameObject effect, Explosion explosion) = Supcom2.LoadExplosion("explosionOvercharge");

                explosionToSpawn[0] = new ObjectsToSpawn
                {
                    AddToProjectile = AddToProjectile,
                    direction = ObjectsToSpawn.Direction.forward,
                    effect = effect,
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
            block.BlockAction += OnBlock;

            base.OnStart();
        }

        public override void OnOnDestroy()
        {
            block.BlockAction -= OnBlock;
        }

        private void OnBlock(BlockTrigger.BlockTriggerType trigger)
        {
            if (trigger == BlockTrigger.BlockTriggerType.Default ||
                trigger == BlockTrigger.BlockTriggerType.Echo ||
                trigger == BlockTrigger.BlockTriggerType.ShieldCharge)
            {
                Activate();
            }
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
