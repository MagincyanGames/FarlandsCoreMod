using BepInEx;
using BepInEx.Unity.Bootstrap;
using FMOD;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarlandsCoreMod.Utiles
{
    public class Path
    {
        public BaseUnityPlugin mod;
        public string path;
        public Path(BaseUnityPlugin mod, string path)
        {
            this.mod = mod;
            this.path = path;
        }

        public Path(string path)
        {
            var splited = path.Split(':');

            if (splited.Length == 1)
            {
                this.mod = null;
                this.path = path;
            }
            else 
            {
                this.mod = (BaseUnityPlugin) UnityChainloader.Instance.Plugins[splited[0].Trim()].Instance;
                this.path = splited[1].Trim();
            }
        }

        public string name
        {
            get
            {
                if (mod == null) return path;
                return $"{mod.Info.Metadata.GUID}:{path}";
            } 
        }
    }
}
