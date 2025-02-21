using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx;
using FarlandsCoreMod.Utiles.AssetBundles;
using FarlandsCoreMod.Configuration;
using FarlandsCoreMod.UI.ConfigUI;
using System.Linq;

namespace FarlandsCoreMod
{
    [BepInDependency("magin.fcm", $"^{FCMInfo.Version}")]
    public abstract class AbstractMod : BaseUnityPlugin, IMod
    {
        public string AssetBundlePath;

        private AssetBundle resourceBundle;
        public AssetBundle ResourceBundle => resourceBundle;

        public string GUID => this.Info.Metadata.GUID;

        public void Awake()
        {
            resourceBundle = AssetBundle.LoadFromFile(Paths.Plugin + "/" + AssetBundlePath);
        }
        public void Start()
        {

        }

        public void Update()
        {

        }

        public void AddConfig<T>(string sectionKey, string description, T defValue)
        {
            var splt = sectionKey.Split('/');
            var section = splt.Length== 1 ? "" : splt[0];
            var key = splt.Length == 1 ? splt[0] : splt[1];
            CONFIG.Add(this, section, key, description, defValue);
        }
        public T GetConfig<T>(string sectionKey) => CONFIG.Get<T>(this, sectionKey);
        public void SetConfig<T>(string sectionKey, T value) => CONFIG.Set(this, sectionKey, value);

        public abstract Sprite LoadSprite(string path);

        public T LoadBundle<T>(string path) where T : UnityEngine.Object
        {
            throw new NotImplementedException();
        }

        public virtual void ConfigUI(ConfigUIMaker ui)
        {
            var Configs = CONFIG.GetConfigsBySection(this);
            List<string> sections = Configs.Keys.ToList();

            foreach (var section in sections)
            {
                ui.RenderSection(section);

                foreach (var config in Configs[section].Select(x=>$"{x.Definition.Section}/{x.Definition.Key}"))
                {
                    ui.RenderConfigs(config);
                }
            }
        }
    }
}
