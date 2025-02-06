using FarlandsCoreMod.Extensors;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FarlandsCoreMod.UI.Components
{
    public class RButton : RImage
    {
        public override string type => "button";
        public Action? onClick;

        public override Component Render()
        {
            var gameObject = base.Render().gameObject;
            Button button = gameObject.TryAddComponent<Button>();

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick?.Invoke());


            return button;  
        }
    }
}
