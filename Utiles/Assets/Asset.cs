using System;
using System.Collections.Generic;
using System.Text;

namespace FarlandsCoreMod.Utiles.Assets
{
    public class BundleAsset : GenericBundle
    {
        public BundleAsset()
        {
        }
        public BundleAsset(string path) : base(path)
        {
        }
        public BundleAsset(byte[] raw) : base(raw)
        {
        }

        public T GetPrefab<T>(string path) where T : UnityEngine.Object
        { 
            return bundle.LoadAsset<T>(path);
        }

        public T[] GetAllPrefabs<T>() where T : UnityEngine.Object
        {
            return bundle.LoadAllAssets<T>();
        }
    }
}
