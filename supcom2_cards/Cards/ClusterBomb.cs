using UnboundLib.Cards;
using UnityEngine;
using UnboundLib;
using Supcom2Cards.RoundsEffects;

namespace Supcom2Cards.Cards
{
    class ClusterBomb : CustomCard
    {
        public static readonly int EXPLOSION_AMOUNT = 7;
        public static readonly int EXPLOSION_DMG = 20;
        public static readonly int EXPLOSION_SPREAD = 6;

        public static readonly int FRAMES_MIN = 1;
        public static readonly int FRAMES_MAX = 25;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage *= 0.6f;

            gun.attackSpeed *= 2f;

            ClusterBombEffect clusterBomb = player.IncrementCardEffect<ClusterBombEffect>();
            if (clusterBomb.CardAmount == 1)
            {
                (GameObject AddToProjectile, GameObject effect, Explosion explosion) = Supcom2.LoadExplosion("explosionClusterBomb");

                explosion.damage = EXPLOSION_DMG;
                explosion.force *= 0.01f;

                clusterBomb.Explosion = new ObjectsToSpawn
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
            player.DecrementCardEffect<ClusterBombEffect>();
        }

        protected override string GetTitle()
        {
            return "Cluster Bomb";
        }
        protected override string GetDescription()
        {
            return $"Bullets create {EXPLOSION_AMOUNT} (extra) tiny explosions ({EXPLOSION_DMG} DMG) after impact";
        }
        protected override GameObject GetCardArt()
        {
            _ = Supcom2.CardArt.TryGetValue("ClusterBomb", out GameObject cardArt);
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
                    positive = false,
                    stat = "DMG",
                    amount = "-40%",
                    simepleAmount = CardInfoStat.SimpleAmount.lower
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "ATKSPD",
                    amount = "50%",
                    simepleAmount = CardInfoStat.SimpleAmount.lower
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DefensiveBlue;
        }
        public override string GetModName()
        {
            return Supcom2.ModInitials;
        }
    }
}
