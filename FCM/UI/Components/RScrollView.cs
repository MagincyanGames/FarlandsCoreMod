using FarlandsCoreMod.Extensors;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FarlandsCoreMod.UI.Components
{
    public class RScrollView : RImage
    {
        public override string type => "scrollView";
        public bool? horizontal = null;
        public bool? vertical = null;
        public ScrollRect.MovementType? movementType = null;
        public float? elasticity = null;
        public bool? inertia = null;
        public float? decelerationRate = null;
        public float? scrollSensitivity = null;

        public override Component Render()
        {
            base.Render();
            // Añade el componente ScrollRect al GameObject
            var scrollRect = gameObject.TryAddComponent<ScrollRect>();

            // Crea el viewport, que define el área visible y que actuará como máscara
            GameObject viewportGO = new GameObject("Viewport", typeof(RectTransform), typeof(Image), typeof(RectMask2D));
            viewportGO.transform.SetParent(gameObject.transform, false);
            RectTransform viewportRect = viewportGO.GetComponent<RectTransform>();
            // Configura el viewport para que ocupe todo el espacio del GameObject padre
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.offsetMin = new Vector2(10,10);
            viewportRect.offsetMax = new Vector2(-10, -10);
            // No se asigna sizeDelta manualmente, se usará el estiramiento de anclajes
            // Ajusta la imagen del viewport para que sea casi transparente y no interfiera con los clics
            Image viewportImage = viewportGO.GetComponent<Image>();
            viewportImage.color = new Color(1, 1, 1, 0.01f);
            viewportImage.raycastTarget = false;

            // Crea el content, contenedor para los elementos desplazables
            // Se añade ContentSizeFitter para ajustar automáticamente el tamaño según su contenido
            GameObject contentGO = new GameObject("Content", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
            contentGO.transform.SetParent(viewportGO.transform, false);
            RectTransform contentRect = contentGO.GetComponent<RectTransform>();
            // Configura anclajes y pivote para scroll vertical (pivote en la parte superior)
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0.5f, 1);

            // Configura el VerticalLayoutGroup para que los elementos NO se estiren y agrega margen
            var layoutGroup = contentGO.GetComponent<VerticalLayoutGroup>();
            layoutGroup.childForceExpandWidth = true;
            layoutGroup.childForceExpandHeight = true;
            layoutGroup.childAlignment = TextAnchor.UpperCenter;
            layoutGroup.spacing = 6;
            layoutGroup.childControlWidth = false;
            layoutGroup.childControlHeight = false;
            // Configura ContentSizeFitter para que ajuste su tamaño según el contenido
            var fitter = contentGO.GetComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

            // Asigna el viewport y el content al ScrollRect
            scrollRect.viewport = viewportRect;
            scrollRect.content = contentRect;

            // Configura las propiedades del ScrollRect según los valores asignados (si se proporcionan)
            if (horizontal != null) scrollRect.horizontal = horizontal.Value;
            if (vertical != null) scrollRect.vertical = vertical.Value;
            if (movementType != null) scrollRect.movementType = movementType.Value;
            if (elasticity != null) scrollRect.elasticity = elasticity.Value;
            if (inertia != null) scrollRect.inertia = inertia.Value;
            if (decelerationRate != null) scrollRect.decelerationRate = decelerationRate.Value;
            if (scrollSensitivity != null) scrollRect.scrollSensitivity = scrollSensitivity.Value;

            return scrollRect;
        }

        public override Transform SubPoint() => gameObject.GetComponent<ScrollRect>().content;
    }
}
