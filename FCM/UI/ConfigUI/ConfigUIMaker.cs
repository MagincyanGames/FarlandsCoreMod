using BepInEx;
using BepInEx.Configuration;
using FarlandsCoreMod.UI.Components;
using System;
using System.Collections.Generic;
using System.Text;
using static FarlandsCoreMod.Utiles.Sprites.SpriteManager.Sprites;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FarlandsCoreMod.Configuration;

namespace FarlandsCoreMod.UI.ConfigUI
{
    public class ConfigUIMaker : UIMaker
    {
        private BaseUnityPlugin mod;
        private GameObject basePoint;

        public void PointBase() => Point(basePoint);
        public ConfigUIMaker(IMod mod, RElement basePoint)
        {
            this.mod = (BaseUnityPlugin)mod;
            this.basePoint = basePoint.gameObject;
            PointBase();
        }
        public ConfigUIMaker(IMod mod, Component basePoint)
        {
            this.mod = (BaseUnityPlugin)mod;
            this.basePoint = basePoint.gameObject;
            PointBase();
        }
        public ConfigUIMaker(IMod mod, GameObject basePoint)
        {
            this.mod = (BaseUnityPlugin)mod;
            this.basePoint = basePoint;
        }

        public void RenderSection(string sectionName)
        {
            this.Point(basePoint);
            var sectionGroup = new RVerticalGroup
            {
                name = $"{sectionName}-vg",
                size = new Vector2(0.85f, 1),
                anchoredPosition = new Vector2(0, 0),
                spacing = 2,
                source = "magin.fcm:UI_31",
                //color = new Color(0, 0, 0, 0.5f)

            };
            this.RenderAndPoint(sectionGroup);

            this.Render(new RText()
            {
                name = $"{sectionName}-text",
                text = $"[{sectionName}]",
                size = new Vector2(45, 15),
                anchoredPosition = new Vector2(0, 0),
                fontSize = 7,
                verticalAlignment = TMPro.VerticalAlignmentOptions.Middle,
                horizontalAlignment = TMPro.HorizontalAlignmentOptions.Center,
            });

        }
        public void RenderConfig(string sectionKey)
        {
            var pnt = this.point;
            var config = CONFIG.GetConfigurable(mod).GetBase(sectionKey);
            string txt;
            TMP_InputField.ContentType type = TMP_InputField.ContentType.Standard;
            if (config.SettingType == typeof(int)) type = TMP_InputField.ContentType.IntegerNumber;
            if (config.SettingType == typeof(float)) type = TMP_InputField.ContentType.DecimalNumber;

            txt = config.Definition.Key;

            this.RenderAndPoint(new RHorizontalGroup
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

            this.Render(new RText()
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

                this.Render(new RCheckmark()
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
                this.RenderAndPoint(new RImage()
                {
                    source = "magin.fcm:UI_32",
                    size = new Vector2(50, 10),
                });
                this.Render(new RInputText()
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

            this.point = pnt;
        }

        public void RenderConfigs(params string[] sectionKeys) => Array.ForEach(sectionKeys, RenderConfig);
    }
}
