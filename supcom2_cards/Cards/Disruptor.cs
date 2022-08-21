using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class Disruptor : CustomCard
    {
        private readonly ObjectsToSpawn[] objectsToSpawn = new ObjectsToSpawn[2];

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been setup.");
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`


        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been added to player {player.playerID}.");
            //Edits values on player when card is selected

            gun.damage *= 3f;

            gun.attackSpeed *= 4f;

            statModifiers.health *= 1.5f;
            statModifiers.movementSpeed *= 0.75f;

            // add explosion effect
            if (objectsToSpawn[0] == null)
            {
                // load explosion effect from Explosive Bullet card
                GameObject? explosiveBullet = (GameObject)Resources.Load("0 cards/Explosive bullet");
                GameObject A_ExplosionSpark = explosiveBullet.GetComponent<Gun>().objectsToSpawn[0].AddToProjectile;
                GameObject A_Explosion = explosiveBullet.GetComponent<Gun>().objectsToSpawn[0].effect;
                Explosion explosion = A_Explosion.GetComponent<Explosion>();

                explosion.damage = 0f;
                explosion.force = 0f;
                explosion.range = 5f;

                objectsToSpawn[0] = new ObjectsToSpawn
                {
                    AddToProjectile = A_ExplosionSpark,
                    direction = ObjectsToSpawn.Direction.forward,
                    effect = A_Explosion,
                    normalOffset = 0.1f,
                    scaleFromDamage = 0f,
                    scaleStackM = 0f,
                    scaleStacks = false,
                    spawnAsChild = false,
                    spawnOn = ObjectsToSpawn.SpawnOn.all,
                    stacks = 0,
                    stickToAllTargets = false,
                    stickToBigTargets = false,
                    zeroZ = false
                };
            }

            // add stun effect
            if (objectsToSpawn[1] == null)
            {
                // load explosion effect from Explosive Bullet card
                GameObject? dazzle = (GameObject)Resources.Load("0 cards/Dazzle");
                GameObject E_StunOverTime = dazzle.GetComponent<Gun>().objectsToSpawn[0].effect;

                objectsToSpawn[1] = new ObjectsToSpawn
                {
                    AddToProjectile = null,
                    direction = ObjectsToSpawn.Direction.forward,
                    effect = E_StunOverTime,
                    normalOffset = 0f,
                    scaleFromDamage = 0.5f,
                    scaleStackM = 1f,
                    scaleStacks = true,
                    spawnAsChild = true,
                    spawnOn = ObjectsToSpawn.SpawnOn.all,
                    stacks = 0,
                    stickToAllTargets = false,
                    stickToBigTargets = false,
                    zeroZ = false
                };
            }

            gun.objectsToSpawn = gun.objectsToSpawn.Concat(objectsToSpawn).ToArray();
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been removed from player {player.playerID}.");
            //Run when the card is removed from the player

            gun.objectsToSpawn = gun.objectsToSpawn.Except(objectsToSpawn).ToArray();
        }

        protected override string GetTitle()
        {
            return "Disruptor";
        }
        protected override string GetDescription()
        {
            return "Bullets cause explosions that don't do damage but stun enemies.";
        }
        protected override GameObject GetCardArt()
        {
            return null;
        }
        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Common;
        }
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Bullets",
                    amount = "+3",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DefensiveBlue;
        }
        public override string GetModName()
        {
            return Supcom2.ModInitials;
        }
    }
}
