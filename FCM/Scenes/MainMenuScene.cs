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
using FarlandsCoreMod.Configuration;
using SuperTiled2Unity;
using TMPro;

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

                var group = new RVerticalGroup
                {
                    name = $"{mod.Info.Metadata.Name}-group",
                    source = "magin.fcm:UI_31",
                    spacing = 1,
                };

                ui.RenderAndPoint(group);
                ui.Render(new RText()
                {
                    size = new Vector2(105, 15),
                    name = $"{mod.Info.Metadata.Name}-text",
                    text = $"<b>{mod.Info.Metadata.Name}</b>",
                    fontSize = 8,
                    anchoredPosition = new Vector2(0, 0),
                    verticalAlignment = TMPro.VerticalAlignmentOptions.Middle,
                    horizontalAlignment = TMPro.HorizontalAlignmentOptions.Center,
                });

                foreach (var config in CONFIG.GetConfigs(mod))
                {
                    ui.Point(group.gameObject);
                    string txt;
                    TMP_InputField.ContentType type = TMP_InputField.ContentType.Standard;
                    if (config.SettingType == typeof(int)) type = TMP_InputField.ContentType.IntegerNumber;
                    if (config.SettingType == typeof(float)) type = TMP_InputField.ContentType.DecimalNumber;

                    if (config.Definition.Section.IsEmpty())
                        txt = config.Definition.Key;
                    else txt = $"[{config.Definition.Section }]"+ "\n" + config.Definition.Key;
                    ui.RenderAndPoint(new RHorizontalGroup
                    {
                        name = $"{config.Definition}-hg",
                        size = new Vector2(105, 30),
                        anchoredPosition = new Vector2(0, 0),
                        spacing = 5,
                        source = null

                    });

                    Func<string, object> caster = (string s) =>
                    {
                        if (type == TMP_InputField.ContentType.IntegerNumber)
                        {
                            return int.Parse(s);
                        }
                        return s;
                    };

                    ui.Render(new RText()
                    {
                        name = $"{txt}-text",
                        text = txt,
                        size = new Vector2(30, 15), 
                        fontSize = 7,
                        verticalAlignment = TMPro.VerticalAlignmentOptions.Middle,
                        horizontalAlignment = TMPro.HorizontalAlignmentOptions.Center,
                    });
                    

                    if (config.SettingType == typeof(bool)) ui.Render(new RCheckmark()
                    {
                        name = $"{txt}-it",
                        //fontSize = 10,
                        size = new Vector2(15, 15),
                        OnReload = go => go.GetComponent<Toggle>().isOn = (bool)config.BoxedValue,
                        OnEnable = go => go.GetComponent<UIMakerElementComponent>().element.Reload(),
                        onValueChanged = b => {

                            config.BoxedValue = b;
                            Debug.Log(config.BoxedValue);
                        },
                    });
                    else 
                    {
                        ui.RenderAndPoint(new RImage()
                        {
                            source = "magin.fcm:UI_32",
                            size = new Vector2(50, 15),
                        });
                        ui.Render(new RInputText()
                        {
                            name = $"{txt}-it",
                            text = config.BoxedValue.ToString(),
                            fontSize = 8,
                            OnReload = go => go.GetComponent<TMP_InputField>().text = config.BoxedValue.ToString(),
                            OnEnable = go => go.GetComponent<UIMakerElementComponent>().element.Reload(),
                            onEndEdit = s =>
                            {
                                config.BoxedValue = caster(s);
                                Debug.Log(config.BoxedValue);
                            },
                            contentType = type,
                            //fontSize = 10,
                            size = new Vector2(50, 15)
                        });
                    }
                }
            }
        }
    }
}
