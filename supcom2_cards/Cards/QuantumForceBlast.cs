using Supcom2Cards.RoundsEffects;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class QuantumForceBlast : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            gun.knockback = 4f;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            QuantumForceBlastEffect effect = player.IncrementCardEffect<QuantumForceBlastEffect>();
            effect.Owner = player;
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.DecrementCardEffect<QuantumForceBlastEffect>();
        }

        protected override string GetTitle()
        {
            return "Quantum Force Blast";
        }
        protected override string GetDescription()
        {
            return null;
        }
        protected override GameObject GetCardArt()
        {
            _ = Supcom2.CardArt.TryGetValue("QuantumForceBlast", out GameObject cardArt);
            return cardArt;
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
                    stat = "Knockback",
                    amount = "+300%",
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
