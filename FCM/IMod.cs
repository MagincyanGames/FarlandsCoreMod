using FarlandsCoreMod.UI.ConfigUI;
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
        public string GUID { get; }
        public void ConfigUI(ConfigUIMaker ui);
        public void Awake();
        public void Start();
        public void Update();
    }
}
