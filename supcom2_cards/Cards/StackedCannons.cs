using Supcom2Cards.MonoBehaviours;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class StackedCannons : CustomCard
    {
        public static readonly float BULLET_SPREAD = 10f / 180f; // degrees / 180f

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            gun.spread = BULLET_SPREAD;
            gun.numberOfProjectiles = 6;
            gun.ammo = 6;
            gun.damage = 0.5f;
            gun.reloadTimeAdd = 0.25f;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {

        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {

        }

        protected override string GetTitle()
        {
            return "Stacked Cannons";
        }
        protected override string GetDescription()
        {
            return "Minimal bullet spread increase";
        }
        protected override GameObject GetCardArt()
        {
            _ = Supcom2.CardArt.TryGetValue("StackedCannons", out GameObject cardArt);
            return cardArt;
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
                    stat = "Bullets",
                    amount = "+6",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "AMMO",
                    amount = "+6",
                    simepleAmount = CardInfoStat.SimpleAmount.aHugeAmountOf
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "DMG",
                    amount = "-50%",
                    simepleAmount = CardInfoStat.SimpleAmount.aLotLower
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "Reload time",
                    amount = "+0.25s",
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
