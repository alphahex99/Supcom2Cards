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
        private readonly ObjectsToSpawn[] explosionToSpawn = new ObjectsToSpawn[2];

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been setup.");
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`


        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been added to player {player.playerID}.");
            //Edits values on player when card is selected

            gun.projectileColor = Color.white;

            gun.size += 0.5f;
            gun.projectileSize += 3f;
            gun.numberOfProjectiles += 3;
            gun.spread += 0.05f;

            gun.attackSpeed *= 4f;

            gun.damage *= 0.2f;


            // add explosion effect
            if (explosionToSpawn[0] == null)
            {
                // load explosion effect from Explosive Bullet card
                GameObject? explosiveBullet = (GameObject)Resources.Load("0 cards/Explosive bullet");
                GameObject A_ExplosionSpark = explosiveBullet.GetComponent<Gun>().objectsToSpawn[0].AddToProjectile;
                GameObject A_Explosion = explosiveBullet.GetComponent<Gun>().objectsToSpawn[0].effect;
                Explosion explosion = A_Explosion.GetComponent<Explosion>();

                // load blue bullets from Cold Bullets card
                GameObject? coldBullets = (GameObject)Resources.Load("0 cards/Cold bullets");
                GameObject E_Cold = coldBullets.GetComponent<Gun>().objectsToSpawn[0].AddToProjectile;

                explosion.silence += 1.5f;
                explosion.stun += 0.5f;
                explosion.dmgColor = Color.cyan;

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
                explosionToSpawn[1] = new ObjectsToSpawn
                {
                    AddToProjectile = E_Cold, // TOOD: Cold visual effect isn't working
                    direction = ObjectsToSpawn.Direction.forward,
                    normalOffset = 0f,
                    removeScriptsFromProjectileObject = false,
                    scaleFromDamage = 0.5f,
                    scaleStackM = 0.3f,
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
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been removed from player {player.playerID}.");
            //Run when the card is removed from the player

            gun.objectsToSpawn = gun.objectsToSpawn.Except(explosionToSpawn).ToArray();
        }

        protected override string GetTitle()
        {
            return "Disruptor";
        }
        protected override string GetDescription()
        {
            return "Bullets explode and stun enemies.";
        }
        protected override GameObject GetCardArt()
        {
            return null;
        }
        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Uncommon;
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
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Silence",
                    amount = "+1.5s",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Stun",
                    amount = "+0.5s",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "ATKSPD",
                    amount = "Pathetic",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "DMG",
                    amount = "-80%",
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
