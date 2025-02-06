using BepInEx;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.Utiles.Sprites
{
    public static class SpriteManager
    {
        public static class Sprites
        {
            public static class UI
            { 
                public static Path UI_FM = new("magin.fcm:UI_FM");
            }
        }

        public static Sprite Load(Path path)
        {
            return Load(path.mod, path.path);
        }

        public static Sprite Load(BaseUnityPlugin Mod, string path)
        {
            if (Mod == null){
                if (path.StartsWith("$")) return Resources.InstanceIDToObject(int.Parse(path.Substring(1))) as Sprite;
                return Resources.Load<Sprite>(path); 
            }
            if (Mod is ISpriteLoader spriteLoader) return spriteLoader.Load(path);

            return null;
        }
    }
}
