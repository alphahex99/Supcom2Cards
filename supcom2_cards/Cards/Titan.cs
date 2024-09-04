using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class Titan : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            statModifiers.movementSpeed = 1.5f;
            statModifiers.jump = 1.15f;
            statModifiers.health = 0.75f;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {

        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {

        }

        protected override string GetTitle()
        {
            return "Titan";
        }
        protected override string GetDescription()
        {
            return null;
        }
        protected override GameObject GetCardArt()
        {
            _ = Supcom2.CardArt.TryGetValue("Titan", out GameObject cardArt);
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
                    stat = "Movement speed",
                    amount = "+50%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Jump height",
                    amount = "+15%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "HP",
                    amount = "-25%",
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
