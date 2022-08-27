using Supcom2Cards.MonoBehaviours;
using System.Linq;
using UnboundLib.Cards;
using UnityEngine;
using ModdingUtils;

namespace Supcom2Cards.Cards
{
    class ClusterBomb : CustomCard
    {
        private readonly int EXPLOSIONS = 15;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been setup.");
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`

            
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been added to player {player.playerID}.");
            //Edits values on player when card is selected

            ClusterBombEffect clusterBomb = player.gameObject.GetComponent<ClusterBombEffect>();
            if (clusterBomb == null)
            {
                clusterBomb = player.gameObject.AddComponent<ClusterBombEffect>();

                // load explosion effect from Explosive Bullet card
                GameObject? explosiveBullet = (GameObject)Resources.Load("0 cards/Explosive bullet");
                GameObject A_ExplosionSpark = explosiveBullet.GetComponent<Gun>().objectsToSpawn[0].AddToProjectile;
                GameObject A_Explosion = explosiveBullet.GetComponent<Gun>().objectsToSpawn[0].effect;
                GameObject explosionClusterBomb = Instantiate(A_Explosion);
                explosionClusterBomb.transform.position = new Vector3(1000, 0, 0);
                explosionClusterBomb.hideFlags = HideFlags.HideAndDontSave;
                explosionClusterBomb.name = "explosionClusterBomb";
                DestroyImmediate(explosionClusterBomb.GetComponent<RemoveAfterSeconds>());
                Explosion explosion = explosionClusterBomb.GetComponent<Explosion>();

                // temporary, to see if explosions are doing anything
                explosion.damage *= 100f;
                explosion.range *= 100f;
                explosion.silence += 100f;
                explosion.stun += 100f;

                clusterBomb.Explosion = new ObjectsToSpawn
                {
                    AddToProjectile = A_ExplosionSpark,
                    direction = ObjectsToSpawn.Direction.forward,
                    effect = explosionClusterBomb,
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
                clusterBomb.Explosions = EXPLOSIONS;
                clusterBomb.FramesBetweenExplosions = 15;
                clusterBomb.Player = player;
                clusterBomb.Spread = 5;
            }
            clusterBomb.HowMany++;

        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been removed from player {player.playerID}.");
            //Run when the card is removed from the player

            ClusterBombEffect clusterBomb = player.gameObject.GetComponent<ClusterBombEffect>();
            clusterBomb.HowMany--;

            if (clusterBomb.HowMany < 1)
            {
                Destroy(clusterBomb);
            }
        }

        protected override string GetTitle()
        {
            return "Cluster Bomb";
        }
        protected override string GetDescription()
        {
            return $"Bullets create {EXPLOSIONS} (extra) tiny explosions after impact";
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
            return CardThemeColor.CardThemeColorType.DefensiveBlue;
        }
        public override string GetModName()
        {
            return Supcom2.ModInitials;
        }
    }
}
