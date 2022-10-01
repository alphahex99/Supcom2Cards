using Supcom2Cards.MonoBehaviours;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class Veterancy : CustomCard
    {
        public const float ADD_MULT_PER_KILL = 0.2f;
        public const int MAX_KILLS = 5;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been setup.");
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`


        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been added to player {player.playerID}.");
            //Edits values on player when card is selected

            VeterancyEffect veterancy = player.gameObject.GetComponent<VeterancyEffect>();
            if (veterancy == null)
            {
                veterancy = player.gameObject.AddComponent<VeterancyEffect>();
                veterancy.SetLivesToEffect(int.MaxValue);
            }
            veterancy.HowMany++;
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been removed from player {player.playerID}.");
            //Run when the card is removed from the player

            VeterancyEffect veterancy = player.gameObject.GetComponent<VeterancyEffect>();
            veterancy.HowMany--;

            if (veterancy.HowMany < 1)
            {
                Destroy(veterancy);
            }
        }

        protected override string GetTitle()
        {
            return "Veterancy";
        }
        protected override string GetDescription()
        {
            return $"Every enemy kill permanently boosts your DMG/HP\n(+{MAX_KILLS * ADD_MULT_PER_KILL * 100}% max per card)";
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
                    stat = "DMG per kill",
                    amount = $"+{ADD_MULT_PER_KILL * 100}%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "HP per kill",
                    amount = $"+{ADD_MULT_PER_KILL * 100}%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.FirepowerYellow;
        }
        public override string GetModName()
        {
            return Supcom2.ModInitials;
        }
    }
}
