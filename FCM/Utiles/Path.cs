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
                this.mod = ModManager.GetMod(splited[0].Trim());
                this.path = splited[1].Trim();
            }
        }

        public static implicit operator string(Path path)
        {
            return path.path;
        }
        public static implicit operator Path(string path)
        {
            return new Path(path);
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
