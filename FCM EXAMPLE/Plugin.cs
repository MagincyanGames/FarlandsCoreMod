using BepInEx;
using CommandTerminal;
using Farlands.Inventory;
using Farlands.PlantSystem;
using FarlandsCoreMod;
using FarlandsCoreMod.Configuration;
using FarlandsCoreMod.Extensors;
using System.Diagnostics;
using System.Linq;
namespace FrancopetaMod
{
    [BepInPlugin("magin.test", "Test", "1.0.0")]
    [BepInDependency("magin.fcm", "^0.2.0.0")]
    public class FarlandsExampleMod : BaseUnityPlugin
    {
        public void Awake()
        {
            CONFIG.Add(this, "example", "test", "This is an test example", "test");
            Logger.LogInfo("THIS IS A TEST");
        }

    }
}
