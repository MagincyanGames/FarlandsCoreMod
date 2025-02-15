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
using CommandTerminal;
using PixelCrushers.DialogueSystem.Articy.Articy_1_4;
using FarlandsCoreMod.Utiles.AssetBundles;

namespace FarlandsCoreMod
{
    [BepInPlugin("magin.fcm", "FarlandsCoreMod", FCMInfo.Version)]
    public class FarlandsCoreMod : BaseUnityPlugin, IMod
    {
        public static FarlandsCoreMod Instance;
        public static BepInPlugin Metadata => Instance.Info.Metadata;
        public AssetBundle ResourceBundle = AssetBundle.LoadFromFile(Paths.Plugin + "/fcm_bundle");
        public Harmony harmony = new Harmony("magin.fcm");

        public void Awake()
        {
            CONFIG.Add(this,"Debug","SkipIntro", "If true, the intro will be skipped", false);
            harmony.PatchAll();
            Instance = this;

            SceneLoader(typeof(FarlandsCoreMod), typeof (MainMenuScene));
        }

        public void Start()
        {
            this.AddComponent<Terminal>();
        }

        public void Update()
        {
        }

        Coroutine onjandu = null;
        [OnLoadScene("JanduSoftLogoScene")]
        public static void OnJanduSoftScene()
        {
            
            Instance.Logger.LogInfo("JANDUUUU");
            if(CONFIG.Get<bool>("magin.fcm:Debug/SkipIntro"))
                SceneManager.LoadScene("PreloadScene");
            else Instance.onjandu  = Instance.StartCoroutine(Instance.onJanduSoft());
        }
        [OnUnloadScene("JanduSoftLogoScene")]
        public static void OnJanduSoftSceneUnload()
        {
            Instance.Logger.LogInfo("SOFTTTTT");

            if (!CONFIG.Get<bool>("magin.fcm:Debug/SkipIntro"))
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


        public List<PluginInfo> LoadedMods => UnityChainloader.Instance.Plugins.Values.Where(x => x.Metadata.GUID != Metadata.GUID).ToList();

        AssetBundle IBundleLoader.ResourceBundle => throw new NotImplementedException();

        public BaseUnityPlugin GetPlugin(string guid) => (BaseUnityPlugin) UnityChainloader.Instance.Plugins[guid].Instance;
        public T LoadBundle<T>(string path) where T : UnityEngine.Object
        {
            return ResourceBundle.LoadAsset<T>(path);
        }
        public Sprite LoadSprite(string path)
        {
            return ResourceBundle.LoadAsset<Sprite>(path);
        }
        public void SceneLoader(SceneLoader sceneLoader)
        {
            sceneLoader.Load();
        }
        public void SceneLoader(params List<Type> types)
        {
            types.ForEach(x => SceneLoader(new SceneLoader(x)));
        }
        public void OnLoadScene(Action<Scene, LoadSceneMode> action)
        { 
            SceneManager.sceneLoaded += new (action);
        }

        
    }
}
