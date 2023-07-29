using Supcom2Cards.MonoBehaviours;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class RadarJammer : CustomCard
    {
        public static readonly float BULLET_SPREAD = 15f / 180f; // degrees / 180f
        public static readonly float BULLET_SPEED_RAND = 0.1f;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {

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
            return "Enemy guns are less accurate while you're alive";
        }
        protected override GameObject GetCardArt()
        {
            _ = Supcom2.CardArt.TryGetValue("RadarJammer", out GameObject cardArt);
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
                    stat = "Enemy Spread",
                    amount = $"+{180f * BULLET_SPREAD}°",
                    simepleAmount = CardInfoStat.SimpleAmount.aLittleBitOf
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Enemy Bullet speed",
                    amount = $"Random (±{BULLET_SPEED_RAND*100}%)",
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
