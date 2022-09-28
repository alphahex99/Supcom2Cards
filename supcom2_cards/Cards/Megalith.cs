using Supcom2Cards.MonoBehaviours;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class Megalith : CustomCard
    {
        public const float UPS = 10; // updates per second

        public const float DPS_MULT = 0.25f;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been setup.");
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`


        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been added to player {player.playerID}.");
            //Edits values on player when card is selected

            MegalithEffect megalith = player.gameObject.GetComponent<MegalithEffect>();
            if (megalith == null)
            {
                megalith = player.gameObject.AddComponent<MegalithEffect>();
                megalith.SetLivesToEffect(int.MaxValue);
            }
            megalith.HowMany++;
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been removed from player {player.playerID}.");
            //Run when the card is removed from the player

            MegalithEffect megalith = player.gameObject.GetComponent<MegalithEffect>();
            megalith.HowMany--;

            if (megalith.HowMany < 1)
            {
                Destroy(megalith);
            }
        }

        protected override string GetTitle()
        {
            return "Megalith";
        }
        protected override string GetDescription()
        {
            return $"Acquire 2 (extra) lasers automatically firing at nearest visible enemies\n\nEach laser does {DPS_MULT*100}% DPS of your gun and prioritizes enemies that aren't being fired at by another laser";
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
