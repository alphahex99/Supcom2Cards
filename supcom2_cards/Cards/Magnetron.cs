using Supcom2Cards.MonoBehaviours;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class Magnetron : CustomCard
    {
        public const float DPS = 150f;
        public const float HPS = 200f;
        public const float MG_SECONDS = 3f;

        public const float FORCE_PUSH = 15f;
        public const float FORCE_PULL = 20f;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been setup.");
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`


        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been added to player {player.playerID}.");
            //Edits values on player when card is selected

            MagnetronEffect magnetron = player.gameObject.GetComponent<MagnetronEffect>();
            if (magnetron == null)
            {
                magnetron = player.gameObject.AddComponent<MagnetronEffect>();
                magnetron.SetLivesToEffect(int.MaxValue);
            }
            magnetron.HowMany++;

            block.cdAdd += 2f;
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been removed from player {player.playerID}.");
            //Run when the card is removed from the player

            MagnetronEffect magnetron = player.gameObject.GetComponent<MagnetronEffect>();
            magnetron.HowMany--;

            if (magnetron.HowMany < 1)
            {
                Destroy(magnetron);
            }
        }

        protected override string GetTitle()
        {
            return "Magnetron";
        }
        protected override string GetDescription()
        {
            return $"Blocking pushes (aim up) or pulls (aim down) enemies for {MG_SECONDS} (extra) seconds";
        }
        protected override GameObject GetCardArt()
        {
            return Supcom2.C_Magnetron;
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
                    positive = false,
                    stat = "Block Cooldown",
                    amount = "+2.0s",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "touch DPS if active",
                    amount = $"+{DPS}",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "touch HPS if active",
                    amount = $"{HPS}",
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
