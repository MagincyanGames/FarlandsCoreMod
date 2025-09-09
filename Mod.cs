using System;
using BepInEx;
using UnityEngine;

namespace FarlandsCoreMod;

public class Mod : MonoBehaviour
{

    public ModInfo Info;
    public void RegisterMod(BaseUnityPlugin plugin)
    {
        this.Info = new ModInfo()
        {
            Name = plugin.Info.Metadata.Name,
            GUID = plugin.Info.Metadata.GUID,
            Version = $"{plugin.Info.Metadata.Version.Major}.{plugin.Info.Metadata.Version.Minor}.{plugin.Info.Metadata.Version.Revision}",
        };

        FCM.Instance.RegisterMod(this);
    }

    public virtual void OnSceneChange() { }
}

public class ModInfo
{
    public string Name;
    public string GUID;
    public string Version;
}