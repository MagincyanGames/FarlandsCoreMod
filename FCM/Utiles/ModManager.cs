using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.Bootstrap;
using FMOD;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FarlandsCoreMod.Utiles
{
    public static class ModManager
    {
        public static BaseUnityPlugin GetMod(string guid) => (BaseUnityPlugin)UnityChainloader.Instance.Plugins[guid].Instance;
        public static T GetMod<T>() where T : BaseUnityPlugin
        {
            foreach (var plugin in UnityChainloader.Instance.Plugins.Values)
            {
                if (plugin.Instance is T mod)
                {
                    return mod;
                }
            }
            return null;
        }

        public static List<BaseUnityPlugin> mods => UnityChainloader.Instance.Plugins.Values
            .Where(x => x.Dependencies.Any(x=>x.DependencyGUID == "magin.fcm") || x.Metadata.GUID == "magin.fcm")
            .ToList().ConvertAll(plugin => (BaseUnityPlugin)plugin.Instance);
    }
}
