using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.Extensors
{
    public static class PrintExtensor
    {
        public static void Print(this object o, string msg)
        {
            UnityEngine.Debug.Log(msg);
        }
    }
}
