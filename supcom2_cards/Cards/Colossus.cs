using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class Colossus : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            data.maxHealth *= 3.5f;

            block.cdAdd += 2.5f;

            characterStats.movementSpeed *= 0.75f;
            characterStats.jump *= 0.9f;
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {

        }

        protected override string GetTitle()
        {
            return "Colossus";
        }
        protected override string GetDescription()
        {
            return null;
        }
        protected override GameObject GetCardArt()
        {
            _ = Supcom2.CardArt.TryGetValue("Colossus", out GameObject cardArt);
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
                    stat = "HP",
                    amount = "+250%",
                    simepleAmount = CardInfoStat.SimpleAmount.aHugeAmountOf
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "Block Cooldown",
                    amount = "+2.5s",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "Movement Speed",
                    amount = "-25%",
                    simepleAmount = CardInfoStat.SimpleAmount.aLotLower
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "Jump Height",
                    amount = "-10%",
                    simepleAmount = CardInfoStat.SimpleAmount.lower
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.PoisonGreen;
        }
        public override string GetModName()
        {
            return Supcom2.ModInitials;
        }
    }
}
