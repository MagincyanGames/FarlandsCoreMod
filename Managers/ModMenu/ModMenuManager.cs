using FarlandsCoreMod.ui;
using FarlandsCoreMod.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FarlandsCoreMod.Managers.ModMenu;

public class ModMenuManager : Manager
{
    public override void OnSceneChange(Scene oldScene, Scene newScene)
    {
        Debug.Log($"New Scene {newScene.name}");
        if (newScene.name == "MainMenu")
        {
            var uiParent = newScene.GetGameObjectByPath("Canvas/Settings/MenuSettings").transform;

            var uiGameObject = new GameObject("Mods");
            uiGameObject.transform.SetParent(uiParent);
            uiGameObject.transform.localPosition = Vector3.zero;
            uiGameObject.AddComponent<ModsConfigUI>();
        }
    }

}
