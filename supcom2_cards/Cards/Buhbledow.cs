using Supcom2Cards.RoundsEffects;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class Buhbledow : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage *= 0.75f;

            player.IncrementCardEffect<BuhbledowEffect>().Owner = player;
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.DecrementCardEffect<BuhbledowEffect>();
        }

        protected override string GetTitle()
        {
            return "Buhbledow";
        }
        protected override string GetDescription()
        {
            return "Dealing damage to somebody halfs the current recharge of their block meter unless full";
        }
        protected override GameObject GetCardArt()
        {
            _ = Supcom2.CardArt.TryGetValue("Buhbledow", out GameObject cardArt);
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
                    positive = false,
                    stat = "DMG",
                    amount = "-25%",
                    simepleAmount = CardInfoStat.SimpleAmount.slightlyLower
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
