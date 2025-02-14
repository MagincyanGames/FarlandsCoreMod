using FarlandsCoreMod.Extensors;
using FarlandsCoreMod.Utiles;
using FarlandsCoreMod.Utiles.Sprites;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using Image = UnityEngine.UI.Image;

namespace FarlandsCoreMod.UI.Components
{
    public class RImage : RRect
    {
        public override string type => "image";
        public Utiles.Path source;
        public Color? color;
        public override Component Render()
        {
            base.Render();
            Sprite sprite = null;
            try
            {
                sprite = SpriteManager.Load(source);
            }
            catch 
            {
            }

            if (sprite != null)
            {
                Image img = gameObject.TryAddComponent<Image>();
                img.sprite = sprite;
                if (color.HasValue) img.color = color.Value;
            }

            return gameObject.GetComponent<RectTransform>();

        }
    }
}
