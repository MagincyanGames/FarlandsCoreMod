using FarlandsCoreMod.Extensors;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace FarlandsCoreMod.UI.Components
{
    public class RText : RRect
    {
        public override string type => "text";
        public int? fontSize;
        public string? text;
        public TMPro.VerticalAlignmentOptions? verticalAlignment;
        public TMPro.HorizontalAlignmentOptions? horizontalAlignment;
        public override Component Render()
        {
            base.Render();

            var text = gameObject.TryAddComponent<TextMeshProUGUI>();
            if(this.text != null) text.text = this.text;
            if (this.fontSize.HasValue) text.fontSize = fontSize.Value;
            if (this.verticalAlignment.HasValue) text.verticalAlignment = this.verticalAlignment.Value;
            if (this.horizontalAlignment.HasValue) text.horizontalAlignment = this.horizontalAlignment.Value;


            return text;
        }

        public override Transform SubPoint() => gameObject.transform;
    }
}
