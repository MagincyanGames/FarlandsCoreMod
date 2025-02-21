using BepInEx;
using CommandTerminal;
using Farlands.Inventory;
using Farlands.PlantSystem;
using FarlandsCoreMod;
using FarlandsCoreMod.Configuration;
using FarlandsCoreMod.Extensors;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
namespace FrancopetaMod
{
    [BepInPlugin("magin.test", "Test", "1.0.0")]
    [BepInDependency("magin.fcm", "^0.2.0.0")]
    public class FarlandsExampleMod : AbstractMod
    {
        public void Awake()
        {
            AddConfig("example/1test", "This is an test example", "test");
            AddConfig("example/2test", "This is an test example", 4);
            AddConfig("example/3test", "This is an test example", 2.5f);
            AddConfig("example/4test", "This is an test example", false);
            AddConfig("example/5test", "This is an test example", "prueba");
            Logger.LogInfo("THIS IS A TEST");
        }

        public override Sprite LoadSprite(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}
