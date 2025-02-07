using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine.SceneManagement;

namespace FarlandsCoreMod.Scenes
{
    public class SceneLoader
    {
        private List<Type> types;
        public SceneLoader()
        {
            types = new List<Type>();
        }
        public SceneLoader(params List<Type> types)
        {
            this.types = types;
        }

        public void Add<T>() where T : class
        {
            types.Add(typeof(T));
        }
        public void Add(Type type)
        {
            types.Add(type);
        }

        public void Load()
        {
            foreach (Type type in types)
            {
                foreach (MethodInfo method in type.GetMethods())
                {
                    if (method.GetCustomAttributes(typeof(OnLoadScene), false).Length > 0)
                        LoadMethod(method);

                    if (method.GetCustomAttributes(typeof(OnUnloadScene), false).Length > 0)
                        UnloadMethod(method);
                }
            }
        }
        public void UnloadMethod(MethodInfo methodInfo)
        {
            var att = (OnUnloadScene)methodInfo.GetCustomAttributes(typeof(OnUnloadScene), false)[0];

            ParameterInfo[] parameters = methodInfo.GetParameters();

            SceneManager.sceneUnloaded += (scene) =>
            {
                if (att.SceneName == null || att.SceneName == scene.name)
                {
                    if (parameters.Length == 0)
                        methodInfo.Invoke(null, null);
                    else if (parameters.Length == 1)
                        methodInfo.Invoke(null, [scene]);
                }
            };
        }
        public void LoadMethod(MethodInfo methodInfo)
        {
            var att = (OnLoadScene)methodInfo.GetCustomAttributes(typeof(OnLoadScene), false)[0];

            ParameterInfo[] parameters = methodInfo.GetParameters();
            
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                if (att.SceneName == null || att.SceneName == scene.name)
                {
                    if (parameters.Length == 0)
                        methodInfo.Invoke(null, null);
                    else if (parameters.Length == 1)
                        methodInfo.Invoke(null, [scene]);
                    else if (parameters.Length == 2)
                        methodInfo.Invoke(null, [scene, mode]);
                }
            };
        }
    }
}
