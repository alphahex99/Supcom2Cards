#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable IDE0059 // Unnecessary assignment of a value

using System;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

namespace Supcom2Cards
{
    public static partial class ExtensionMethods
    {
        public static float CooldownRatio(this Block block)
        {
            float cooldown = block.Cooldown();
            if (block.counter > cooldown)
            {
                return 1;
            }
            return block.counter / cooldown;
        }

        public static float DPS(this Gun gun)
        {
            GunAmmo gunAmmo = (GunAmmo)gun.GetFieldValue("gunAmmo");

            // theoretical dps with infinite ammo
            float dps = 55 * gun.damage / gun.attackSpeed;

            // time spent reloading adjustment
            dps *= gunAmmo.maxAmmo * gun.attackSpeed / ((2 + gunAmmo.reloadTimeAdd) * gunAmmo.reloadTimeMultiplier); // time to empty clip / reload time

            return dps;
        }

        public static void RemovePlayerDiedAction(this PlayerManager pm, Action<Player, int> listener)
        {
            Action<Player, int> action = (Action<Player, int>)pm.GetFieldValue("PlayerDiedAction");
            action -= listener;
        }

        public static void SetListCount<T>(this List<T> list, int count) where T : new()
        {
            // overcomplicated but useful for testing

            int current = list.Count;
            if (count > current)
            {
                for (int i = 0; i < count - current; i++)
                {
                    list.Add(new T());
                }
            }
            else if (current > count)
            {
                for (int i = 0; i < current - count; i++)
                {
                    list.RemoveAt(0);
                }
            }
        }

        public static void TakeDamage(this Player player, float damage, Player? damagingPlayer = null)
        {
            player.data.health -= damage;

            if (player.data.health <= 0)
            {
                // remind the game that this guy is supposed to be dead
                player.data.healthHandler.TakeDamage(Vector2.up, player.data.transform.position, damagingPlayer: damagingPlayer, ignoreBlock: true);
            }
        }
    }
}
