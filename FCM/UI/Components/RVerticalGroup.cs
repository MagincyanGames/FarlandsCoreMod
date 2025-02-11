using FarlandsCoreMod.Extensors;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FarlandsCoreMod.UI.Components
{
    public class RVerticalGroup : RImage
    {
        public override string type => "verticalGroup";
        public float? spacing;
        public override Component Render()
        {
            base.Render();
            gameObject.TryAddComponent<VerticalLayoutGroup>();
            gameObject.TryAddComponent<ContentSizeFitter>();

            RectTransform contentRect = gameObject.GetComponent<RectTransform>();
            // Configura anclajes y pivote para scroll vertical (pivote en la parte superior)
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0.5f, 1);

            // Configura el VerticalLayoutGroup para que los elementos NO se estiren y agrega margen
            var layoutGroup = gameObject.GetComponent<VerticalLayoutGroup>();
            layoutGroup.childForceExpandWidth = true;
            layoutGroup.childForceExpandHeight = true;
            layoutGroup.childAlignment = TextAnchor.UpperCenter;
            if(spacing != null) layoutGroup.spacing = spacing.Value;
            layoutGroup.childControlWidth = false;
            layoutGroup.childControlHeight = false;
            // Configura ContentSizeFitter para que ajuste su tamaño según el contenido
            var fitter = gameObject.GetComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

            return gameObject.GetComponent<RectTransform>();
        }
        public override Transform SubPoint() => gameObject.transform;
    }


}
