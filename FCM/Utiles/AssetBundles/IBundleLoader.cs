using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.Utiles.AssetBundles
{
    public interface IBundleLoader
    {
        public AssetBundle ResourceBundle { get; }
        public T LoadBundle<T>(string path) where T : UnityEngine.Object;

    }
}
