using System.Linq;
using ModdingUtils.MonoBehaviours;
using Sonigon;
using Supcom2Cards.Cards;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class OverchargeEffect : CounterReversibleEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public float counterValue = 0f;

        private readonly ObjectsToSpawn?[] explosionToSpawn = new ObjectsToSpawn[1];

        public override CounterStatus UpdateCounter()
        {
            counterValue -= TimeHandler.deltaTime;
            if (counterValue > 0f)
            {
                return CounterStatus.Apply;
            }
            counterValue = 0f;
            Reset();
            return CounterStatus.Remove;
        }

        public override void UpdateEffects()
        {
            gunStatModifier.damage_mult = 2f;
            gunStatModifier.attackSpeed_mult = 0.25f;
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
                gun.objectsToSpawn = gun.objectsToSpawn.Concat(explosionToSpawn).ToArray();
            }
        }

        public override void OnApply()
        {

        }

        public override void Reset()
        {
            if (explosionToSpawn[0] != null)
            {
                gun.objectsToSpawn = gun.objectsToSpawn.Except(explosionToSpawn).ToArray();
                explosionToSpawn[0] = null;
            }
        }

        public override void OnStart()
        {
            block.BlockAction += OnBlock;
            PlayerManager.instance.AddPlayerDiedAction(PlayerDied);
        }

        public override void OnOnDestroy()
        {
            block.BlockAction -= OnBlock;
            PlayerManager.instance.RemovePlayerDiedAction(PlayerDied);
        }

        private void OnBlock(BlockTrigger.BlockTriggerType trigger)
        {
            if (trigger == BlockTrigger.BlockTriggerType.Default ||
                trigger == BlockTrigger.BlockTriggerType.Echo ||
                trigger == BlockTrigger.BlockTriggerType.ShieldCharge)
            {
                counterValue += Overcharge.DURATION * CardAmount;

                SoundManager.Instance.Play(player.data.block.soundBlockStatusEffect, block.transform);
            }
        }

        private void PlayerDied(Player p, int idk)
        {
            if (p == player)
            {
                counterValue = 0f;
                ClearModifiers();
                Reset();
            }
        }
    }
}
