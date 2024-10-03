﻿using Farlands.Inventory;
using FarlandsCoreMod.FarlandsLua;
using FarlandsCoreMod.Utiles;
using FarlandsCoreMod.Utiles.Loaders;
using Language.Lua;
using MoonSharp.Interpreter;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.ChatMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;


namespace FarlandsCoreMod.FarlandsLua.Functions
{
    public static class LuaFactory
    {
        // ------------------- Variables ------------------- //
        // private static DynValue updateFunction;
        // private static DynValue startFunction;


        /* Componentes y/o propiedades que se pueden acceder
         * Escribir/Editar
         * Leer
         * Eliminar
        */
        public static DynValue FromObject(object @object)
        {
            DynValue result = DynValue.NewTable(new Table(LuaManager.LUA));

            result.Table.Set("get", DynValue.NewCallback((ctx, args) =>
            {
                // TODO

                Type type = @object.GetType();
                string fieldName = args[0].String;
                bool isPublic = args.Count > 1 ? args[1].Boolean : true;

                if (fieldName == null) return DynValue.Nil;

                var field = type.GetField(fieldName, BindingFlags.Instance | (isPublic ? BindingFlags.Public : BindingFlags.NonPublic));
                
                if(field != null) return LuaConverter.ToLua(field.GetValue(@object));

                var property = type.GetProperty(fieldName, BindingFlags.Instance | (isPublic ? BindingFlags.Public : BindingFlags.NonPublic));

                if(property != null) return LuaConverter.ToLua(property.GetValue(@object));

                return DynValue.Nil;
            }));
            result.Table.Set("set", DynValue.NewCallback((ctx, args) =>
            {
                Type type = @object.GetType();

                if (args[0].Type == DataType.String)
                {
                    string fieldName = args[0].String;
                    if (!ObjectSet(@object, fieldName, args[1], true))
                        ObjectSet(@object, fieldName, args[1], false);

                    return DynValue.Void;
                }

                Table table = args[0].Table;
                foreach (var pair in table.Pairs)
                { 
                    string name = pair.Key.String;
                    if (!ObjectSet(@object, name, pair.Value, true))
                        ObjectSet(@object, name, pair.Value, false);
                }

                return DynValue.Void;
                
            }));
            result.Table.Set("call", DynValue.NewCallback((ctx, args) =>
            {
                // TODO
                string methodName = args[0].String;
                DynValue[] methodArgs = args.GetArray().Skip(1).ToArray();
                
                MethodInfo methodInfo = null;

                foreach (var meth in @object.GetType().GetMethods())
                {
                    if (meth.Name == methodName && meth.GetParameters().Length == methodArgs.Length)
                    {
                        methodInfo = meth;
                        break;
                    }
                }

                ParameterInfo[] parameters = methodInfo.GetParameters();
                object[] invokeArgs = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (i < methodArgs.Length)
                    {
                        invokeArgs[i] = LuaConverter.ToCS(methodArgs[i], parameters[i].ParameterType);
                    }
                    else
                    {
                        invokeArgs[i] = Type.Missing; // Valores por defecto si no hay suficientes argumentos
                    }
                }

                object returnValue = methodInfo.Invoke(@object, invokeArgs);

                return DynValue.FromObject(LuaManager.LUA, returnValue);
            }));

            return result;
        }

        public static DynValue FromComponent(Component component)
        {
            DynValue result = FromObject(component);

            // Devuelve el gameObject de este componente
            result.Table.Set("game_object", DynValue.NewCallback((ctx, args) =>
            {
                return FromGameObject(component.gameObject);
            }));

            if (component is SpriteRenderer spriteRenderer)
            {
                result.Table.Set("set_sprite", DynValue.NewCallback((ctx, args) =>
                {
                    var path = args[0].String;

                    var raw = LuaManager.GetFromMod(path);
                    var texture = new Texture2D(1, 1);

                    texture.LoadImage(raw);
                    texture.filterMode = FilterMode.Point;

                    spriteRenderer.sprite = SpriteLoader.FromTexture(texture);

                    return DynValue.Void;
                }));
            }
            if (component is Image image)
            {
                result.Table.Set("set_sprite", DynValue.NewCallback((ctx, args) =>
                {
                    var path = args[0].String;

                    var raw = LuaManager.GetFromMod(path);
                    var texture = new Texture2D(1, 1);

                    texture.LoadImage(raw);
                    texture.filterMode = FilterMode.Point;

                    image.sprite = SpriteLoader.FromTexture(texture);

                    return DynValue.Void;
                }));
            }
            
            return result;
        }

