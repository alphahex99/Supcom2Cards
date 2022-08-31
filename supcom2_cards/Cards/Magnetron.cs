using Supcom2Cards.MonoBehaviours;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class Magnetron : CustomCard
    {
        public const float DPS = 150f;
        public const float HPS = 200f;
        public const float MG_SECONDS = 4f;

        public const float FORCE_PUSH = 20f;
        public const float FORCE_PULL = 25f;

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

            block.cdAdd += 1.5f;
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
            return $"Blocking pushes (aim gun up) or pulls (aim gun down) enemies for {MG_SECONDS} (extra) seconds\n\nContact with enemy while active does {DPS} (extra) DPS and heals {HPS} HP per second (healing doesn't stack, neither does push/pull force)";
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
                    positive = false,
                    stat = "Block Cooldown",
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
