using Supcom2Cards.MonoBehaviours;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class HalfBaked : CustomCard
    {
        public const float BLOCK_FAIL_CHANCE = 0.25f;
        public const float BLOCK_FAIL_SILENCE = 2f;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            block.cdMultiplier = 0.3f;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.IncrementCardEffect<HalfBakedEffect>();
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.DecrementCardEffect<HalfBakedEffect>();
        }

        protected override string GetTitle()
        {
            return "Half Baked";
        }
        protected override string GetDescription()
        {
            return $"Your block will fail {BLOCK_FAIL_CHANCE*100}% of the time\nStacking only prolongs silence";
        }
        protected override GameObject GetCardArt()
        {
            _ = Supcom2.CardArt.TryGetValue("HalfBaked", out GameObject cardArt);
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
                    stat = "Block cooldown",
                    amount = "-70%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "Silence on fail",
                    amount = $"+{BLOCK_FAIL_SILENCE}s",
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
