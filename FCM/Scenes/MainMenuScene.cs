using FarlandsCoreMod.UI.Components;
using FarlandsCoreMod.UI;
using FarlandsCoreMod.Utiles.Sprites;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using FarlandsCoreMod.Utiles.GameObjects;
using UnityEngine.UI;

namespace FarlandsCoreMod.Scenes
{
    public class MainMenuScene
    {
        [OnLoadScene("MainMenu")]
        public static void OnLoadMainMenu()
        {
            var optionButton = GameObjects.Find("MainMenu:Canvas/MenuSpace/MainMenu/MenuWindow/Menu/MainMenuButton (1)").GetComponent<Button>();
            optionButton.onClick.AddListener(RenderOptionMenu);
            UIMaker ui = new(new()
            {
                baseGameObject = GameObjects.Find("MainMenu", "Canvas")
            });
            ui.Point("MainMenu:Canvas/MenuSpace/MainMenu");
            ui.Open(new RButton()
            {
                anchoredPosition = new Vector2(-4.8f, -60f),
                size = new Vector2(30, 30),
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
        public static void RenderOptionMenu()
        {
            UIMaker ui = new(new()
            {
                baseGameObject = GameObjects.Find("MainMenu", "Canvas")
            });
            ui.Point("MainMenu:Canvas/Settings");
            ui.Open(new RImage()
            {
                size = new Vector2(150, 125),
                anchoredPosition = new Vector2(204, 16),
                source = "magin.fcm:UI_24"
            });
            ui.End();
        }
    }
}
