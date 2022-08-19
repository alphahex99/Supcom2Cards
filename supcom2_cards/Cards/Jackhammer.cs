using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace Supcom2Cards.Cards
{
    class Jackhammer : CustomCard
    {
        private RemoveAfterSeconds? explosionRemove;
        private ObjectsToSpawn? explosionSpawn;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been setup.");
            //Edits values on card itself, which are then applied to the player in `ApplyCardStats`


        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been added to player {player.playerID}.");
            //Edits values on player when card is selected

            gun.size += 1f;
            gun.projectileSize += 3f;
            
            gun.projectileSpeed *= 1.5f;
            //gun.attackSpeed /= 0.2f;

            // add explosion effect
            if (explosionSpawn == null)
            {
                GameObject? explosiveBullet = (GameObject)Resources.Load("0 cards/Explosive bullet");
                GameObject explosion = Instantiate(explosiveBullet.GetComponent<Gun>().objectsToSpawn[0].effect);
                explosion.transform.position = new Vector3(1000, 0, 0);
                explosion.hideFlags = HideFlags.HideAndDontSave;
                explosionRemove = explosion.GetComponent<RemoveAfterSeconds>();
                explosionRemove.seconds = 1000000f;
                explosion.GetComponent<Explosion>().force = 100000;

                explosionSpawn = new ObjectsToSpawn
                {
                    effect = explosion,
                    normalOffset = 0.1f,
                    numberOfSpawns = 1,
                    scaleFromDamage = .5f,
                };
            }
            gun.objectsToSpawn = new[] { explosionSpawn };
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log($"[{Supcom2.ModInitials}][Card] {GetTitle()} has been removed from player {player.playerID}.");
            //Run when the card is removed from the player

            // remove explosion effect
            if (explosionRemove != null)
            {
                explosionRemove.seconds = 0f;
                explosionSpawn = null;
            }
        }

        protected override string GetTitle()
        {
            return "Jackhammer";
        }
        protected override string GetDescription()
        {
            return "Fire huge projectiles that explode";
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
                    positive = true,
                    stat = "Projectile speed",
                    amount = "+50%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = false,
                    stat = "ATKSPD",
                    amount = "-80%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
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
