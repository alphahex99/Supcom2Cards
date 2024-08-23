using UnboundLib.Cards;
using UnityEngine;
using UnboundLib;
using Supcom2Cards.RoundsEffects;
using Sonigon;

namespace Supcom2Cards.Cards
{
    class ClusterBomb : CustomCard
    {
        public static readonly int EXPLOSION_AMOUNT = 5;
        public static readonly float EXPLOSION_DMG_MULT = 0.4f * 55f; // MULT * gun.damage
        public static readonly int EXPLOSION_SPREAD = 7;

        public static readonly int FRAMES_MIN = 5;
        public static readonly int FRAMES_MAX = 15;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damage *= 1.25f;

            gun.reflects = -99999;

            ClusterBombEffect clusterBomb = player.IncrementCardEffect<ClusterBombEffect>();
            if (clusterBomb.CardAmount == 1)
            {
                (GameObject AddToProjectile, GameObject effect, Explosion explosion) = Supcom2.LoadExplosion("explosionClusterBomb");
                
                clusterBomb.Explosion = explosion;
                explosion.force *= 0.1f;

                clusterBomb.ExplosionToSpawn = new ObjectsToSpawn
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

                clusterBomb.gun = gun;
                ClusterBombEffect.sound = gun.player.data.playerSounds.soundCharacterLand;
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
            return $"Bullets create {EXPLOSION_AMOUNT} (extra) tiny explosions after impact";
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
                    positive = true,
                    stat = "DMG",
                    amount = "+25%",
                    simepleAmount = CardInfoStat.SimpleAmount.lower
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "Bullet bounces",
                    amount = "NO",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
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
