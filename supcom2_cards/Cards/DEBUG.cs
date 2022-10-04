using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    #region https://github.com/willuwontu/wills-wacky-cards/blob/main/Wills%20Wacky%20Cards/Cards/Testing/RemoveCards.cs
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
        public override string GetModName()
        {
            return Supcom2.ModInitials;
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
        public override string GetModName()
        {
            return Supcom2.ModInitials;
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
        public override string GetModName()
        {
            return Supcom2.ModInitials;
        }
    }
    #endregion https://github.com/willuwontu/wills-wacky-cards/blob/main/Wills%20Wacky%20Cards/Cards/Testing/RemoveCards.cs

    class FreezeBullets : CustomCard
    {
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been added to player {player.playerID}.");
            //Edits values on player when card is selected

            UnityEngine.Debug.Log($"grav = {gun.gravity}");
            UnityEngine.Debug.Log($"pseed = {gun.projectileSpeed}");

            gun.gravity = 0f;
            gun.projectileSpeed = 0f;
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been removed from player {player.playerID}.");
            //Run when the card is removed from the player

            gun.gravity = 1f;
            gun.projectileSpeed = 1f;
        }
        protected override string GetTitle()
        {
            return "Freeze Bullets";
        }
        protected override string GetDescription()
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
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat()
                {
                    positive = false,
                    stat = "Bullet speed",
                    amount = "-100%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
        public override string GetModName()
        {
            return Supcom2.ModInitials;
        }
    }
}