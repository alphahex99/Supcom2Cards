using BepInEx;
using UnboundLib.Cards;
using Supcom2Cards.Cards;
using HarmonyLib;
using UnityEngine;
using Jotunn.Utils;

namespace Supcom2Cards
{
    // These are the mods required for our mod to work
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.dk.rounds.plugins.zerogpatch", BepInDependency.DependencyFlags.HardDependency)]
    // Declares our mod to Bepin
    [BepInPlugin(ModId, ModName, Version)]
    // The game our mod is associated with
    [BepInProcess("Rounds.exe")]
    public class Supcom2 : BaseUnityPlugin
    {
        private const string ModId = "com.alphahex.rounds.supcom2cards";
        private const string ModName = "Supcom2 Cards";
        public const string Version = "1.0.1";
        public const string ModInitials = "SC2";

        public static Supcom2? instance { get; private set; }

        // card art
        private static readonly AssetBundle NukePrefab = AssetUtils.LoadAssetBundleFromResources("nuke", typeof(Supcom2).Assembly);
        public static GameObject NukeArt = NukePrefab.LoadAsset<GameObject>("C_Nuke");

        private static readonly AssetBundle ShotjaPrefab = AssetUtils.LoadAssetBundleFromResources("shotja", typeof(Supcom2).Assembly);
        public static GameObject ShotjaArt = ShotjaPrefab.LoadAsset<GameObject>("C_Shotja");

        void Awake()
        {
            // Use this to call any harmony patch files your mod may have
            var harmony = new Harmony(ModId);
            harmony.PatchAll();
        }
        void Start()
        {
#if FALSE
            CustomCard.BuildCard<RemoveFirst>();
            CustomCard.BuildCard<RemoveLast>();
            CustomCard.BuildCard<RemoveAll>();
#endif
            CustomCard.BuildCard<ChromeShield>();
            CustomCard.BuildCard<Colossus>();
            CustomCard.BuildCard<Disruptor>();
            CustomCard.BuildCard<FieldEngineer>();
            CustomCard.BuildCard<Harden>();
            CustomCard.BuildCard<Jackhammer>();
            CustomCard.BuildCard<Loyalist>();
            CustomCard.BuildCard<Magnetron>();
            CustomCard.BuildCard<Nuke>();
            CustomCard.BuildCard<Overcharge>();
            CustomCard.BuildCard<Poseidon>();
            CustomCard.BuildCard<RadarJammer>();
            CustomCard.BuildCard<Recycler>();
            CustomCard.BuildCard<RogueNanites>();
            CustomCard.BuildCard<Shotja>();
            CustomCard.BuildCard<SuperTriton>();
            CustomCard.BuildCard<Titan>();
            CustomCard.BuildCard<Tml>();
            CustomCard.BuildCard<Training>();

            instance = this;
        }
    }
}
