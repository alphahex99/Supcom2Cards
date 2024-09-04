﻿using Supcom2Cards.MonoBehaviours;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class Wilfindja : CustomCard
    {
        public const float DPS_REL = 1f;
        public const int DRONES = 12;
        public const float DRONE_DISTANCE = 7f;
        public const int DRONE_EDGES = 3;
        public const float DRONE_HITBOX = 15f;
        public const float DRONE_RPM_MULT = 3f;
        public const float DRONE_SIZE = 0.3f;
        public const float RPM = 60f;
        public const float UPS = 10f; // updates per second

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            gun.damage = 0.5f;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.IncrementCardEffect<WilfindjaEffect>();
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.DecrementCardEffect<WilfindjaEffect>();
        }

        protected override string GetTitle()
        {
            return "Wilfindja";
        }
        protected override string GetDescription()
        {
            return $"Friendly drones follow you around\n" +
                $"and damage enemies\n(Wilfindja DPS = gun DMG)\n";
        }
        protected override GameObject GetCardArt()
        {
            _ = Supcom2.CardArt.TryGetValue("Wilfindja", out GameObject cardArt);
            return cardArt;
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
                    stat = "Drones",
                    amount = "+12",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "DMG",
                    amount = "-50%",
                    simepleAmount = CardInfoStat.SimpleAmount.aLotLower
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
