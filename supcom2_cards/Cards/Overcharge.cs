using UnboundLib.Cards;
using UnityEngine;
using Supcom2Cards.MonoBehaviours;

namespace Supcom2Cards.Cards
{
    class Overcharge : CustomCard
    {
        public const float DURATION = 2.5f;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            block.cdAdd = 0.75f;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.IncrementCardEffect<OverchargeEffect>();
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.DecrementCardEffect<OverchargeEffect>();
        }

        protected override string GetTitle()
        {
            return "Overcharge";
        }
        protected override string GetDescription()
        {
            return $"Blocking doubles DMG and ATKSPD for {DURATION} (extra) seconds - bullets also explode";
        }
        protected override GameObject GetCardArt()
        {
            _ = Supcom2.CardArt.TryGetValue("Overcharge", out GameObject cardArt);
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
                    stat = "DMG if active",
                    amount = "+100%",
                    simepleAmount = CardInfoStat.SimpleAmount.aHugeAmountOf
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "ATKSPD if active",
                    amount = "+100%",
                    simepleAmount = CardInfoStat.SimpleAmount.aHugeAmountOf
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "Block cooldown",
                    amount = "+0.75s",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.EvilPurple;
        }
        public override string GetModName()
        {
            return Supcom2.ModInitials;
        }
    }
}
