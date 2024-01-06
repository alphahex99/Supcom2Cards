﻿using BepInEx;
using UnboundLib.Cards;
using Supcom2Cards.Cards;
using HarmonyLib;
using UnityEngine;
using Jotunn.Utils;
using System.Collections;
using UnboundLib.GameModes;
using System.Collections.Generic;
using Supcom2Cards.MonoBehaviours;
using UnboundLib;

namespace Supcom2Cards
{
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.willis.rounds.modsplus", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class Supcom2 : BaseUnityPlugin
    {
        public const string Version = "1.3.5";

        private const string ModId = "com.alphahex.rounds.supcom2cards";
        private const string ModName = "Supcom2 Cards";
        public const string ModInitials = "SC2";

        public static Dictionary<string, GameObject> CardArt = new Dictionary<string, GameObject>();

        public static Supcom2? Instance { get; private set; }

        public static bool PickPhase = false;

        void Awake()
        {
            // Use this to call any harmony patch files your mod may have
            Harmony harmony = new Harmony(ModId);
            harmony.PatchAll();

            GameModeManager.AddHook(GameModeHooks.HookGameEnd, GameEnd);
            GameModeManager.AddHook(GameModeHooks.HookPointEnd, PointEnd);
            GameModeManager.AddHook(GameModeHooks.HookPointStart, PointStart);
            GameModeManager.AddHook(GameModeHooks.HookRoundEnd, RoundEnd);
            GameModeManager.AddHook(GameModeHooks.HookPlayerPickEnd, (gm) => TempExtraPicks.HandleExtraPicks());
        }

        void Start()
        {
            List<string> cardArt = new List<string>()
            {
                //"Afterburn",
                "BombBouncer",
                "Buhbledow",
                "ChromeShield",
                "ClusterBomb",
                "Colossus",
                "Darkenoid",
                "Disruptor",
                //"DynamicPowerShunt",
                //"Fatboy",
                "Fistoosh",
                //"HalfBaked",
                //"Harden",
                //"Harvog",
                //"Hunker",
                "Jackhammer",
                //"JumpJets",
                "Loyalist",
                "Magnetron",
                "Megalith",
                //"NanobotWeapon",
                "Overcharge",
                "Poseidon",
                //"ProtoBrain",
                "Pulinsmash",
                //"QuantumSponge",
                //"QuantumForceBlast",
                //"RadarJammer",
                //"Recycler",
                //"RockHead",
                //"RogueNanites",
                "Shotja",
                //"StackedCannons",
                //"SuperTriton",
                //"Titan",
                "Tml",
                //"Training",
                //"Urchinow",
                "Veterancy",
                "Wilfindja"
            };

            AssetBundle bundle1 = AssetUtils.LoadAssetBundleFromResources("Supcom2Art_1", typeof(Supcom2).Assembly);
            AssetBundle bundle2 = AssetUtils.LoadAssetBundleFromResources("Supcom2Art_2", typeof(Supcom2).Assembly);
            AssetBundle bundle3 = AssetUtils.LoadAssetBundleFromResources("sc2test", typeof(Supcom2).Assembly);
            foreach (string cardName in cardArt)
            {
                GameObject art = bundle2.LoadAsset<GameObject>("C_" + cardName);
                if (art == null)
                {
                    art = bundle1.LoadAsset<GameObject>("C_" + cardName);
                }
                if (art == null)
                {
                    art = bundle3.LoadAsset<GameObject>("C_" + cardName);
                }
                if (art != null)
                {
                    CardArt.Add(cardName, art);
                }
            }
#if FALSE
            CustomCard.BuildCard<Crash>();
            CustomCard.BuildCard<FreezeBullets>();

            // testing cards by willuwontu
            CustomCard.BuildCard<RemoveFirst>();
            CustomCard.BuildCard<RemoveLast>();
            CustomCard.BuildCard<RemoveAll>();
#endif
            CustomCard.BuildCard<Afterburn>();
            CustomCard.BuildCard<BombBouncer>();
            CustomCard.BuildCard<Buhbledow>();
            CustomCard.BuildCard<ChromeShield>();
            CustomCard.BuildCard<ClusterBomb>();
            CustomCard.BuildCard<Colossus>();
            CustomCard.BuildCard<Crahdow>();
            CustomCard.BuildCard<Darkenoid>();
            CustomCard.BuildCard<Disruptor>();
            CustomCard.BuildCard<DynamicPowerShunt>();
            CustomCard.BuildCard<Fatboy>();
            CustomCard.BuildCard<Fistoosh>();
            CustomCard.BuildCard<HalfBaked>();
            CustomCard.BuildCard<Harden>();
            CustomCard.BuildCard<Harvog>();
            CustomCard.BuildCard<Hunker>();
            CustomCard.BuildCard<Jackhammer>();
            CustomCard.BuildCard<JumpJets>();
            CustomCard.BuildCard<Loyalist>();
            CustomCard.BuildCard<Magnetron>();
            CustomCard.BuildCard<Megalith>();
            //CustomCard.BuildCard<NanobotWeapon>();
            CustomCard.BuildCard<Overcharge>();
            CustomCard.BuildCard<Poseidon>();
            //CustomCard.BuildCard<ProtoBrain>(); TODO: Fix
            CustomCard.BuildCard<Pulinsmash>();
            CustomCard.BuildCard<QuantumForceBlast>();
            CustomCard.BuildCard<QuantumSponge>();
            //CustomCard.BuildCard<RadarJammer>(); TODO: Fix
            //CustomCard.BuildCard<Recycler>(); TODO: Fix
            CustomCard.BuildCard<RockHead>();
            CustomCard.BuildCard<RogueNanites>();
            CustomCard.BuildCard<Shotja>();
            CustomCard.BuildCard<StackedCannons>();
            CustomCard.BuildCard<SuperTriton>();
            CustomCard.BuildCard<Titan>();
            CustomCard.BuildCard<Tml>();
            CustomCard.BuildCard<Training>();
            CustomCard.BuildCard<Urchinow>();
            //CustomCard.BuildCard<Veterancy>(); TODO: Fix
            CustomCard.BuildCard<Wilfindja>();

            Instance = this;
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

        public List<int> GetRoundWinners() => new List<int>(GameModeManager.CurrentHandler.GetRoundWinners());

        private IEnumerator GameEnd(IGameModeHandler gm)
        {
            ISingletonEffect.GameEnd();

            yield break;
        }

        private IEnumerator PointEnd(IGameModeHandler gm)
        {
            PickPhase = true;

            yield break;
        }

        private IEnumerator PointStart(IGameModeHandler gm)
        {
            PickPhase = false;

            foreach (BombBouncerEffect effect in FindObjectsOfType<BombBouncerEffect>())
            {
                effect.Charge = 0f;
            }

            // fix block meters not being full when round starts
            foreach (DynamicPowerShuntEffect effect in FindObjectsOfType<DynamicPowerShuntEffect>())
            {
                Block block = effect.player.data.block;

                block.sinceBlock = block.Cooldown() / DynamicPowerShunt.CD_MULT_STILL * 5f;
            }

            foreach (RadarJammerEffect effect in FindObjectsOfType<RadarJammerEffect>())
            {
                effect.RemoveJammed();
            }

            yield break;
        }

        private IEnumerator RoundEnd(IGameModeHandler gm)
        {
            // give extra picks to players with Proto-Brain who just won
            List<int> winners = GetRoundWinners();
            foreach (ProtoBrainEffect effect in FindObjectsOfType<ProtoBrainEffect>())
            {
                if (winners.Contains(effect.player.teamID))
                {
                    effect.player.gameObject.GetOrAddComponent<TempExtraPicks>().ExtraPicks++;
                }
            }

            yield break;
        }
    }
}
