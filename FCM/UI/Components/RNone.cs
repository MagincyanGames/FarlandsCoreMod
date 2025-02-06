using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.UI.Components
{
    public class RNone : RElement
    {
        public override string type => throw new NotImplementedException();

        public override Component Render()
        {
            return gameObjectForRender().transform;
        }
    }
}
