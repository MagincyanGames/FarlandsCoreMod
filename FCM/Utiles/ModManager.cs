using BepInEx;
using BepInEx.Unity.Bootstrap;
using System;
using System.Collections.Generic;
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

        public static List<BaseUnityPlugin> mods => UnityChainloader.Instance.Plugins.Values.ToList().ConvertAll(plugin => (BaseUnityPlugin)plugin.Instance);
    }
}
