using Supcom2Cards.MonoBehaviours;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class Cybranasaurus : CustomCard
    {
        public const float HP_DMG_MULT = 1f;

        public const float JUMP_MIN = 4f; // no damage
        public const float JUMP_MAX = 15f; // max damage

        public const float DISTANCE_MIN = 3f; // max damage
        public const float DISTANCE_MAX = 7.5f; // no damage

        // visuals
        public const int CHARGE_EDGES = 5;
        public const float CHARGE_SIZE = 3.5f;
        public const float CHARGE_RPM = 250f;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            block.cdAdd = 1.5f;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.IncrementCardEffect<CybranasaurusEffect>();
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.DecrementCardEffect<CybranasaurusEffect>();
        }

        protected override string GetTitle()
        {
            return "Cybranasaurus";
        }
        protected override string GetDescription()
        {
            return "Landing after a jump deals DMG " +
                "that scales with current HP and " +
                "jump height\n" +
                "(up to 100% of HP as DMG)";
        }
        protected override GameObject GetCardArt()
        {
            _ = Supcom2.CardArt.TryGetValue("Cybranasaurus", out GameObject cardArt);
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
                    stat = "Block cooldown",
                    amount = "+1.5s",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
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
