using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.Utiles.Sprites
{
    internal interface ISpriteLoader
    {
        public Sprite Load(string path);
    }
}
