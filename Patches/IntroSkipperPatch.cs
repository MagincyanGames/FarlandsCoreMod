using System;
using System.Collections;
using System.Linq;
using FarlandsCoreMod.Utils;
using HarmonyLib;
using JanduSoft;
using JanduSoft.Intro;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace FarlandsCoreMod.Patches;

[HarmonyPatch(typeof(IntroJanduSoft))]
[HarmonyPatch("PreLoadNextScene")]
public class IntroSkipperPatch
{
    static bool Prefix(IntroJanduSoft __instance, ref IEnumerator __result)
    {
        var skipp = FCM.Instance.Config.Bind("IntroSkipper", "enable", false);

        if (skipp.Value)
        {
            __result = NewCoroutine(__instance);
            return false; // Skip
        }

        return true;
    }

    static IEnumerator NewCoroutine(IntroJanduSoft instance)
    {
        Debug.Log("Prev Scene");
        var scene = instance.GetFieldValue<string>("nextScene");
        Debug.Log("Post Scene");
        while (!Singleton<JSManager>.Instance.finishedLoadingBundle || !Singleton<DataManager>.Instance.IsInitialized())
        {
            yield return new WaitForEndOfFrame();
        }

        AsyncOperation loadNextScene = SceneManager.LoadSceneAsync(scene);
        yield return new WaitForEndOfFrame();
        loadNextScene.allowSceneActivation = true;
    }
}
