using BepInEx;
using UnboundLib.Cards;
using Supcom2Cards.Cards;
using HarmonyLib;
using UnityEngine;
using Jotunn.Utils;
using System;
using System.Collections.Generic;

namespace Supcom2Cards
{
    // These are the mods required for our mod to work
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch", BepInDependency.DependencyFlags.HardDependency)]
    // Declares our mod to Bepin
    [BepInPlugin(ModId, ModName, Version)]
    // The game our mod is associated with
    [BepInProcess("Rounds.exe")]
    public class Supcom2 : BaseUnityPlugin
    {
        private const string ModId = "com.alphahex.rounds.supcom2cards";
        private const string ModName = "Supcom2 Cards";
        public const string Version = "1.1.2";
        public const string ModInitials = "SC2";

        public static Supcom2? instance { get; private set; }

        public static Dictionary<string, GameObject> CardArt = new Dictionary<string, GameObject>();
        void Awake()
        {
            // Use this to call any harmony patch files your mod may have
            var harmony = new Harmony(ModId);
            harmony.PatchAll();
        }

        void Start()
        {
            List<string> cardArt = new List<string>()
            {
                //"Afterburn",
                //"Buhbledow",
                "ChromeShield",
                "ClusterBomb",
                "Colossus",
                "Darkenoid",
                "Disruptor",
                //"Harden",
                "Jackhammer",
                //"Loyalist",
                "Magnetron",
                "Megalith",
                "Nuke",
                "Overcharge",
                "Poseidon",
                //"QuantumSponge",
                //"RadarJammer",
                //"Recycler",
                //"RogueNanites",
                "Shotja",
                //"StackedCannons",
                //"SuperTriton",
                //"Titan",
                "Tml",
                //"Training",
                "Veterancy"
            };

            AssetBundle bundle = AssetUtils.LoadAssetBundleFromResources("Supcom2Art", typeof(Supcom2).Assembly);
            foreach (string cardName in cardArt)
            {
                GameObject art = bundle.LoadAsset<GameObject>("C_" + cardName);
                if (cardArt != null)
                {
                    CardArt.Add(cardName, art);
                }
            }
#if FALSE
            CustomCard.BuildCard<FreezeBullets>();

            // testing cards by willuwontu
            CustomCard.BuildCard<RemoveFirst>();
            CustomCard.BuildCard<RemoveLast>();
            CustomCard.BuildCard<RemoveAll>();
#endif
            CustomCard.BuildCard<Afterburn>();
            CustomCard.BuildCard<Buhbledow>();
            CustomCard.BuildCard<ChromeShield>();
            CustomCard.BuildCard<ClusterBomb>();
            CustomCard.BuildCard<Colossus>();
            CustomCard.BuildCard<Darkenoid>();
            CustomCard.BuildCard<Disruptor>();
            CustomCard.BuildCard<Harden>();
            CustomCard.BuildCard<Jackhammer>();
            CustomCard.BuildCard<Loyalist>();
            CustomCard.BuildCard<Magnetron>();
            CustomCard.BuildCard<Megalith>();
            CustomCard.BuildCard<Nuke>();
            CustomCard.BuildCard<Overcharge>();
            CustomCard.BuildCard<Poseidon>();
            CustomCard.BuildCard<QuantumSponge>();
            CustomCard.BuildCard<RadarJammer>();
            CustomCard.BuildCard<Recycler>();
            CustomCard.BuildCard<RogueNanites>();
            CustomCard.BuildCard<Shotja>();
            CustomCard.BuildCard<StackedCannons>();
            CustomCard.BuildCard<SuperTriton>();
            CustomCard.BuildCard<Titan>();
            CustomCard.BuildCard<Tml>();
            CustomCard.BuildCard<Training>();
            CustomCard.BuildCard<Veterancy>();

            instance = this;
        }

        public static float GetGunDPS(Gun gun, GunAmmo gunAmmo)
        {
            // theoretical dps with infinite ammo
            float dps = 55 * gun.damage / gun.attackSpeed;

            // time spent reloading adjustment
            dps *= gunAmmo.maxAmmo * gun.attackSpeed / ((2 + gunAmmo.reloadTimeAdd) * gunAmmo.reloadTimeMultiplier); // time to empty clip / reload time

            return dps;
        }

        public static (GameObject AddToProjectile, GameObject effect, Explosion explosion) LoadExplosion(string name, Gun? gun = null)
        {
            // load explosion effect from Explosive Bullet card
            GameObject? explosiveBullet = (GameObject)Resources.Load("0 cards/Explosive bullet");

            Gun explosiveGun = explosiveBullet.GetComponent<Gun>();

            if (gun != null)
            {
                // change the gun sounds
                gun.soundGun.AddSoundImpactModifier(explosiveGun.soundImpactModifier);
            }

            // load assets
            GameObject A_ExplosionSpark = explosiveGun.objectsToSpawn[0].AddToProjectile;
            GameObject explosionCustom = Instantiate(explosiveGun.objectsToSpawn[0].effect);
            explosionCustom.transform.position = new Vector3(1000, 0, 0);
            explosionCustom.hideFlags = HideFlags.HideAndDontSave;
            explosionCustom.name = name;
            DestroyImmediate(explosionCustom.GetComponent<RemoveAfterSeconds>());
            Explosion explosion = explosionCustom.GetComponent<Explosion>();

            return (A_ExplosionSpark, explosionCustom, explosion);
        }
    }
}
