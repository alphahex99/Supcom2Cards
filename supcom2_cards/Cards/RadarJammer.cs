using Supcom2Cards.RoundsEffects;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class RadarJammer : CustomCard
    {
        public const float DURATION = 4f;
        public static readonly float BULLET_SPREAD = 30f / 180f; // degrees / 180f

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            gun.reloadTimeAdd = 0.25f;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.IncrementCardEffect<RadarJammerEffect>();
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.DecrementCardEffect<RadarJammerEffect>();
        }

        protected override string GetTitle()
        {
            return "Radar Jammer";
        }
        protected override string GetDescription()
        {
            return $"Damaging players worsens their aim for {DURATION} (extra) seconds";
        }
        protected override GameObject GetCardArt()
        {
            _ = Supcom2.CardArt.TryGetValue("RadarJammer", out GameObject cardArt);
            return cardArt;
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
                    stat = "Enemy Spread",
                    amount = $"+{180f * BULLET_SPREAD}°",
                    simepleAmount = CardInfoStat.SimpleAmount.aLittleBitOf
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "Reload time",
                    amount = "0.25s",
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
