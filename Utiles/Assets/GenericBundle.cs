using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.Utiles.Assets
{
    public abstract class GenericBundle
    {
        public AssetBundle bundle;

        public bool isRaw = false;
        public string path;
        public byte[] raw;
        public GenericBundle() { }
        public GenericBundle(string path)
        {
            isRaw = false;
            this.path = path;
        }
        public GenericBundle(byte[] raw) 
        { 
            isRaw=true;
            this.raw = raw;

        }

        public void Load()
        {
            if (isRaw)
            {
                bundle = AssetBundle.LoadFromMemory(raw);
            }
            else
            {
                bundle = AssetBundle.LoadFromFile(path);
            }
        }

        public void Unload(bool unloadAll = false)
        {
            bundle.Unload(unloadAll);
        }


    }
}
