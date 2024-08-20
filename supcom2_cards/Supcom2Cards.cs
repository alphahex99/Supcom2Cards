using BepInEx;
using UnboundLib.Cards;
using Supcom2Cards.Cards;
using HarmonyLib;
using UnityEngine;
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
        public const string Version = "1.3.9";

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
            LoadCardArt();
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
            //CustomCard.BuildCard<FieldEngineer>();
            CustomCard.BuildCard<Fistoosh>();
            CustomCard.BuildCard<HalfBaked>();
            CustomCard.BuildCard<Harden>();
            CustomCard.BuildCard<Harvog>();
            CustomCard.BuildCard<Hunker>();
            CustomCard.BuildCard<Jackhammer>();
            CustomCard.BuildCard<JumpJets>();
            CustomCard.BuildCard<Loyalist>();
            CustomCard.BuildCard<LoyaltyGun>();
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
            CustomCard.BuildCard<RateOfFire>();
            CustomCard.BuildCard<Recycler>();
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

        private void LoadCardArt()
        {
            // Krawl (OLD)
            List<string> c1 = new List<string>()
            {
                "ClusterBomb",
                "Colossus",
                "Darkenoid",
                "Disruptor",
                "Magnetron",
                "Megalith",
                "Poseidon",
                "Shotja",
                "Tml",
                //"Veterancy",
            };

            // Krawl (NEW)
            List<string> c2 = new List<string>()
            {
                "Afterburn",
                "Buhbledow",
                "ChromeShield",
                "Crahdow",
                "DynamicPowerShunt",
                "Fatboy",
                "FieldEngineer",
                "Fistoosh",
                "HalfBaked",
                "Harden",
                "Harvog",
                "Hunker",
                "Jackhammer",
                "JumpJets",
                "Loyalist",
                "Overcharge",
                "Poseidon",
                "Pulinsmash",
                "QuantumForceBlast",
                "QuantumSponge",
                "RateOfFire",
                "Recycler",
                "RockHead",
                "RogueNanites",
                "StackedCannons",
                "SuperTriton",
                "Titan",
                "Training",
                "Urchinow",
                "Wilfindja"
            };

            List<string> c3 = new List<string>() // Roxban
            {
                "BombBouncer",
            };

            CardArt.LoadArt("Supcom2Art_1", c1);
            CardArt.LoadArt("Supcom2Art_2", c2);
            CardArt.LoadArt("sc2test", c3);
        }

        public static void Log(string word)
        {
            UnityEngine.Debug.Log(word);
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

            // fix silenced on round start before blocking
            foreach (HalfBakedEffect effect in FindObjectsOfType<HalfBakedEffect>())
            {
                Player player = effect.player;

                player.data.silenceTime = 0f;
            }

            foreach (JumpJetsEffect effect in FindObjectsOfType<JumpJetsEffect>())
            {
                effect.Refuel();
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
