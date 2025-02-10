using FarlandsCoreMod.UI.Components;
using FarlandsCoreMod.UI;
using FarlandsCoreMod.Utiles.Sprites;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using FarlandsCoreMod.Utiles.GameObjects;
using UnityEngine.UI;
using FarlandsCoreMod.Utiles;

namespace FarlandsCoreMod.Scenes
{
    public class MainMenuScene
    {
        [OnLoadScene("MainMenu")]
        public static void OnLoadMainMenu()
        {
            UIMaker ui = new();
            ui.Point("MainMenu:Canvas/MenuSpace/MainMenu");
            ui.Render(new RButton()
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

            });
            ui.Point("MainMenu:Canvas/Settings");
            ui.Render(new RScrollView()
            {
                name = "ScrollView",
                horizontal = false,
                vertical = true,
                movementType = ScrollRect.MovementType.Clamped,
                elasticity = 0.1f,
                inertia = true,
                decelerationRate = 0.135f,
                scrollSensitivity = 10f,
                size = new Vector2(150, 125),
                anchoredPosition = new Vector2(204, 16),
                source = "magin.fcm:UI_24"
            });
            foreach(var mod in ModManager.mods)
            {
                ui.Point("MainMenu:Canvas/Settings/ScrollView/Viewport/Content");
                ui.RenderAndPoint(new RImage()
                {
                    size = new Vector2(105, 25),
                    source = "magin.fcm:UI_36"
                });
                ui.Render(new RText()
                {
                    size = new Vector2(105, 25),
                    name = $"{mod.Info.Metadata.Name}-text",
                    text = mod.Info.Metadata.Name,
                    fontSize = 8,
                    anchoredPosition = new Vector2(0, 2.5f),
                    verticalAlignment = TMPro.VerticalAlignmentOptions.Middle,
                    horizontalAlignment = TMPro.HorizontalAlignmentOptions.Center,
                });
            }
        }
    }
}
