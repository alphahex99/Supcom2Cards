using Supcom2Cards.RoundsEffects;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class Recycler : CustomCard
    {
        public static readonly int AMMO_STEAL = 2;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            statModifiers.lifeSteal = 0.5f;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            RecyclerEffect recycler = player.IncrementCardEffect<RecyclerEffect>();
            recycler.Owner = player;
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.DecrementCardEffect<RecyclerEffect>();
        }

        protected override string GetTitle()
        {
            return "Recycler";
        }
        protected override string GetDescription()
        {
            return $"Every hit steals enemy bullets";
        }
        protected override GameObject GetCardArt()
        {
            _ = Supcom2.CardArt.TryGetValue("Recycler", out GameObject cardArt);
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
                    stat = "AMMO steal",
                    amount = $"+{AMMO_STEAL}",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Life steal",
                    amount = "+50%",
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
