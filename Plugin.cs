using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using FarlandsCoreMod;
using UnityEngine;

namespace FarlandsCoreMod;

[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
public class FCM : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    public static FCM Instance = null;

    public List<Mod> Mods { get; private set; }

    private void Awake()
    {
        
        if (Instance == null)
            Instance = this;

        Mods = new();

        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"FarlandsCoreMod is Loaded!");
    }

    public void RegisterMod(Mod mod)
    {
        Mods.Add(mod);
        Logger.LogInfo($"{mod.Info.GUID} loaded!");
    }
}