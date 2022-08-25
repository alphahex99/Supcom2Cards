using Supcom2Cards.MonoBehaviours;
using System;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class Magnetron : CustomCard
    {
        private const float FORCE_PUSH = 25f;
        private const float FORCE_PULL = 15f;
        private const float MG_SECONDS = 5f;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been setup.");
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`


        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been added to player {player.playerID}.");
            //Edits values on player when card is selected

            if (player.gameObject.GetComponent<MagnetronEffect>() == null)
            {
                player.gameObject.AddComponent<MagnetronEffect>().SetLivesToEffect(int.MaxValue);
            }
            MagnetronEffect magnetron = player.gameObject.GetComponent<MagnetronEffect>();
            magnetron.HowMany++;

            if (magnetron.HowMany <= 1)
            {
                block.BlockAction = (Action<BlockTrigger.BlockTriggerType>)Delegate.Combine(block.BlockAction, new Action<BlockTrigger.BlockTriggerType>(GetDoBlockAction(player)));
            }

            block.cdAdd = 2.5f;
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been removed from player {player.playerID}.");
            //Run when the card is removed from the player

            MagnetronEffect magnetron = player.gameObject.GetComponent<MagnetronEffect>();
            magnetron.HowMany--;

            if (magnetron.HowMany < 1)
            {
                block.BlockAction = (Action<BlockTrigger.BlockTriggerType>)Delegate.Remove(block.BlockAction, GetDoBlockAction(player));
                magnetron.Destroy();
            }
        }

        private Action<BlockTrigger.BlockTriggerType> GetDoBlockAction(Player player)
        {
            return delegate (BlockTrigger.BlockTriggerType trigger)
            {
                if (trigger != BlockTrigger.BlockTriggerType.None && trigger != BlockTrigger.BlockTriggerType.Empower)
                {
                    player.gameObject.GetComponent<MagnetronEffect>().Activate(player.data.aimDirection.y > 0 ? FORCE_PUSH : -FORCE_PULL, MG_SECONDS);
                }
            };
        }

        protected override string GetTitle()
        {
            return "Magnetron";
        }
        protected override string GetDescription()
        {
            return $"Blocking pushes or pulls enemies for {MG_SECONDS} (extra) seconds and steals HP from enemies you touch\n\nBefore blocking:\n• Aim gun UP to PUSH\n• Aim gun DOWN to PULL";
        }
        protected override GameObject GetCardArt()
        {
            return null;
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
                    stat = "Block Cooldown",
                    amount = "+2.5s",
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
