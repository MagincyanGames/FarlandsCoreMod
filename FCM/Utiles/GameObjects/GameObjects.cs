using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FarlandsCoreMod.Utiles.GameObjects
{
    public static class GameObjects
    {
        public static GameObject Find(string path)
        {
            var spl1 = path.Split(':');
            var scene = spl1[0];
            var spl2 = spl1[1].Split('/');

            return Find(scene, spl2);
        }

        public static GameObject Find(string scene, params string[] path)
        {
            GameObject selected = null;
            foreach (GameObject go in SceneManager.GetSceneByName(scene).GetRootGameObjects())
            {
                if (go.name != path[0])
                    continue;

                selected = go;
                foreach (string p in path.Skip(1))
                {
                    selected = FindChildByName(selected, p);
                }
            }

            return selected;
        }

        static GameObject FindChildByName(GameObject parent, string childName)
        {
            foreach (Transform child in parent.transform)
            {
                if (child.name == childName)
                {
                    return child.gameObject;
                }
            }
            return null; // Si no se encuentra, retorna null
        }
    }
}
