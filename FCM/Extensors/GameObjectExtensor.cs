using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.Extensors
{
    public static class GameObjectExtensor
    {
        public static T TryAddComponent<T>(this GameObject gameObject, params object[] args) where T : Component
        {
            if(gameObject.TryGetComponent<T>(out var component)) return component;
            else return gameObject.AddComponent<T>(); 
        }
    }
}
