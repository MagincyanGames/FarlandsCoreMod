using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.Utiles.Sprites
{
    public interface ISpriteLoader
    {
        public Sprite LoadSprite(string path);
    }
}
