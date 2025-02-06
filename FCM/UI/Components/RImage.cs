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
        public override Component Render()
        {
            var gameObject = RectRender().gameObject;
            Image img = gameObject.TryAddComponent<Image>();

            img.sprite = SpriteManager.Load(source);

            return img;
        }
    }
}
