using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using FarlandsCoreMod;
using FarlandsCoreMod.Managers;
using UnityEngine;
using HarmonyLib;

namespace FarlandsCoreMod;

[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
public class FCM : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    public static FCM Instance = null;
    private Harmony harmony;

    public List<Mod> Mods { get; private set; }

    private void Awake()
    {
        Mods = new();

        if (Instance == null)
            Instance = this;

        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"FarlandsCoreMod is Loaded!");

        var managersGameObject = new GameObject("Managers");
        managersGameObject.transform.SetParent(transform);

        harmony = new Harmony(PluginInfo.GUID);
        harmony.PatchAll();

        Logger.LogInfo("Patches applied");

        Assembly.GetAssembly(typeof(FCM))
            .GetTypes()
            .Where(t => typeof(Manager).IsAssignableFrom(t) && t != typeof(Manager))
            .ToList()
            .ForEach(m => managersGameObject.AddComponent(m));

        Invoke("ListConfigs", 5);
    }

    public void ListConfigs()
    {
        Logger.LogInfo("---------- Configurations ----------");
        foreach (var m in Mods)
        {
            Logger.LogInfo($"--- {m.Info.GUID} ---");
            foreach (var c in m.Config.ToList())
            {
                Logger.LogInfo($"{c.Key.Section}.{c.Key.Key} = {c.Value.BoxedValue}");
            }
        }
    }

    public void RegisterMod(Mod mod)
    {
        Mods.Add(mod);
        Logger.LogInfo($"{mod.Info.GUID} loaded by FCM");
    }
}