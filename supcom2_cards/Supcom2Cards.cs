using BepInEx;
using UnboundLib;
using UnboundLib.Cards;
using Supcom2.Cards;
using HarmonyLib;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;

namespace Supcom2
{
    // These are the mods required for our mod to work
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch", BepInDependency.DependencyFlags.HardDependency)]
    // Declares our mod to Bepin
    [BepInPlugin(ModId, ModName, Version)]
    // The game our mod is associated with
    [BepInProcess("Rounds.exe")]
    public class Supcom2Cards : BaseUnityPlugin
    {
        private const string ModId = "com.alphahex.rounds.supcom2cards";
        private const string ModName = "Supcom2 Cards";
        public const string Version = "1.0.0"; // What version are we on (major.minor.patch)?

        public const string ModInitials = "SC2";

        public static Supcom2Cards instance { get; private set; }

        void Awake()
        {
            // Use this to call any harmony patch files your mod may have
            var harmony = new Harmony(ModId);
            harmony.PatchAll();
        }
        void Start()
        {
            CustomCard.BuildCard<Shotja>();

            instance = this;
        }
    }
}
