﻿using System;
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
        private readonly ObjectsToSpawn[] explosionToSpawn = new ObjectsToSpawn[1];

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been setup.");
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`

            // add explosion effect
            if (explosionToSpawn[0] == null)
            {
                // load explosion effect from Explosive Bullet card
                GameObject? explosiveBullet = (GameObject)Resources.Load("0 cards/Explosive bullet");
                GameObject A_ExplosionSpark = explosiveBullet.GetComponent<Gun>().objectsToSpawn[0].AddToProjectile;
                GameObject A_Explosion = explosiveBullet.GetComponent<Gun>().objectsToSpawn[0].effect;
                GameObject explosionDisruptor = Instantiate(A_Explosion);
                explosionDisruptor.transform.position = new Vector3(1000, 0, 0);
                explosionDisruptor.hideFlags = HideFlags.HideAndDontSave;
                explosionDisruptor.name = "explosionDisruptor";
                DestroyImmediate(explosionDisruptor.GetComponent<RemoveAfterSeconds>());
                Explosion explosion = explosionDisruptor.GetComponent<Explosion>();

                explosion.silence += 1.5f;
                explosion.stun += 0.5f;

                explosionToSpawn[0] = new ObjectsToSpawn
                {
                    AddToProjectile = A_ExplosionSpark,
                    direction = ObjectsToSpawn.Direction.forward,
                    effect = explosionDisruptor,
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
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been added to player {player.playerID}.");
            //Edits values on player when card is selected

            gun.projectileColor = Color.white;

            gun.size += 0.5f;
            gun.projectileSize += 4f;
            gun.numberOfProjectiles += 3;
            gun.spread += 0.1f;

            gun.attackSpeed *= 5f;

            gun.damage *= 0.5f;
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
