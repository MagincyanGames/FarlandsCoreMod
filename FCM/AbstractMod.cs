using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx;
using FarlandsCoreMod.Utiles.AssetBundles;

namespace FarlandsCoreMod
{
    [BepInDependency("magin.fcm", $"^{FCMInfo.Version}")]
    public abstract class AbstractMod : BaseUnityPlugin, IMod
    {
        public string AssetBundlePath;

        private AssetBundle resourceBundle;
        public AssetBundle ResourceBundle => resourceBundle;

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

        public abstract Sprite LoadSprite(string path);

        public T LoadBundle<T>(string path) where T : UnityEngine.Object
        {
            throw new NotImplementedException();
        }
    }
}
