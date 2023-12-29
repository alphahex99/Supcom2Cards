using System.Linq;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class NanobotWeapon : CustomCard
    {
        private readonly ObjectsToSpawn[] explosionToSpawn = new ObjectsToSpawn[1];

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gunAmmo.maxAmmo *= 2;

            gun.attackSpeed *= 0.75f;

            gun.damage *= 0.5f;

            // add toxic cloud effect
            if (explosionToSpawn[0] == null)
            {
                // load toxic cloud effect from Toxic cloud card
                GameObject? toxicCloud = (GameObject)Resources.Load("0 cards/Toxic cloud");
                GameObject A_ToxicCloud = toxicCloud.GetComponent<Gun>().objectsToSpawn[0].effect;

                /*ParticleSystem ps = A_ToxicCloud.GetComponent<ParticleSystem>();
                ps.startColor = Color.red;*/

                /*Explosion E_ToxicCloud = A_ToxicCloud.GetComponent<Explosion>();
                E_ToxicCloud.damage *= 0.25f;
                E_ToxicCloud.dmgColor = Color.yellow;
                E_ToxicCloud.range *= 2f;
                E_ToxicCloud.slow = 0f;*/

                /*RemoveAfterSeconds ras = A_ToxicCloud.GetComponent<RemoveAfterSeconds>();
                ras.seconds *= 100f;
                ras.SetFieldValue("shrinkSpeed", (float)ras.GetFieldValue("shrinkSpeed") * 100f);*/

                explosionToSpawn[0] = new ObjectsToSpawn
                {
                    AddToProjectile = null,
                    direction = ObjectsToSpawn.Direction.forward,
                    effect = A_ToxicCloud,
                    normalOffset = 0.05f,
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
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {

        }

        protected override string GetTitle()
        {
            return "Nanobot Weapon";
        }
        protected override string GetDescription()
        {
            return "Bullets spawn a big cloud of nanobots.\nClouds deal tiny damage.";
        }
        protected override GameObject GetCardArt()
        {
            _ = Supcom2.CardArt.TryGetValue("NanobotWeapon", out GameObject cardArt);
            return cardArt;
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
                    stat = "AMMO",
                    amount = "x2",
                    simepleAmount = CardInfoStat.SimpleAmount.aLotLower
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "ATKSPD",
                    amount = "+50%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "DMG",
                    amount = "-50%",
                    simepleAmount = CardInfoStat.SimpleAmount.aHugeAmountOf
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DestructiveRed;
        }
        public override string GetModName()
        {
            return Supcom2.ModInitials;
        }
    }
}
