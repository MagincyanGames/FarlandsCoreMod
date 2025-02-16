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
using Unity.VisualScripting.FullSerializer;
using static System.Net.Mime.MediaTypeNames;
using Application = UnityEngine.Application;
using BepInEx;
using static FarlandsCoreMod.Utiles.Sprites.SpriteManager.Sprites;
using System.Linq;
using BepInEx.Configuration;
using static System.Collections.Specialized.BitVector32;
using static Unity.VisualScripting.Member;
using System.Drawing;

namespace FarlandsCoreMod.Scenes
{
    public class MainMenuScene
    {
        private static UIMaker ui;
        private static BaseUnityPlugin selectedMod;

        [OnLoadScene("MainMenu")]
        public static void OnLoadMainMenu()
        {
            ui = new();
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
            ui.RenderAndPoint(new RImage()
            {
                name = "Mods",
                size = new Vector2(150, 100),
                anchoredPosition = new Vector2(205, 12),
                source = "magin.fcm:UI_31",
            });

            renderContent();

            ui.Point("MainMenu:Canvas/Settings/Mods");
            ui.Render(Sidebar);
            
            ModManager.mods.ForEach(rederForMod);
        }

        private static void rederForMod(BaseUnityPlugin mod)
        {
            ui.Point(Sidebar);
            Debug.Log(mod.Info.Metadata.GUID);

            ui.RenderAndPoint(new RButton()
            {
                size = new Vector2(105, 15),
                source = "magin.fcm:UI_31",
                onClick = () =>
                {
                    selectedMod = mod;
                    renderContent();
                }
            });
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

            // CONFIG.GetConfigsBySection(mod).ToList().ForEach(renderForSection);

        }
        private static RVerticalGroup sectionGroup;
        private static void renderContent()
        {
            ui.Point("MainMenu:Canvas/Settings/Mods");
            
            ui.RenderAgainAndPoint(ContentConteiner);
            if (selectedMod != null){
                Debug.Log(selectedMod.Info.Metadata.GUID);
                CONFIG.GetConfigsBySection(selectedMod).ToList().ForEach(renderForSection);
            }

            else Debug.Log("NULL");


        }
        private static void renderForSection(KeyValuePair<string, List<ConfigEntryBase>> section)
        {
            Debug.Log(section.Key);
            ui.Point(ContentConteiner);
            sectionGroup = new RVerticalGroup
            {
                name = $"{section.Key}-vg",
                size = new Vector2(0.85f, 1),
                anchoredPosition = new Vector2(0, 0),
                spacing = 2,
                source = "magin.fcm:UI_31",
                //color = new Color(0, 0, 0, 0.5f)

            };
            ui.RenderAndPoint(sectionGroup);

            ui.Render(new RText()
            {
                name = $"{section.Key}-text",
                text = $"[{section.Key}]",
                size = new Vector2(30, 15),
                anchoredPosition = new Vector2(0, 0),
                fontSize = 7,
                verticalAlignment = TMPro.VerticalAlignmentOptions.Middle,
                horizontalAlignment = TMPro.HorizontalAlignmentOptions.Center,
            });

            section.Value.ForEach(renderForConfig);
        }

        private static void renderForConfig(ConfigEntryBase config)
        {
            ui.Point(sectionGroup.gameObject);
            string txt;
            TMP_InputField.ContentType type = TMP_InputField.ContentType.Standard;
            if (config.SettingType == typeof(int)) type = TMP_InputField.ContentType.IntegerNumber;
            if (config.SettingType == typeof(float)) type = TMP_InputField.ContentType.DecimalNumber;

            txt = config.Definition.Key;

            ui.RenderAndPoint(new RHorizontalGroup
            {
                name = $"{config.Definition}-hg",
                size = new Vector2(105, 20),
                anchoredPosition = new Vector2(0, 0),
                spacing = 5,
                childExpandHeight = false,
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
                size = new Vector2(30, 10),
                fontSize = 7,
                verticalAlignment = TMPro.VerticalAlignmentOptions.Middle,
                horizontalAlignment = TMPro.HorizontalAlignmentOptions.Center,
            });



            if (config.SettingType == typeof(bool))
            {

                ui.Render(new RCheckmark()
                {
                    name = $"{txt}-it",
                    //fontSize = 10,
                    size = new Vector2(10, 10),
                    OnReload = go => go.GetComponent<Toggle>().isOn = (bool)config.BoxedValue,
                    OnEnable = go => go.GetComponent<UIMakerElementComponent>().element.Reload(),
                    onValueChanged = b =>
                    {

                        config.BoxedValue = b;
                        Debug.Log(config.BoxedValue);
                    },
                });
            }
            else
            {
                ui.RenderAndPoint(new RImage()
                {
                    source = "magin.fcm:UI_32",
                    size = new Vector2(50, 10),
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
                    size = new Vector2(50, 10)
                });
            }
        }

        private static RScrollView Sidebar = new RScrollView()
        {
            name = "Sidebar",
            horizontal = false,
            vertical = true,
            movementType = ScrollRect.MovementType.Clamped,
            elasticity = 0.1f,
            inertia = true,
            decelerationRate = 0.135f,
            scrollSensitivity = 10f,
            size = new Vector2(50, 100),
            anchoredPosition = new Vector2(-50, 0),
            source = "magin.fcm:UI_24"
        };

        private static RImage ContentConteiner = new RImage()
        {
            name = "ContentConteiner",
            size = new Vector2(100, 100),
            anchoredPosition = new Vector2(25, 0),
            source = "magin.fcm:UI_31",
        };
    }
}
