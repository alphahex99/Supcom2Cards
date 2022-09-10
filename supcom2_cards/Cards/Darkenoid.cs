using Supcom2Cards.MonoBehaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class Darkenoid : CustomCard
    {
        public static float DAMAGE_BUFF = 19f;
        public static float DAMAGE_DEBUFF = 0.15f;

        public static float DEGREES = 60f;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been setup.");
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`


        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been added to player {player.playerID}.");
            //Edits values on player when card is selected

            DarkenoidEffect darkenoid = player.gameObject.GetComponent<DarkenoidEffect>();
            if (darkenoid == null)
            {
                darkenoid = player.gameObject.AddComponent<DarkenoidEffect>();
                darkenoid.SetLivesToEffect(int.MaxValue);
            }
            darkenoid.HowMany++;
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been removed from player {player.playerID}.");
            //Run when the card is removed from the player

            DarkenoidEffect darkenoid = player.gameObject.GetComponent<DarkenoidEffect>();
            darkenoid.HowMany--;

            if (darkenoid.HowMany < 1)
            {
                Destroy(darkenoid);
            }
        }

        protected override string GetTitle()
        {
            return "Darkenoid";
        }
        protected override string GetDescription()
        {
            return $"Bullets deal more DMG if you're firing down ({DEGREES}° ark)\nBullets deal less DMG if you're not\nDirect damage only, explosion size not affected";
        }
        protected override GameObject GetCardArt()
        {
            return null;
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
                    stat = $"DMG if firing down",
                    amount = $"+{(1 + DAMAGE_BUFF) * 100}%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = $"DMG if not",
                    amount = $"-{(1 - DAMAGE_DEBUFF) * 100}%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
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
