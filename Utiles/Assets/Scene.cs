using CommandTerminal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

namespace FarlandsCoreMod.Utiles.Assets
{
    public class BundleScene : GenericBundle
    {
        public BundleScene() { }

        public BundleScene(string path) : base(path) { }

        public BundleScene(byte[] raw) : base(raw) { }

        public string GetScenePath(string scene)
        {
            foreach (var scn in bundle.GetAllScenePaths())
            {
                Debug.Log(scn);
                if(Path.GetFileNameWithoutExtension(scn) == scene)
                    return scn;
            }

            return null;
        }

        public void LoadScene(string scene) => SceneManager.LoadScene(GetScenePath(scene));
    }
}
