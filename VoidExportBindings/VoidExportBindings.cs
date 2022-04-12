using BepInEx;
using HarmonyLib;
using JetBrains.Annotations;
using VoidExportBindings.Core.Patches;

namespace VoidExportBindings
{
    [BepInPlugin(k_PluginNameSpace, k_PluginName, k_PluginVersion)]
    public class VoidExportBindings : BaseUnityPlugin
    {
        [UsedImplicitly] public const string k_PluginNameSpace = "org.com.VoidExportBindings";
        [UsedImplicitly] public const string k_PluginName = "VoidExportBindings";
        [UsedImplicitly] public const string k_PluginVersion = "0.1.0";
        [UsedImplicitly] public const string k_PluginVersionMinRequired = "0.1.0";

        private Harmony _harmony = null!;

        private void Awake()
        {
            _harmony = new Harmony(Info.Metadata.GUID);
            _harmony.PatchAll();
        }

        private void Start()
        {
            ConsolePatch.Init();
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }
    }
}