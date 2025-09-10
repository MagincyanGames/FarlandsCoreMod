using System;
using Unity.VisualScripting;
using UnityEngine;

namespace FarlandsCoreMod.Utils;

public static class GameObjectExtensor
{

    public static GameObject GetChildByPath(this GameObject gameObject, string path)
    {
        var splittedPath = path.Split('/');

        foreach (var name in splittedPath)
        {
            Debug.Log($"Getting {name}");
            if (gameObject == null) return null;

            gameObject = gameObject.GetChildByName(name);
        }

        return gameObject;
    }
    public static GameObject GetChildByName(this GameObject gameObject, string name)
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.name == name)
                return child.gameObject;
        }

        return null; //? Debería dar error en lugar de devolver nulo?
    }
}
