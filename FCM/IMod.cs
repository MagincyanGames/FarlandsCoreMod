using FarlandsCoreMod.Utiles.AssetBundles;
using FarlandsCoreMod.Utiles.Sprites;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod
{
    public interface IMod : ISpriteLoader, IBundleLoader
    {
        public void Awake();
        public void Start();
        public void Update();
    }
}
