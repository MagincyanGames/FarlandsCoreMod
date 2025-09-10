using System;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using Language.Lua;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FarlandsCoreMod;

public class Mod : MonoBehaviour
{
    public ConfigFile Config;
    public ModInfo Info;

    public void RegisterMod(BaseUnityPlugin plugin)
    {
        this.Info = new ModInfo()
        {
            Name = plugin.Info.Metadata.Name,
            GUID = plugin.Info.Metadata.GUID,
            Version = $"{plugin.Info.Metadata.Version.Major}.{plugin.Info.Metadata.Version.Minor}.{plugin.Info.Metadata.Version.Revision}",
        };

        Config = plugin.Config;

        FCM.Instance.RegisterMod(this);

        SceneManager.activeSceneChanged += OnSceneChange;
    }

    public virtual void OnSceneChange(Scene oldScene, Scene newScene) { }
}

public class ModInfo
{
    public string Name;
    public string GUID;
    public string Version;
}