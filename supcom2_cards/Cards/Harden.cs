using UnboundLib.Cards;
using UnityEngine;
using Supcom2Cards.MonoBehaviours;

namespace Supcom2Cards.Cards
{
    class Harden : CustomCard
    {
        public const float HARDEN_SECONDS = 2.5f;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been setup.");
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`


        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been added to player {player.playerID}.");
            //Edits values on player when card is selected

            HardenEffect harden = player.gameObject.GetComponent<HardenEffect>();
            if (harden == null)
            {
                harden = player.gameObject.AddComponent<HardenEffect>();
                harden.SetLivesToEffect(int.MaxValue);
            }
            harden.HowMany++;

            block.cdAdd += 0.5f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been removed from player {player.playerID}.");
            //Run when the card is removed from the player

            HardenEffect harden = player.gameObject.GetComponent<HardenEffect>();
            harden.HowMany--;

            if (harden.HowMany < 1)
            {
                Destroy(harden);
            }
        }

        protected override string GetTitle()
        {
            return "Harden";
        }
        protected override string GetDescription()
        {
            return $"Blocking doubles your ATKSPD and Bullet speed for {HARDEN_SECONDS} (extra) seconds";
        }
        protected override GameObject GetCardArt()
        {
            return null;
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
                    stat = "ATKSPD if active",
                    amount = "100%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Bullet speed if active",
                    amount = "100%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "Block Cooldown",
                    amount = "+0.5s",
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
