using System;
using System.Collections.Generic;
using System.Text;

namespace FarlandsCoreMod.Scenes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class OnLoadScene : Attribute
    {
        public string SceneName { get; private set; }

        public OnLoadScene()
        {
            SceneName = null;
        }
        public OnLoadScene(string sceneName)
        {
            SceneName = sceneName;
        }


    }

    [AttributeUsage(AttributeTargets.Method)]
    public class OnUnloadScene : Attribute
    {
        public string SceneName { get; private set; }

        public OnUnloadScene()
        {
            SceneName = null;
        }
        public OnUnloadScene(string sceneName)
        {
            SceneName = sceneName;
        }


    }
}
