using FarlandsCoreMod.Extensors;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FarlandsCoreMod.UI.Components
{
    public class RHorizontalGroup : RImage
    {
        // Cambié el tipo a "horizontalGroup" en lugar de "verticalGroup"
        public override string type => "horizontalGroup";
        public float? spacing;  // Espaciado entre los elementos

        public override Component Render()
        {
            base.Render();

            // Añadir los componentes necesarios
            gameObject.TryAddComponent<HorizontalLayoutGroup>();
            gameObject.TryAddComponent<ContentSizeFitter>();

            RectTransform contentRect = gameObject.GetComponent<RectTransform>();

            // Configuración de anclajes para asegurar que el grupo esté alineado correctamente
            contentRect.anchorMin = new Vector2(0, 0);    // Comienza desde el lado izquierdo
            contentRect.anchorMax = new Vector2(1, 1);    // Termina en el lado derecho
            contentRect.pivot = new Vector2(0.5f, 0.5f);  // Pivote centrado en ambos ejes

            // Configura el HorizontalLayoutGroup
            var layoutGroup = gameObject.GetComponent<HorizontalLayoutGroup>();
            layoutGroup.childForceExpandWidth = false;   // Evitar que los elementos se expandan
            layoutGroup.childForceExpandHeight = true;  // Evitar que los elementos se expandan en altura
            layoutGroup.childAlignment = TextAnchor.MiddleCenter;  // Alineación centrada
            if (spacing != null) layoutGroup.spacing = spacing.Value; // Asignar el espaciado si se ha dado

            layoutGroup.childControlWidth = false;  // Controlar el ancho de los elementos
            layoutGroup.childControlHeight = false; // Controlar la altura de los elementos

            // Configura el ContentSizeFitter
            var fitter = gameObject.GetComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize; // Ajuste según el contenido
            fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained; // Sin restricción en el eje Y

            return gameObject.GetComponent<RectTransform>();
        }

        // Método que devuelve el transform del objeto
        public override Transform SubPoint() => gameObject.transform;
    }
}
