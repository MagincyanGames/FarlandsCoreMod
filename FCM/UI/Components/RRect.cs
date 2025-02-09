using FarlandsCoreMod.Extensors;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.UI.Components
{
    public class RRect : RElement
    {
        public Vector2? anchoredPosition;
        public Vector2? size;
        public Vector2? anchorMax;
        public Vector2? anchorMin;
        public Vector2? pivot;
        public Vector2? offsetMax;
        public Vector2? offsetMin;

        public override string type => "rect";

        public override Component Render()
        {
            var rect = gameObject.TryAddComponent<RectTransform>();

            if (anchorMax != null) rect.anchorMax = anchorMax.Value;
            if (anchorMin != null) rect.anchorMin = anchorMin.Value;
            if (pivot != null) rect.pivot = pivot.Value;
            if (offsetMax != null) rect.offsetMax = offsetMax.Value;
            if (offsetMin != null) rect.offsetMin = offsetMin.Value;

            if (size != null) rect.sizeDelta = size.Value;
            if (anchoredPosition != null) rect.anchoredPosition = anchoredPosition.Value;
            rect.transform.localScale = new Vector3(1, 1, 1);

            return rect;
        }
        public void RectPosition()
        {
            
        }

        public override Transform SubPoint() => gameObject.transform;
    }
}
