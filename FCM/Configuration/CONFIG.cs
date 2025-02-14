using BepInEx;
using BepInEx.Configuration;
using FarlandsCoreMod.Utiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarlandsCoreMod.Configuration
{
    public static class CONFIG
    {
        public static Dictionary<string, Configurable> Configurables = new();
        
        public static Configurable GetConfigurable(BaseUnityPlugin plugin)
        {
            if (!Configurables.ContainsKey(plugin.Info.Metadata.GUID))
            {
                Configurables.Add(plugin.Info.Metadata.GUID, new Configurable(plugin));
            }

            return Configurables[plugin.Info.Metadata.GUID];
        }

        public static object Get(BaseUnityPlugin mod, string key) => GetConfigurable(mod).GetConfig(key);
        public static object Get(Path config) => Get(config.mod, config.path);

        public static T Get<T>(BaseUnityPlugin mod, string key) => GetConfigurable(mod).GetConfig<T>(key);
        public static T Get<T>(Path config) => Get<T>(config.mod, config.path);

        public static void Set(BaseUnityPlugin mod, string key, object value) => GetConfigurable(mod).UpdateConfig(key, value);
        public static void Set(Path config, object value) => Set(config.mod, config.path, value);

        public static void Add<T>(BaseUnityPlugin mod, string section, string key, string description, T defaultValue) => GetConfigurable(mod).AddConfig(section, key, description, defaultValue);
        public static void Add<T>(Path path, string description, T defaultValue)
        {
            var splt = path.path.Split('/');
            if(splt.Length == 1)
                Add(path.mod, "", path.path, description, defaultValue);
            else Add(path.mod, splt[0].Trim(), splt[1].Trim(), description, defaultValue);
        }

        public static List<ConfigEntryBase> GetConfigs(BaseUnityPlugin plugin) => GetConfigurable(plugin).GetConfigs();
        public static Dictionary<string, List<ConfigEntryBase>> GetConfigsBySection(BaseUnityPlugin plugin) => GetConfigurable(plugin).GetConfigsBySection();

    }
}
