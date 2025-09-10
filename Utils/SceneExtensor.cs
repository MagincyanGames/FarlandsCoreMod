using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FarlandsCoreMod.Utils;

public static class SceneExtensor
{
    public static GameObject GetGameObjectByPath(this Scene scene, string path)
    {
        var splittedPath = path.Split('/');
        Debug.Log($"Getting {splittedPath.First()}");
        var currentGameObject = scene.GetRootGameObjects().First(go => go.name == splittedPath.First());

        return currentGameObject.GetChildByPath(string.Join("/", splittedPath.Skip(1)));
    }
}
