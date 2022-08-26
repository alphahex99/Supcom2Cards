using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

// from: https://github.com/willuwontu/wills-wacky-cards/blob/main/Wills%20Wacky%20Cards/Cards/Testing/RemoveCards.cs

#if DEBUG
namespace Supcom2Cards.Cards
{
    class RemoveFirst : CustomCard
    {
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Supcom2.instance.ExecuteAfterFrames(20,
                () => {
                    int[] indices = new int[2];

                    indices[0] = 0;
                    indices[1] = player.data.currentCards.Count - 1;

                    ModdingUtils.Utils.Cards.instance.RemoveCardsFromPlayer(player, indices);
                });
        }
        protected override string GetTitle()
        {
            return "Remove First";
        }
        protected override string GetDescription()
        {
            return "Removes the first card picked and this one.";
        }
        protected override CardInfoStat[] GetStats()
        {
            return null;
        }
        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }
        protected override GameObject GetCardArt()
        {
            return null;
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
    }
    class RemoveLast : CustomCard
    {
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Supcom2.instance.ExecuteAfterFrames(20,
                () => {
                    int[] indices = new int[2];

                    indices[0] = player.data.currentCards.Count - 2;
                    indices[1] = player.data.currentCards.Count - 1;

                    ModdingUtils.Utils.Cards.instance.RemoveCardsFromPlayer(player, indices);
                });
        }
        protected override string GetTitle()
        {
            return "Remove Last";
        }
        protected override string GetDescription()
        {
            return "Removes the last card picked and this one.";
        }
        protected override CardInfoStat[] GetStats()
        {
            return null;
        }
        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }
        protected override GameObject GetCardArt()
        {
            return null;
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
    }
    class RemoveAll : CustomCard
    {
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Supcom2.instance.ExecuteAfterFrames(20, () => ModdingUtils.Utils.Cards.instance.RemoveAllCardsFromPlayer(player, true));
        }
        protected override string GetTitle()
        {
            return "Remove All";
        }
        protected override string GetDescription()
        {
            return "Removes all cards on the player, including this one.";
        }
        protected override CardInfoStat[] GetStats()
        {
            return null;
        }
        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }
        protected override GameObject GetCardArt()
        {
            return null;
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
    }
}
#endif