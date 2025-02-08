using BepInEx;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FarlandsCoreMod.Configuration
{
    public class Configurable
    {
        public BaseUnityPlugin plugin;
        private Dictionary<string, ConfigEntryBase> configurations = new();

        public Configurable(BaseUnityPlugin plugin)
        {
            this.plugin = plugin;
        }

        public void AddConfig<T>(string key, string description, T defaultValue)
        {
            var config = plugin.Config.Bind("FarlandsCoreMod", key, defaultValue, description);
            configurations.Add(key, config);
        }
        public ConfigEntryBase GetBase(string key) => GetBase(key);
        public object GetConfig(string key) => GetBase(key).BoxedValue;

        public T GetConfig<T>(string key) => (T)GetConfig(key);


        public List<ConfigEntryBase> GetConfigs() => configurations.Values.ToList();

        public void UpdateConfig<T>(string key, T value)
        {
            var config = GetBase(key);
            config.BoxedValue = value;
        }
    }
}
