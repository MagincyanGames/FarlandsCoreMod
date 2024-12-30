using BepInEx;
using BepInEx.Configuration;
using System.Collections.Generic;

namespace FarlandsCoreMod
{
    [BepInPlugin("top.magincian.fcm", "FarlandsCoreMod", FCMInfo.Version)]
    public class FarlandsCoreMod : BaseUnityPlugin
    {
        private ConfigEntry<bool> debug_skipIntro;
        private ConfigEntry<bool> debug_quitEarlyAccessScreen;
        public static FarlandsCoreMod Instance;

        public static List<FarlandsMod> ModList = new();
        public string SHORT_NAME => "FCM";

        private void prepareLoadding()
        {
            
        }

        private void LoadBundles()
        {

        }

        private void Awake()
        {

        }
    }
}
