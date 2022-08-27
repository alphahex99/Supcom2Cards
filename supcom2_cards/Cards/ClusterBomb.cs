﻿using UnboundLib.Cards;
using UnityEngine;
using UnboundLib;
using Supcom2Cards.RoundsEffects;

namespace Supcom2Cards.Cards
{
    class ClusterBomb : CustomCard
    {
        private readonly int EXPLOSIONS = 15;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been setup.");
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`

            
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been added to player {player.playerID}.");
            //Edits values on player when card is selected

            gun.attackSpeed *= 2.5f;

            gun.damage *= 0.85f;

            ClusterBombEffect clusterBomb = player.gameObject.GetComponent<ClusterBombEffect>();
            if (clusterBomb == null)
            {
                clusterBomb = player.gameObject.AddComponent<ClusterBombEffect>();

                (GameObject AddToProjectile, GameObject effect, Explosion explosion) = Supcom2.LoadExplosion("explosionClusterBomb");

                explosion.damage = 15f;
                explosion.force *= 0.1f;
                explosion.range *= 1.5f;

                clusterBomb.Explosion = new ObjectsToSpawn
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
                clusterBomb.Explosions = EXPLOSIONS;
                clusterBomb.FramesBetweenExplosions = 5;
                clusterBomb.Spread = 6;

                // set this player as owner of the explosion
                effect.GetOrAddComponent<SpawnedAttack>().spawner = player;
            }
            clusterBomb.HowMany++;

        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been removed from player {player.playerID}.");
            //Run when the card is removed from the player

            ClusterBombEffect clusterBomb = player.gameObject.GetComponent<ClusterBombEffect>();
            clusterBomb.HowMany--;

            if (clusterBomb.HowMany < 1)
            {
                Destroy(clusterBomb);
            }
        }

        protected override string GetTitle()
        {
            return "Cluster Bomb";
        }
        protected override string GetDescription()
        {
            return $"Bullets create {EXPLOSIONS} (extra) tiny explosions after impact";
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
                    positive = false,
                    stat = "ATKSPD",
                    amount = "-150%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "DMG",
                    amount = "-15%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
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
