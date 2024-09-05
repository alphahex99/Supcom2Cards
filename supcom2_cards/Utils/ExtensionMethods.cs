#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable IDE0059 // Unnecessary assignment of a value

using Jotunn.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnboundLib;
using UnboundLib.Extensions;
using UnityEngine;

namespace Supcom2Cards
{
    public static partial class ExtensionMethods
    {
        /*
        public static float CooldownRatio(this Block block)
        {
            if (block.counter > block.Cooldown())
            {
                // block is ready
                return 1;
            }
            else
            {
                return block.counter / block.Cooldown();
            }
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
        */
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

        public static void LoadArt(this Dictionary<string, GameObject> CardArt, string bundleName, List<string> assetNames)
        {
            AssetBundle bundle = AssetUtils.LoadAssetBundleFromResources(bundleName, typeof(Supcom2).Assembly);

            foreach (string cardName in assetNames)
            {
                GameObject art = bundle.LoadAsset<GameObject>("C_" + cardName);
                if (art != null)
                {
                    CardArt.Add(cardName, art);
                }
            }
        }

        #region PLAYER
        public static Color Color(this Player player)
        {
            return player.GetTeamColors().color;
        }

        /// <returns>true if a game is in progress</returns>
        public static bool Simulated(this Player player)
        {
            return (bool)player.data.playerVel.GetFieldValue("simulated");
        }

        // default walk speed = 0.03f
        private static readonly float MAX_SPEED = 0.01f;
        public static bool StandingStill(this Player player, ref Vector3 lastPosition)
        {
            if (player.data.isGrounded || player.data.isWallGrab)
            {
                float dx = player.transform.position.x - lastPosition.x;
                dx = dx > 0 ? dx : -dx;
                float dy = player.transform.position.y - lastPosition.y;
                dy = dy > 0 ? dy : -dy;

                lastPosition = player.transform.position;
                return dx * dx + dy * dy < MAX_SPEED;
            }
            else
            {
                lastPosition = player.transform.position;
                return false;
            }
        }
        #endregion PLAYER

        #region GUN
        public static int CurrentAmmo(this GunAmmo gunAmmo)
        {
            return (int)gunAmmo.GetFieldValue("currentAmmo");
        }
        public static int CurrentAmmoAdd(this GunAmmo gunAmmo, int add)
        {
            if (add == 0)
            {
                return 0;
            }

            int oldAmmo = gunAmmo.CurrentAmmo();
            int newAmmo = oldAmmo + add;
            if (newAmmo < 0)
            {
                newAmmo = 0;
            }
            if (newAmmo > gunAmmo.maxAmmo)
            {
                newAmmo = gunAmmo.maxAmmo;
            }

            gunAmmo.SetFieldValue("currentAmmo", newAmmo);

            // return difference
            return newAmmo - oldAmmo;
        }
        public static Gun Gun(this Player player)
        {
            return player.data.weaponHandler.gun;
        }
        public static GunAmmo GunAmmo(this Gun gun)
        {
            return (GunAmmo)gun.GetFieldValue("gunAmmo");
        }
        public static float ReloadTime(this GunAmmo gunAmmo)
        {
            return (gunAmmo.reloadTime + gunAmmo.reloadTimeAdd) * gunAmmo.reloadTimeMultiplier;
        }
        public static void SetActiveBullets(this GunAmmo gunAmmo, bool forceTurnOn = false)
        {
            typeof(GunAmmo).InvokeMember("SetActiveBullets", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, gunAmmo, new object[] { forceTurnOn });
        }
        public static void UpdateAmmo(this Gun gun)
        {
            GunAmmo gunAmmo = gun.GunAmmo();
            int currentAmmo = gunAmmo.CurrentAmmo();
            if (currentAmmo > 0)
            {
                if (gun.isReloading)
                {
                    // freeReloadCounter doesn't go up when reloading with 0 currentAmmo
                    // swap freeReloadCounter / reloadCounter
                    float reloadCounter = (float)gunAmmo.GetFieldValue("reloadCounter");
                    float freeReloadCounter = gunAmmo.ReloadTime() - reloadCounter;
                    gunAmmo.SetFieldValue("freeReloadCounter", freeReloadCounter);
                    gunAmmo.SetFieldValue("reloadCounter", reloadCounter);

                    // force finish reload
                    gunAmmo.ReloadAmmo();
                    gunAmmo.SetFieldValue("currentAmmo", currentAmmo);
                }

                // redraw AMMO without changing currentAmmo (which GunAmmo::ReDrawTotalBullets does)
                for (int i = gunAmmo.populate.transform.childCount - 1; i >= 0; i--)
                {
                    if (gunAmmo.populate.transform.GetChild(i).gameObject.activeSelf)
                    {
                        UnityEngine.Object.Destroy(gunAmmo.populate.transform.GetChild(i).gameObject);
                    }
                }
                gunAmmo.populate.times = gunAmmo.CurrentAmmo();
                gunAmmo.populate.DoPopulate();
                gunAmmo.SetActiveBullets(true);
            }
        }
        #endregion GUN

        public static void SetTeamColor(this List<Laser> lasers, Player player, float brightness_mult = 1f)
        {
            lasers.ForEach(l => l.Color = player.Color() * brightness_mult);
        }
        public static void SetTeamColor(this List<Laser> lasers, GameObject gameObjectMono, float brightness_mult = 1f)
        {
            lasers.SetTeamColor(gameObjectMono.GetComponentInParent<Player>(), brightness_mult);
        }
    }
}
