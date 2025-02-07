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

namespace FarlandsCoreMod
{
    [BepInPlugin("magin.fcm", "FarlandsCoreMod", FCMInfo.Version)]
    public class FarlandsCoreMod : BaseUnityPlugin, ISpriteLoader
    {
        private ConfigEntry<bool> debug_skipIntro;

        public static FarlandsCoreMod Instance;
        public static BepInPlugin Metadata => Instance.Info.Metadata;
        public AssetBundle ResourceBundle { get; private set; }
        public Harmony harmony = new Harmony("magin.fcm");

        public void Awake()
        {
            harmony.PatchAll();
            Instance = this;
            ResourceBundle = AssetBundle.LoadFromFile(Paths.Plugin + "/fcm_bundle");
            ResourceBundle.GetAllAssetNames().ToList().ForEach(Debug.Log);

            SceneLoader(new(typeof(FarlandsCoreMod)));

            OnLoadScene((scene, mode) =>
            {
                if (scene.name == "MainMenu")
                {

                    UIMaker ui = new(new()
                    {
                        baseGameObject = GameObjects.Find("MainMenu", "Canvas")
                    });
                        ui.Point("MainMenu:Canvas/MenuSpace/MainMenu");
                            ui.Open(new RButton() 
                            {
                                anchoredPosition = new Vector2(-4.8f, -60f),
                                size = new Vector2(30,30),
                                anchorMax = new Vector2(1, 0.5f),
                                anchorMin = new Vector2(1, 0.5f),
                                offsetMax = new Vector2(-4.8f, -94.1f),
                                offsetMin = new Vector2(-34.8f, -109.1f),
                                pivot = new Vector2(1, 0.5f),
                                source = SpriteManager.Sprites.UI.UI_FM,
                                onClick = () => Application.OpenURL("https://discord.gg/Uw42AhwygN")

                            }).Close();
                        ui.Close();

                    ui.End();
                }
            });

        }

        [OnLoadScene("JanduSoftLogoScene")]
        public static void OnJanduSoftScene()
        {
            //UIMaker ui = new(new()
            //{
            //    renderMode = RenderMode.ScreenSpaceOverlay,
            //    pixelPerfect = true,
            //    scaleFactor = 4.5f,
            //    renderOrder = 1000,

            //    CanvasScaler_scaleFactor = 3.23f,
            //    CanvasScaler_uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize,
            //    CanvasScaler_screenMatchMode = CanvasScaler.ScreenMatchMode.Expand,
            //    CanvasScaler_referencePixelPerUnit = 24,
            //    CanvasScaler_referenceResolution = new Vector2(320, 240),
            //});

            //ui.Open(new RButton()
            //{
            //    name = "Logo",
            //    source = new("magin.fcm:UI_29"),
            //    position = new Vector2(0, -90),
            //    size = new Vector2(48, 16),
            //    onClick = () => SceneManager.LoadScene("MainMenu")
            //}).Close().Close();

            //ui.End();
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
