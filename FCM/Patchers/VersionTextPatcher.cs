using Farlands;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FarlandsCoreMod.Patchers
{
    [HarmonyPatch(typeof(VersionText), "LoadVersionFromTextFile")]
    internal class VersionTextPatcher
    {
        [HarmonyPostfix]
        public static void VersionTextPostFix(ref VersionText __instance, ref string __result)
        {
            string version = FCMInfo.Version;
            string[] versionParts = version.Split('.');
            if (versionParts.Length == 4)
            {
                version = string.Join('.', versionParts.Take(3));
            }

            // Agregar la versión modificada al resultado
            __result += " · FCM " + version;
        }
    }
}
