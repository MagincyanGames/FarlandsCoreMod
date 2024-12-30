using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.Bootstrap;
using FarlandsCoreMod.Attributes;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace FarlandsCoreMod
{
    [BepInDependency("top.magincian.fcm", "~0.1.0")]
    public abstract class FarlandsMod : BaseUnityPlugin
    {
        public Assembly ASM => Assembly.GetAssembly(this.GetType());
        public string PLUGIN_PATH => Path.Combine(Paths.Plugin, this.Info.Metadata.Name);
        public string GetPath(string path) => Path.Combine(PLUGIN_PATH, path);

        public abstract string SHORT_NAME { get; }

        private void Awake()
        {

        }
    }
}