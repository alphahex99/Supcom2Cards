using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.Networking;
using UnityEngine;
using System.Reflection;
using Supcom2Cards.MonoBehaviours;

namespace Supcom2Cards.Cards
{
    class Overcharge : CustomCard
    {
        private const int OC_SHOTS = 5;
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been setup.");
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`

            cardInfo.allowMultiple = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been added to player {player.playerID}.");
            //Edits values on player when card is selected

            OverchargeEffect OC = player.gameObject.AddComponent<OverchargeEffect>();
            gun.ShootPojectileAction += OC.OnShootProjectileAction;

            block.BlockAction = (Action<BlockTrigger.BlockTriggerType>)Delegate.Combine(block.BlockAction, new Action<BlockTrigger.BlockTriggerType>(GetDoBlockAction(player, block)));

            block.cdAdd = 2f;
        }

        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been removed from player {player.playerID}.");
            //Run when the card is removed from the player

            OverchargeEffect OC = player.gameObject.GetComponent<OverchargeEffect>();
            gun.ShootPojectileAction -= OC.OnShootProjectileAction;
            OC.Destroy();

            block.BlockAction = (Action<BlockTrigger.BlockTriggerType>)Delegate.Remove(block.BlockAction, GetDoBlockAction(player, block));
        }

        private Action<BlockTrigger.BlockTriggerType> GetDoBlockAction(Player player, Block block)
        {
            return delegate (BlockTrigger.BlockTriggerType trigger)
            {
                if (trigger != BlockTrigger.BlockTriggerType.None)
                {
                    // TODO: reload the weapon
                    
                    player.gameObject.GetComponent<OverchargeEffect>().shotsLeft = OC_SHOTS;
                }
            };
        }

        protected override string GetTitle()
        {
            return "Overcharge";
        }
        protected override string GetDescription()
        {
            return $"Blocking reloads and massively increases damage and attack speed for your next {OC_SHOTS} shots.";
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
                /*new CardInfoStat()
                {
                    positive = true,
                    stat = "Ammo when active",
                    amount = $"At least {OC_SHOTS}",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },*/
                new CardInfoStat()
                {
                    positive = false,
                    stat = "Block Cooldown",
                    amount = "+2s",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
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
