using System;

namespace FarlandsCoreMod.Utils;

public static class ObjectExtensor
{
    public static object GetFieldValue(this object instance, string field) =>
         instance.GetType()
            .GetField(field,
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.Static)

            .GetValue(instance);

    public static T GetFieldValue<T>(this object instance, string field) =>
        (T)GetFieldValue(instance, field);
}
