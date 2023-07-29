using Supcom2Cards.MonoBehaviours;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class BombBouncer : CustomCard
    {
        public static readonly Color COLOR_CHARGED = Color.red;
        public static readonly Color COLOR_UNCHARGED = Color.yellow;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            data.maxHealth *= 1.3f;

            block.cdAdd += 0.5f;

            BombBouncerEffect bombBouncerEffect = player.IncrementCardEffect<BombBouncerEffect>();
            if (bombBouncerEffect.CardAmount == 1)
            {
                (GameObject AddToProjectile, GameObject effect, _) = Supcom2.LoadExplosion("explosionBombBouncer");

                bombBouncerEffect.Explosion = new ObjectsToSpawn
                {
                    AddToProjectile = AddToProjectile,
                    direction = ObjectsToSpawn.Direction.forward,
                    effect = effect,
                    normalOffset = 0.1f,
                    scaleFromDamage = 0.5f,
                    scaleStackM = 0.7f,
                    scaleStacks = true,
                    spawnAsChild = false,
                    spawnOn = ObjectsToSpawn.SpawnOn.all,
                    stacks = 0,
                    stickToAllTargets = false,
                    stickToBigTargets = false,
                    zeroZ = false
                };

                // set this player as owner of the explosion
                effect.GetOrAddComponent<SpawnedAttack>().spawner = player;
            }
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.DecrementCardEffect<BombBouncerEffect>();
        }

        protected override string GetTitle()
        {
            return "Bomb Bouncer";
        }
        protected override string GetDescription()
        {
            return "Damage you take will be released\n" +
                "in a big explosion next block\n";
        }
        protected override GameObject GetCardArt()
        {
            _ = Supcom2.CardArt.TryGetValue("BombBouncer", out GameObject cardArt);
            return cardArt;
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
                    positive = true,
                    stat = "Max Charge",
                    amount = "+100%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "HP",
                    amount = "+30%",
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
            return CardThemeColor.CardThemeColorType.DestructiveRed;
        }
        public override string GetModName()
        {
            return Supcom2.ModInitials;
        }
    }
}
