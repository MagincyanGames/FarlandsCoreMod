using BepInEx.Configuration;
using CommandTerminal;
using Farlands.Dev;
using Farlands.Inventory;
using FarlandsCoreMod.Attributes;
using FarlandsCoreMod.Utiles;
using HarmonyLib;
using I2.Loc;
using JanduSoft;
using Language.Lua;
using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq; 
using System.Text; 
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FarlandsCoreMod.FarlandsConsole
{
    [Patcher]
    public class Manager : IManager
    {
        // ----------------------- DECLARACIONES ----------------------- //
        public static ConfigEntry<bool> UnityDebug;
        public static Dictionary<string, FarlandsEasyMod> EasyMods = new();
        public static Dictionary<string, List<Action>> OnEvents = new();
        public static FarlandsEasyMod CURRENT_MOD;
        public static Script LUA = new();
        public static GameObject _o; // public static, Odio mi vida


        /// <summary>
        /// name: MOD
        /// especie de getter y setter para la variable global _mod_
        /// </summary>
        public static DynValue MOD
        {
            get => LUA.Globals.Get("_mod_");
            set => LUA.Globals.Set("_mod_", value);
        }

        public int Index => 1;

        /// <summary>
        ///     name: ExecuteEvent
        ///     Ejecuta un evento en todos los mods cargados
        /// </summary>
        /// <param name="ev"></param>
        public static void ExecuteEvent(params string[] ev)
        {
            Debug.Log(string.Join('.', ev));
            foreach (var mod in EasyMods.Values)
            {
                var dyn = mod.Mod.Table.Get("event");
                foreach (var single in ev)
                {
                    Debug.Log("mondngo");
                    dyn = dyn.Table.Get(single);
                    if (dyn.Type == DataType.Nil) break;
                }

                if (dyn.Type != DataType.Nil)
                {
                    MOD = mod.Mod; // TODO revisar si funciona
                    LUA.Call(dyn);
                }
            }
        }

        // Método para inicializar el Manager
        public void Init()
        {
            UnityDebug = FarlandsCoreMod.AddConfig("Debug", "UnityDebug", "", false);

            AuxiliarFunctions();

            if (!Directory.Exists(Paths.Plugin))
                Directory.CreateDirectory(Paths.Plugin);

            var src = Directory.GetFiles(Paths.Plugin, "*.zip");

            src.ToList().ForEach(FarlandsEasyMod.LoadAndAddZip);
        }

        public static string[] GetFilesInMod(string path)
        {
            var i = path.IndexOf('/');
            var mod = path.Substring(0, i);
            if (mod == ".") mod = MOD.Table.Get("tag").String;
            return EasyMods[mod].GetFilesInFolder(mod, path.Substring(i + 1, path.Length - i - 1));
        }

        // Método para obtener datos de un mod
        public static byte[] GetFromMod(string path)
        {
            var i = path.IndexOf('/');
            var mod = path.Substring(0, i);
            if (mod == ".") mod = MOD.Table.Get("tag").String;
            return EasyMods[mod][path.Substring(i + 1, path.Length - i - 1)];
        }

        // Método para definir funciones auxiliares en LUA
        public static void AuxiliarFunctions()
        {
            //TODO agergar forma para ampliar la cámara

            LUA.Globals["MOD"] = (string tag) =>
            {
                var code =
@$"
{tag} = {{}}
{tag}.tag = '{tag}'
{tag}.scenes = {{}}
{tag}.event = {{}}
{tag}.event.scene = {{}}
{tag}.event.scene.change = {{}}
{tag}.event.language = {{}}
{tag}.event.language.change = {{}}
{tag}.event.dialogue = {{}}
{tag}.event.dialogue.portrait = {{}}

_mod_ = {tag}";

                Execute(code, null);
                CURRENT_MOD.ConfigFile = new(Path.Combine(Paths.Config, $"{tag}.cfg"), true);
                EasyMods.Add(tag, CURRENT_MOD);
            };

            LUA.Globals["config"] = (string section, string key, DynValue def, string description) =>
            {
                var code =
@$"
_mod_.config = _mod_.config or {{}}
_mod_.config.{section} = _mod_.config.{section} or {{}}
";
                Execute(code, null);
                if (def.Type == DataType.Boolean)
                {
                    var entry = CURRENT_MOD.ConfigFile.Bind(section, key, def.Boolean, description);

                    LUA.Globals.Get("_mod_")
                        .Table.Get("config")
                        .Table.Get(section)
                        .Table.Set(key, DynValue.NewCallback((ctx, args) => DynValue.NewBoolean(entry.Value)));
                }

            };

            // ----------------------- COMANDO DE COMANDOS ----------------------- //
            LUA.Globals["execute_command"] = (string _comando) =>
            {
                if (_o == null)
                {
                    _o = new GameObject("FalsoDebugController");
                    _o.AddComponent<Farlands.Dev.DebugController>();
                }
                List<object> _lista = _o.GetComponent<Farlands.Dev.DebugController>().commandList;


                string[] args = _comando.Split(' ', StringSplitOptions.None);
                for (int i = 0; i < _lista.Count; i++)
                {
                    DebugCommandBase debugCommandBase = _lista[i] as DebugCommandBase;
                    if (args.Contains(debugCommandBase.commandId))
                    {
                        if (_lista[i] is DebugCommand)
                        {
                            (_lista[i] as DebugCommand).Invoke();
                        }
                        else if (_lista[i] is DebugCommand<int>)
                        {
                            (_lista[i] as DebugCommand<int>).Invoke(int.Parse(args[1]));
                        }
                        else if (_lista[i] is DebugCommand<int, int>)
                        {
                            int value = int.Parse(args[1]);
                            int value2 = int.Parse(args[2]);
                            if (int.TryParse(args[1], out value) && int.TryParse(args[2], out value2))
                            {
                                (_lista[i] as DebugCommand<int, int>).Invoke(value, value2);
                            }
                            else
                            {
                                Debug.LogWarning("Invalid parameter format for DebugCommand<int, int>.");
                            }
                        }
                    }
                }

                //Destroy(_o);
            };

            // ----------------------- FUNCIONES DE ESCENA ----------------------- //
            LUA.Globals["load_scene"] = (DynValue scene) =>
            {
                if(scene.Type == DataType.String)
                    SceneManager.LoadScene(scene.String);
                else if(scene.Type == DataType.Number)
                    SceneManager.LoadScene(Convert.ToInt32(scene.Number));
            };

            LUA.Globals["print_scene"] = () =>
            {
                Scene currentScene = SceneManager.GetActiveScene();
                Terminal.Log($"({currentScene.buildIndex}) {currentScene.name}");
            };

            LUA.Globals["texture_override"] = DynValue.NewCallback((ctx, args) =>
            {
                if (args.Count == 0) throw new Exception("Invalid args for TextureOverride");
                else if (args.Count == 1)
                {
                    var path = args[0].String;
                    Source.Replace.OtherTexture(Path.GetFileNameWithoutExtension(path), GetFromMod(path));
                }
                else if (args.Count == 2)
                {
                    var origin = args[0].String;
                    var path = args[1].String;
                    Source.Replace.OtherTexture(origin, GetFromMod(path));
                }

                return DynValue.Void;
            });

            LUA.Globals["texture_override_in"] = DynValue.NewCallback((ctx, args) =>
            {
                var path = args[0].String;
                Debug.Log(string.Join('\n', GetFilesInMod(path)));

                foreach (var item in GetFilesInMod(path))
                    Source.Replace.OtherTexture(Path.GetFileNameWithoutExtension(item), GetFromMod(item));

                return DynValue.Void;
            });

            // No funciona
            LUA.Globals["sprite_override"] = (string origin, int[] position, string path) =>
            {
                var vec = new Vector2Int(position[0], position[1]);
                Source.Replace.ReplaceSprite(origin, vec, GetFromMod(path));
            };

            LUA.Globals["portrait_override"] = (string origin, string path) =>
            {
                string code =
@$"
    function _mod_.event.dialogue.portrait:{origin}()
        texture_override('{origin}', '{path}')
    end
    ";
                LUA.DoString(code);
            };

            /// <summary>
            /// 
            /// </summary>
            LUA.Globals["add_language"] = (string path) =>
            {
                FarlandsDialogueMod.Manager.AddSourceFromBytes(GetFromMod(path));
            };
            LUA.Globals["get_language"] = () =>
            {
                return LocalizationManager.CurrentLanguage;
            };

            
            LUA.Globals["print"] = (string txt) =>
            {
                if (!UnityDebug.Value) Debug.Log(txt);
                Terminal.Log(txt);

            };

            // ----------------------- BUSCAR OBJETOS ----------------------- //
            LUA.Globals["get_object"] = DynValue.NewCallback((ctx, args) =>
            {
                if (args.Count < 1) return DynValue.Nil;
                var nameObjects = args.GetArray().Select(x => x.String);

                var scene = SceneManager.GetActiveScene();
                GameObject previous = null;

                foreach (var go in nameObjects)
                {
                    if (previous == null) previous = scene.GetRootGameObjects().First(x => x.name == go);
                    else
                    {
                        for (var i = 0; i < previous.transform.childCount; i++)
                        {
                            var next = previous.transform.GetChild(i).gameObject;
                            if (next.name == go)
                            {
                                previous = next;
                                break;
                            }
                        }
                    }
                }
                return LuaGameObject.FromGameObject(previous);
            });

            LUA.Globals["find_object"] = DynValue.NewCallback((ctx, args) =>
            {
                if (args.Count < 1) return DynValue.Nil;

                if (args.Count == 1)
                {
                    var go = GetAllGameObjectsInScene(SceneManager.GetActiveScene()).First(x => x.name == args[0].String);
                    return LuaGameObject.FromGameObject(go);
                }

                var gameObject = GetAllGameObjectsInScene(SceneManager.GetSceneByName(args[1].String)).First(x => x.name == args[0].String);
                return LuaGameObject.FromGameObject(gameObject);
            });


            LUA.Globals["add_item"] = (int _id, int _cantidad) =>
            {
                UnityEngine.Object.FindObjectOfType<InventorySystem>().AddItemByID(_id, _cantidad);
            };

            LUA.Globals["add_credits"] = (int _cantidad) =>
            {
                Singleton<FarlandsGameManager>.Instance.persistentDataScript.credits += _cantidad;
                UnityEngine.Object.FindObjectOfType<HUDMoneyScript>().UpdateCredits();
            };

            // ----------------------- CREAR OBJETOS ----------------------- //
            LUA.Globals["create_object"] = (string name) =>
            {
                var go = new GameObject(name);
                return LuaGameObject.FromGameObject(go);
            };

            LUA.Globals["create_scene"] = (string name) =>
            {
                var scene = SceneManager.CreateScene(name);
                //TODO agergar creación del objeto de la escena para lua
            };

            LUA.Globals["add_command"] = (string name, DynValue luaFunc, string help) =>
            {
                Action<CommandArg[]> action = (CommandArg[] args) =>
                {
                    var arguments = new List<DynValue>();

                    foreach (var a in args)
                    {
                        if (float.TryParse(a.String, out var floatValue))
                        {
                            Debug.Log("float:" + floatValue);
                            arguments.Add(DynValue.NewNumber(floatValue));
                        }
                        else if (int.TryParse(a.String, out var intValue))
                        {
                            Debug.Log("int:" + intValue);
                            arguments.Add(DynValue.NewNumber(floatValue));
                        }
                        else if (bool.TryParse(a.String, out var boolValue))
                        {
                            Debug.Log("bool:" + boolValue);
                            arguments.Add(DynValue.NewBoolean(boolValue));
                        }
                        else
                        {
                            Debug.Log("str:" + a.String);
                            arguments.Add(DynValue.NewString(a.String));
                        }
                    }

                    LUA.Call(luaFunc, arguments.ToArray());

                };

                Terminal.Shell.AddCommand(name, action, help: help);
                Terminal.Autocomplete.Register(name);
            };

        }

        // Método para ejecutar código en LUA
        public static DynValue Execute(byte[] codes, FarlandsEasyMod fem) =>
            Execute(Encoding.UTF8.GetString(codes), fem);

        public static DynValue Execute(string codes, FarlandsEasyMod fem)
        {
            if (fem != null && fem.Tag != null)
            {
                CURRENT_MOD = fem;
                MOD = DynValue.NewString(fem.Tag);
            }

            return LUA.DoString(codes);
        }

        private static string currentEvent = null;

        [HarmonyPatch(typeof(DebugController), "HandleInput")]
        [HarmonyPrefix]
        public static bool ExecuteInput(DebugController __instance)
        {
            Execute(Private.GetFieldValue<string>(__instance, "input"), null);
            return false;
        }

        static List<GameObject> GetAllGameObjectsInScene(Scene scene)
        {
            // Obtén todos los objetos raíz de la escena
            GameObject[] rootObjects = scene.GetRootGameObjects();
            System.Collections.Generic.List<GameObject> allObjects = new System.Collections.Generic.List<GameObject>();

            // Recorre cada objeto raíz y sus hijos
            foreach (GameObject rootObject in rootObjects)
            {
                allObjects.Add(rootObject);
                AddChildObjects(rootObject.transform, allObjects);
            }

            return allObjects;
        }

        static void AddChildObjects(Transform parent, System.Collections.Generic.List<GameObject> allObjects)
        {
            foreach (Transform child in parent)
            {
                allObjects.Add(child.gameObject);
                AddChildObjects(child, allObjects);
            }
        }
    }
}