using Farlands.UI;
using JetBrains.Annotations;
using MoonSharp.Interpreter;
using Rewired.Libraries.SharpDX.RawInput;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

namespace FarlandsCoreMod.FarlandsLua.Functions
{
    public class LuaMetadata
    {
        private List<Meta> metadata = new();

        public LuaMetadata()
        {
            metadata = new();
            Add(new() { type = LuaMetadata.Type.META, values = "farlands" });
            ClassObjectMetadata();
            ClassGameObjectMetadata();
        }

        public enum Type
        {
            META,
            CLASS,
            FIELD,
            COMMENT,
            PARAM,
            RETURN,
            CODE
        }
        public class Meta
        {
            public Type type;
            public string values;
            public string typeString {
                get {
                    switch (type)
                    {
                        case Type.FIELD: return "field";
                        case Type.CLASS: return "class";
                        case Type.META: return "meta";
                        case Type.PARAM: return "param";
                        case Type.RETURN: return "return";
                        default: return "comment";
                    }
                }
            }
            public override string ToString()
            {
                if(type == Type.CODE) return values;
                else return $"---@{typeString} {values}";
            }
        }
        public void Add(Meta meta) => metadata.Add(meta);
        public void AddClass(string className) => metadata.Add(new Meta() { type = Type.CLASS, values = className});
        public void AddCode(string code) => metadata.Add(new Meta() { type = Type.CODE, values = code });
        public void AddFunction(string name, string parameters) => metadata.Add(new Meta() { type = Type.CODE, 
            values = $"function {name}({parameters}) end" });

        public void AddParam(string name, System.Type type) => metadata.Add(new Meta()
        {
            type = Type.PARAM,
            values = $"{name} {CSharpTypeToLuaMetadata(type)}",
        });
        public void AddReturn(System.Type type) => metadata.Add(new Meta()
        {
            type = Type.RETURN,
            values = $"{CSharpTypeToLuaMetadata(type)}",
        });

        public void AddField(string name, System.Type type) => metadata.Add(new Meta()
        {
            type = Type.FIELD,
            values = $"{name} {CSharpTypeToLuaMetadata(type)}",
        });
        public void AddFieldFun(string name, string param, System.Type retType) => metadata.Add(new Meta()
        {
            type = Type.FIELD,
            values = $"{name} fun({param}):{CSharpTypeToLuaMetadata(retType)}"
        });
        private static string CSharpTypeToLuaMetadata(System.Type type)
        {
            if (typeof(DynValueGameObject).IsAssignableFrom(type)) return "GameObject";
            if (typeof(DynValue).IsAssignableFrom(type)) return "any";
            if (typeof(int).IsAssignableFrom(type)) return "integer";
            if (typeof(float).IsAssignableFrom(type)) return "number";
            if (typeof(string).IsAssignableFrom(type)) return "string";
            if (typeof(Optional<>).IsAssignableFrom(type))
            {
                var t = CSharpTypeToLuaMetadata(type.GenericTypeArguments[0]); // solo tiene un argumento genérico
                return $"undefined | {t}";
            }
            if (typeof(Both<,>).IsAssignableFrom(type))
            {
                var t = CSharpTypeToLuaMetadata(type.GenericTypeArguments[0]);
                var k = CSharpTypeToLuaMetadata(type.GenericTypeArguments[1]);
                return $"{t} | {k}";
            }
            if (type.IsArray)
            {
                var elementType = CSharpTypeToLuaMetadata(type.GetElementType());
                return elementType + "[]";
            }
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                var genericType = type.IsGenericType ? type.GetGenericArguments()[0] : typeof(DynValue);
                var elementType = CSharpTypeToLuaMetadata(genericType);
                return elementType + "[]";
            }

            return "any";
        }
        private void ClassObjectMetadata()
        {
            AddClass("Object");
            AddFieldFun("get", "key:string", typeof(DynValue));
            AddFieldFun("set", "key:string, value:any", typeof(DynValue));
            AddFieldFun("call", "function:string, value:any", typeof(DynValue));
        }

        private void ClassGameObjectMetadata()
        {
            AddClass("GameObject");
            AddFieldFun("get", "key:string", typeof(DynValue));
            AddFieldFun("set", "key:string, value:any", typeof(DynValue));
            AddFieldFun("call", "function:string, value:any", typeof(DynValue));

            AddFieldFun("get_name", "", typeof(string));

            AddFieldFun("get_position", "", typeof(DynValue)); // TODO cambiar este any por DynValue
            AddFieldFun("set_position", "pos:any", typeof(DynValue)); // TODO cambiar este any por vector3
            AddFieldFun("add_position", "pos:any", typeof(DynValue)); // TODO cambiar este any por vector3

            AddFieldFun("set_scale", "scale:any", typeof(DynValue)); // TODO cambiar este any por vector3

            AddFieldFun("toggle_active", "", typeof(void));

            AddFieldFun("get_layer", "", typeof(int));

            AddFieldFun("set_layer", "id:integer", typeof(int));

            AddFieldFun("set_update", "f:function", typeof(void));
            AddFieldFun("set_start", "f:function", typeof(void));

            // TODO agregar los componentes


        }

        public override string ToString()
        {
            return string.Join("\n", metadata.Select(x => x.ToString()));
        }
    }

    public class Optional<T>
    {
        public T Value;
        public bool IsNull => Value == null;
        public bool IsValue => Value != null;

        public Optional(T value)
        {
            Value = value;
        }
    }

    public class Both<T, K>
    {
        public object Value;

        public bool IsT;

        public Both(object val)
        {
            Value = val;
            IsT = val is T;
        }

        public T GetT() => (T) Value;
        public K GetK() => (K) Value;
    }
}
