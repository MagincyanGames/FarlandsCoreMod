using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.Bootstrap;
using CommandTerminal;
using Farlands.Dev;
using Farlands.PlaceableObjectsSystem;
using FarlandsCoreMod.Attributes;
using FarlandsCoreMod.Patchers;
using FarlandsCoreMod.Utiles;
using FarlandsCoreMod.Utiles.Assets;
using FarlandsCoreMod.Utiles.Loaders;
using FMOD.Studio;
using HarmonyLib;
using JanduSoft;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

namespace FarlandsCoreMod
{
    [BepInPlugin("top.magincian.fcm", "FarlandsCoreMod", FCMInfo.Version)]
    public class FarlandsCoreMod : BaseUnityPlugin
    {
        private static ConfigEntry<bool> debug_skipIntro;
        public static bool Debug_skipIntro => debug_skipIntro.Value;
        private static ConfigEntry<bool> debug_quitEarlyAccessScreen;
        public static bool Debug_quitEarlyAccessScreen => debug_quitEarlyAccessScreen.Value;
        public static FarlandsCoreMod instance;

        public static List<FarlandsMod> ModList = new();
        public string SHORT_NAME => "FCM";

        private void prepareLoadding()
        {
            
            // JanduSoft.Singleton<JSManager>.Instance.sceneToLoad = SceneUtility.GetScenePathByBuildIndex();
        }

        private BundleAsset fcm_assets;
        private BundleScene fcm_scenes;
        private void Awake()
        { 
            fcm_assets = new BundleAsset(Properties.Resources.fcm);
            fcm_scenes = new BundleScene(Properties.Resources.fcm_scenes);

            instance = this;

            fcm_scenes.Load();
            fcm_assets.Load();
            fcm_scenes.LoadScene("LoaddingScene");

            this.gameObject.AddComponent<Terminal>();

            debug_skipIntro = AddConfig("Debug", "SkipIntro", 
                "If true the intro will be skipped", false);

            debug_quitEarlyAccessScreen = AddConfig("Debug", "QuitEarlyAccessScreen", 
                "If true the Early Access Screen will be removed", false);

            Logger.LogInfo($"Plugin {this.Info.Metadata.GUID} is loaded!");
            
            Patcher.LoadAll();

            OnLoadScene.onLoadScene();

            StartCoroutine(allLoaded());
        }

        private IEnumerator LoadManagers()
        {
            var managers = Assembly.GetAssembly(this.GetType())
                .GetTypes().Where(x => typeof(IManager).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract)
                .Select(x => (Activator.CreateInstance(x) as IManager))
                .ToList();

            IComparer<IManager> comparer = Comparer<IManager>.Create((x,y)=>x.Index.CompareTo(y.Index));
            managers.Sort(comparer);

            foreach (var m in managers)
            {
                Debug.Log("MANAGER " + m.GetType().Name + " LOADDING");
                if (m is IManagerASM imasm) imasm.SetASM(ModList.Select(m => m.ASM));
                m.Init();
                yield return null;
            }
        }

        private static bool isLoaded = false;
        private IEnumerator allLoaded()
        {
            yield return new WaitForEndOfFrame();
            yield return StartCoroutine(OnAllModsLoaded());
            yield return LoadManagers();
            yield return SceneManager.LoadSceneAsync("PreloadScene");

            fcm_scenes.Unload();
            fcm_assets.Unload();
            
            isLoaded = true;
        }

        public static bool IsAllLoaded() => isLoaded;
        public static ConfigEntry<T> AddConfig<T>(string section, string key, string description, T defaultValue) =>
            instance.Config.Bind(section, key, defaultValue, description);
        private IEnumerator OnAllModsLoaded()
        {
            Logger.LogMessage("************************");
            var target = "top.magincian.fcm";

            foreach(var plugin in UnityChainloader.Instance.Plugins.Values)
            {
                if (plugin.Dependencies.Any(d => d.DependencyGUID == target))
                {
                    Logger.LogMessage($"{plugin.Metadata.GUID}: {plugin.Metadata.Version}");

                }
                yield return null;
            }

            Logger.LogMessage("************************");
            yield return null;
        }
    }
}
