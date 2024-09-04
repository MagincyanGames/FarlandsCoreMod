﻿using Farlands.DataBase;
using Farlands.Inventory;
using Farlands.PlantSystem;
using FarlandsCoreMod.Attributes;
using FarlandsCoreMod.Utiles;
using HarmonyLib;
using JanduSoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.FarlandsItems
{
    [Patcher]
    public class Manager : IManager
    {
        public static ScriptableObjectsDB DB => Singleton<ScriptableObjectsDB>.Instance;
        public static int GetLastInventoryItemId() => DB.inventoryItems.Count;
        public static int GetLastPlantId() => DB.plants.Count;

        public static int AddInventoryItem(InventoryItem item)
        {
            var id = GetLastInventoryItemId() + 1;

            item.itemID = id;

            DB.inventoryItems.Add(item);

            return id;
        }
        public static int AddPlant(PlantScriptableObject item)
        {
            var id = GetLastInventoryItemId() + 1;

            item.ID = id;

            DB.plants.Add(item);

            return id;
        }

        public class SeedData
        { 
            public int ItemId;
            public List<int> PlantsId;
        }

        public int Index => 0;
        public static List<SeedData> seeds = new();

        public void Init()
        {

        }

        [HarmonyPatch(typeof(InventorySystem), "Start")]
        [HarmonyPostfix]
        public static void OnStart(InventorySystem __instance)
        {
            var ss = __instance.GetComponent<SeedSelector>();
            var originalSeeds = Private.GetFieldValue<List<EquippableData>>(ss, "seedInstances");

            foreach (var s in seeds)
            {
                var seeds = Private.GetFieldValue<List<EquippableData>>(GameObject.FindObjectOfType<SeedSelector>(), "seedInstances");
                var instance = GameObject.Instantiate(seeds.First().instance);
                instance.SetActive(false);
                var seedScript = instance.GetComponent<SeedScript>();
                seedScript.plantsID = s.PlantsId;
                seedScript.ColdplantsID = new();
                seedScript.HotplantsID = new();
                seedScript.isRandom = false;

                originalSeeds.Add(new EquippableData()
                {
                    itemID = s.ItemId,
                    instance = instance,
                });
            }

            FarlandsConsole.Manager.ExecuteEvent("inventory", "start");
        }
    }
}
