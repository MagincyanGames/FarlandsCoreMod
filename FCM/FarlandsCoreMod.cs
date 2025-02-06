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

namespace FarlandsCoreMod
{
    [BepInPlugin("magin.fcm", "FarlandsCoreMod", FCMInfo.Version)]
    public class FarlandsCoreMod : BaseUnityPlugin, ISpriteLoader
    {
        private ConfigEntry<bool> debug_skipIntro;
        private ConfigEntry<bool> debug_quitEarlyAccessScreen;
        public static FarlandsCoreMod Instance;
        public static BepInPlugin Metadata => Instance.Info.Metadata;
        public AssetBundle ResourceBundle { get; private set; }

        public void Awake()
        {
            Instance = this;
            ResourceBundle = AssetBundle.LoadFromFile(Paths.Plugin + "/fcm_bundle");
            ResourceBundle.GetAllAssetNames().ToList().ForEach(Debug.Log);

            var ui_29 = ResourceBundle.LoadAsset("UI_29");

            OnLoadScene((scene, mode) =>
            {
                if (scene.name == "JanduSoftLogoScene")
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

                    //ui.Open(new RImage()
                    //{
                    //    name = "Logo",
                    //    source = SpriteManager.Sprites.UI.UI_29,
                    //    size = new Vector2(200, 100),
                    //}).Close().Close();

                    //ui.End();

                }
                else if (scene.name == "MainMenu")
                {
                    UIMaker ui = new(new()
                    {
                        baseGameObject = GameObjects.Find("MainMenu", "Canvas")
                    });
                        ui.Point("MainMenu:Canvas/MenuSpace/MainMenu");
                            ui.Open(new RImage() 
                            {
                                anchoredPosition = new Vector2(-4.8f, -60f),
                                size = new Vector2(30,30),
                                anchorMax = new Vector2(1, 0.5f),
                                anchorMin = new Vector2(1, 0.5f),
                                offsetMax = new Vector2(-4.8f, -94.1f),
                                offsetMin = new Vector2(-34.8f, -109.1f),
                                pivot = new Vector2(1, 0.5f),
                                source = SpriteManager.Sprites.UI.UI_FCM,

                            }).Close();
                        ui.Close();

                    ui.End();
                }
            });

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

        public void OnLoadScene(Action<Scene, LoadSceneMode> action)
        { 
            SceneManager.sceneLoaded += new (action);
        }
    }
}