        private static bool ObjectSet(object obj, string name, DynValue value, bool isPublic)
        {
            var field = obj.GetType().GetField(name, BindingFlags.Instance | (isPublic ? BindingFlags.Public : BindingFlags.NonPublic));
            object val = LuaConverter.ToCS(value);

            if (field != null)
            {
                field.SetValue(obj, val);
                return true;
            }
            else
            {
                var property = obj.GetType().GetProperty(name, BindingFlags.Instance | (isPublic ? BindingFlags.Public : BindingFlags.NonPublic));

                if (property != null)
                {
                    property.SetValue(obj, val);
                    return true;
                }
            }

            return false;
        }
        public static DynValue FromGameObject(GameObject gameObject)
        {
            if (gameObject == null)
                return DynValue.Nil;

            DynValue result = FromObject(gameObject);

            // Sirve para obtener el nombre del GameObject
            result.Table.Set("get_name", DynValue.NewCallback((ctx, args) =>
            {
                return DynValue.NewString(gameObject.name);
            }));

            // Sirve para obtener la posición del GameObject
            result.Table.Set("get_position", DynValue.NewCallback((ctx, args) =>
            {
                var position = gameObject.transform.position;

                return LuaConverter.ToLua(position);
            }));

            // Sirve para modificar la posición del GameObject (x, y, z)
            result.Table.Set("set_position", DynValue.NewCallback((ctx, args) =>
            {
                if (args.Count < 1) return DynValue.Void;
                var nameObjects = args.GetArray().Select(x => x.String);

                var position = gameObject.transform.position;
                // Modificar el transform

                var addition = new Vector3(
                    args.Count >= 1 ? (float)args[0].Number : 0f, // x
                    args.Count >= 2 ? (float)args[1].Number : 0f, // y
                    args.Count >= 3 ? (float)args[2].Number : 0f); // z

                gameObject.transform.position = addition;

                return DynValue.Void;
            }));

            // Sirve para agregar posición al GameObject (x, y, z)
            result.Table.Set("add_position", DynValue.NewCallback((ctx, args) =>
            {
                if (args.Count < 1) return DynValue.Void;
                var nameObjects = args.GetArray().Select(x => x.String);

                var position = gameObject.transform.position;
                // Modificar el transform

                var addition = new Vector3(
                    args.Count >= 1 ? (float)args[0].Number : 0f,
                    args.Count >= 2 ? (float)args[1].Number : 0f,
                    args.Count >= 3 ? (float)args[2].Number : 0f);

                Debug.Log(gameObject.transform.position);
                gameObject.transform.position += addition;
                Debug.Log(gameObject.transform.position);
                return DynValue.Void;
            }));

            // Cambia el tamaño de un GameObject (x,y,z)
            result.Table.Set("set_scale", DynValue.NewCallback((ctx, args) =>
            {

                if (args.Count == 1) gameObject.transform.localScale = new((float)args[0].Number, (float)args[0].Number, 1);
                else if (args.Count == 2) gameObject.transform.localScale = new((float)args[0].Number, (float)args[1].Number, 1);
                else if (args.Count == 3) gameObject.transform.localScale = new((float)args[0].Number, (float)args[1].Number, (float)args[2].Number);

                return DynValue.Void;
            }));

            // Hace que el estado de activación de un GameObject cambie de valor
            result.Table.Set("toggle_active", DynValue.NewCallback((ctx, args) =>
            {
                gameObject.SetActive(!gameObject.activeSelf);
                return DynValue.Void;
            }));

            //TODO: Modificación completa de la lógica
            result.Table.Set("get_component", DynValue.NewCallback((ctx, args) =>
            {
                if (args == null)
                    return DynValue.Void;

                Debug.Log("Usted ah ejecutado 'get_component' sin plomo 95");

                string _nombreComponente = ""; // = args[0].String;
                Table _properties = null; // = args.Count > 1 ? args[1] : null;
                Type _componentType = null;

                // tu dices que es inicesario, yo digo que me la pela
                foreach (DynValue arg in args.GetArray())
                {
                    if (arg.Type == DataType.String)
                        _nombreComponente = arg.String;

                    if (arg.Type == DataType.Table)
                        _properties = arg.Table;
                }

                foreach (var t in AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()))
                {
                    if (t.Name == _nombreComponente && typeof(Component).IsAssignableFrom(t))
                    {
                        _componentType = t;
                        break;
                    }
                }

                if (_componentType == null || !typeof(Component).IsAssignableFrom(_componentType))
                {
                    Debug.LogError("El tipo especificado no es un componente válido: " + _nombreComponente);
                    return DynValue.Void;
                }

                Component _componente = gameObject.GetComponent(_componentType);
                if (_componente == null)
                    _componente = gameObject.AddComponent(_componentType);

                if(_properties != null) 
                    foreach (var pair in _properties.Pairs)
                    {
                        string name = pair.Key.String;
                        if (!ObjectSet(gameObject, name, pair.Value, true))
                            ObjectSet(gameObject, name, pair.Value, false);
                    }

                return FromComponent(_componente);

            }));
             
            if (gameObject.TryGetComponent<SeedSelector>(out var seedSelector))
            {
                result.Table.Set("seed_selector", DynValue.NewTable(new Table(LuaManager.LUA)));
                var _seedSelector = result.Table.Get("seed_selector");

                _seedSelector.Table.Set("get_instance", DynValue.NewCallback((ctx, args) =>
                {
                    if (args.Count > 0 && args[0].Type == DataType.Number)
                    {
                        var list = Private.GetFieldValue<List<EquippableData>>(seedSelector, "seedInstances");
                        foreach (EquippableData seedInstance in list)
                        {
                            if (Convert.ToInt32(args[0].Number) == seedInstance.itemID)
                                return FromGameObject(seedInstance.instance);

                        }
                    }

                    return DynValue.Nil;
                }));
            }

            // Definicion para start y update

            if (!gameObject.TryGetComponent<LuaGameObjectComponent>(out var _Ficha))
            {
                _Ficha = gameObject.AddComponent<LuaGameObjectComponent>();
            }

            _Ficha.Result = result;
            result.Table.Set("_mod_", LuaManager.MOD);
            result.Table.Set("set_update", DynValue.NewCallback((ctx, args) =>
            {
                result.Table.Set("Update", args[0]);
                return DynValue.Void;
            }));
            result.Table.Set("set_start", DynValue.NewCallback((ctx, args) =>
            {
                result.Table.Set("Start", args[0]);
                return DynValue.Void;
            }));

            return result;
        }

        

    }
}