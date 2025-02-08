using BepInEx;
using BepInEx.Configuration;
using System.Collections.Generic;
using BepInEx.Unity.Bootstrap;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using FarlandsCoreMod.Utiles.Sprites;
using System;
using JetBrains.Annotations;
using FarlandsCoreMod.UI;
using FarlandsCoreMod.UI.Components;
using FarlandsCoreMod.Utiles.GameObjects;
using UnityEngine.UI;
using HarmonyLib;
using FarlandsCoreMod.Scenes;
using BepInEx.Logging;
using Unity.VisualScripting;
using FarlandsCoreMod.Configuration;
using FarlandsCoreMod.Utiles;

namespace FarlandsCoreMod
{
    [BepInPlugin("magin.fcm", "FarlandsCoreMod", FCMInfo.Version)]
    public class FarlandsCoreMod : BaseUnityPlugin, ISpriteLoader
    {
        public static FarlandsCoreMod Instance;
        public static BepInPlugin Metadata => Instance.Info.Metadata;
        public AssetBundle ResourceBundle { get; private set; }
        public Harmony harmony = new Harmony("magin.fcm");

        public void Awake()
        {
            CONFIG.Add(this, "test", "Test", 0);

            harmony.PatchAll();
            Instance = this;
            ResourceBundle = AssetBundle.LoadFromFile(Paths.Plugin + "/fcm_bundle");

            SceneLoader(new(typeof(FarlandsCoreMod), typeof (MainMenuScene)));
        }

        

        Coroutine onjandu = null;
        [OnLoadScene("JanduSoftLogoScene")]
        public static void OnJanduSoftScene()
        {
            Instance.Logger.LogInfo("JANDUUUU");
            Instance.onjandu  = Instance.StartCoroutine(Instance.onJanduSoft());
        }
        [OnUnloadScene("JanduSoftLogoScene")]
        public static void OnJanduSoftSceneUnload()
        {
            Instance.Logger.LogInfo("SOFTTTTT");
            Instance.StopCoroutine(Instance.onjandu);
        }

        private IEnumerator onJanduSoft()
        {
            Instance.Logger.LogInfo("OWO");
            while (true)
            {
                yield return new WaitForEndOfFrame();
                if (!string.IsNullOrEmpty(Input.inputString))
                {
                    Instance.Logger.LogInfo("unu");
                    SceneManager.LoadScene("PreloadScene");
                }
            }
        }

        public void Start()
        { 
        }

        public List<PluginInfo> LoadedMods => UnityChainloader.Instance.Plugins.Values.Where(x => x.Metadata.GUID != Metadata.GUID).ToList();

        public BaseUnityPlugin GetPlugin(string guid) => (BaseUnityPlugin) UnityChainloader.Instance.Plugins[guid].Instance;

        public Sprite Load(string path)
        {
            return ResourceBundle.LoadAsset<Sprite>(path);
        }
        public void SceneLoader(SceneLoader sceneLoader)
        {
            sceneLoader.Load();
        }

        public void OnLoadScene(Action<Scene, LoadSceneMode> action)
        { 
            SceneManager.sceneLoaded += new (action);
        }
    }
}
