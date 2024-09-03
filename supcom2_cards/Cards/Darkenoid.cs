using Supcom2Cards.MonoBehaviours;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class Darkenoid : CustomCard
    {
        public const int BEAM_COUNT = 5;
        public const float BEAM_GAP = 0.075f;
        public const float BEAM_WIDTH = 0.15f;
        public const float DPS_WIDTH = 1f;

        public const float DPS = 100f;
        public const float UPS = 20f;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            characterStats.movementSpeed *= 1.25f;

            player.IncrementCardEffect<DarkenoidEffect>();
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.DecrementCardEffect<DarkenoidEffect>();
        }

        protected override string GetTitle()
        {
            return "Darkenoid";
        }
        protected override string GetDescription()
        {
            return "Adds a downward shooting\n" +
                $"Laser that deals {DPS} DPS\n" +
                "(Laser doesn't damage teammates)\n" +
                "(Laser scales with gun DMG)";
        }
        protected override GameObject GetCardArt()
        {
            _ = Supcom2.CardArt.TryGetValue("Darkenoid", out GameObject cardArt);
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
                    stat = "Movement speed",
                    amount = "+25%",
                    simepleAmount = CardInfoStat.SimpleAmount.aHugeAmountOf
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
