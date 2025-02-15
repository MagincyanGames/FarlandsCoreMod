using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.UI.Components
{
    public class RNone : RElement
    {
        public override string type => "none";

        public override Component Render()
        {
            return gameObject.transform;
        }

        public override Transform SubPoint()
        {
            return gameObject.transform;
        }
    }
}
