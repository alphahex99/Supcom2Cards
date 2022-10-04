using System;
using System.Linq;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class Nuke : CustomCard
    {
        private readonly ObjectsToSpawn[] explosionToSpawn = new ObjectsToSpawn[2];

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been setup.");
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`

            cardInfo.allowMultiple = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been added to player {player.playerID}.");
            //Edits values on player when card is selected

            gun.projectileColor = Color.green;

            gun.damage *= 6f;

            gunAmmo.maxAmmo = Math.Max((int)(0.1f * gunAmmo.maxAmmo), 1);

            gun.attackSpeed *= 0.1f;

            gunAmmo.reloadTimeAdd += 5f;

            // add explosion effect
            if (explosionToSpawn[0] == null)
            {
                (GameObject AddToProjectile, GameObject effect, Explosion explosion) = Supcom2.LoadExplosion("explosionNuke", gun);

                explosion.force *= 0.1f;
                explosion.stun += 0.5f;

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
            // add toxic cloud effect
            if (explosionToSpawn[1] == null)
            {
                // load toxic cloud effect from Toxic cloud card
                GameObject? toxicCloud = (GameObject)Resources.Load("0 cards/Toxic cloud");
                GameObject A_ToxicCloud = toxicCloud.GetComponent<Gun>().objectsToSpawn[0].effect;

                explosionToSpawn[1] = new ObjectsToSpawn
                {
                    AddToProjectile = null,
                    direction = ObjectsToSpawn.Direction.forward,
                    effect = A_ToxicCloud,
                    normalOffset = 0.05f,
                    scaleFromDamage = 0.6f,
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

            gun.gravity = 0f;
            gun.projectileSpeed *= 0.5f;
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been removed from player {player.playerID}.");
            //Run when the card is removed from the player

            // remove explosion effect
            gun.objectsToSpawn = gun.objectsToSpawn.Except(explosionToSpawn).ToArray();
        }

        protected override string GetTitle()
        {
            return "Nuke";
        }
        protected override string GetDescription()
        {
            return "You know what it does";
        }
        protected override GameObject GetCardArt()
        {
            return Supcom2.C_Nuke;
        }
        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat()
                {
                    positive = true,
                    stat = "DMG",
                    amount = "+500%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "AMMO",
                    amount = "-90%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "ATKSPD",
                    amount = "10%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "Reload time",
                    amount = "+5s",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
        public override string GetModName()
        {
            return Supcom2.ModInitials;
        }
    }
}
