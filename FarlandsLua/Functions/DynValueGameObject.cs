using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarlandsCoreMod.FarlandsLua.Functions
{
    public class DynValueCustom
    {
        public DynValue value;
        public static DynValueCustom Nil => new DynValueCustom(DynValue.Nil);
        public DynValueCustom(DynValue value)
        {
            this.value = value;
        }

        public static implicit operator DynValue(DynValueCustom dyn) =>
            dyn.value;
    }

    public class DynValueGameObject : DynValueCustom
    {
        new public static DynValueGameObject Nil => new DynValueGameObject(DynValue.Nil);

        public DynValueGameObject(DynValue value) : base(value)
        {
        }
    }

    public class DynValueComponent : DynValueCustom
    {
        new public static DynValueComponent Nil => new DynValueComponent(DynValue.Nil);

        public DynValueComponent(DynValue value) : base(value)
        {
        }
    }
}
