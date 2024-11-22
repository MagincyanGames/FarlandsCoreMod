using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarlandsCoreMod.FarlandsLua.Functions
{
    public class DynValueGameObject
    {
        public DynValue value;
        public static DynValueGameObject Nil => new DynValueGameObject(DynValue.Nil);
        public DynValueGameObject(DynValue value)
        {
            this.value = value;
        }

        public static implicit operator DynValue(DynValueGameObject dyn) =>
            dyn.value;
    }
}
