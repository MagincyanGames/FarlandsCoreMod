using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.FarlandsLua.Functions.Factory
{
    public class LuaObjectFactory
    {
        public object OBJECT;
        public Type TYPE => OBJECT.GetType();
        
        public DynValue get(string fieldName, Optional<bool> isPublic)
        { 
            var pub = isPublic.IsValue ? isPublic.Value : true;
            
            if(fieldName == null) 
                return DynValue.Nil;

            var field = TYPE.GetField(fieldName, BindingFlags.Instance | (pub ? BindingFlags.Public : BindingFlags.NonPublic));

            if (field != null) 
                return LuaConverter.ToLua(field.GetValue(OBJECT));
            
            var property = TYPE.GetProperty(fieldName, BindingFlags.Instance | (pub ? BindingFlags.Public : BindingFlags.NonPublic));

            if (property != null) 
                return LuaConverter.ToLua(property.GetValue(OBJECT));

            return DynValue.Nil;

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

        public void set(Both<string, Table> fieldInput, Optional<DynValue> value, Optional<bool> isPublic)
        {
            var pub = isPublic.IsValue ? isPublic.Value : true;

            if (fieldInput.IsT) {
                string fieldName = fieldInput.GetT();

            }

        }
    }
}
