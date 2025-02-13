using FarlandsCoreMod.Extensors;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FarlandsCoreMod.UI.Components
{
    public class RCheckmark : RRect
    {
        public override string type => "checkmark";
        public bool? isChecked;
        public string? label;
        public Action<bool> onValueChanged;
        public float? fontSize;
        public Color? labelColor = Color.black;

        public override Component Render()
        {
            base.Render();

            // Agregar el componente Toggle al gameObject principal.
            Toggle toggle = gameObject.AddComponent<Toggle>();

            // Crear el fondo de la casilla (Background) como hijo del gameObject.
            GameObject backgroundGO = new GameObject("Background");
            backgroundGO.transform.SetParent(gameObject.transform);
            backgroundGO.transform.localScale = Vector3.one;
            RectTransform backgroundRT = backgroundGO.TryAddComponent<RectTransform>();
            // Ajustar para que el fondo ocupe todo el espacio del padre.
            backgroundRT.anchorMin = new Vector2(0, 0);
            backgroundRT.anchorMax = new Vector2(1, 1);
            backgroundRT.offsetMin = Vector2.zero;
            backgroundRT.offsetMax = Vector2.zero;

            Image backgroundImage = backgroundGO.AddComponent<Image>();
            backgroundImage.color = Color.white; // Color del fondo (puedes personalizarlo)
            toggle.targetGraphic = backgroundImage;

            // Crear el tick (Checkmark) como hijo del fondo para que se renderice sobre él.
            GameObject checkmarkGO = new GameObject("Checkmark");
            checkmarkGO.transform.SetParent(backgroundGO.transform);
            checkmarkGO.transform.localScale = Vector3.one;
            RectTransform checkmarkRT = checkmarkGO.TryAddComponent<RectTransform>();
            // Configurar anclas para que el tick ocupe una parte proporcional del fondo.
            checkmarkRT.anchorMin = new Vector2(0.25f, 0.25f);
            checkmarkRT.anchorMax = new Vector2(0.75f, 0.75f);
            checkmarkRT.offsetMin = Vector2.zero;
            checkmarkRT.offsetMax = Vector2.zero;

            Image checkmarkImage = checkmarkGO.AddComponent<Image>();
            checkmarkImage.color = Color.black; // Color del tick
            toggle.graphic = checkmarkImage;

            // Configurar el estado inicial del toggle.
            toggle.isOn = isChecked.HasValue ? isChecked.Value : false;
            toggle.onValueChanged.AddListener((bool value) => onValueChanged?.Invoke(value));

            // Crear la etiqueta (Label) si se especifica.
            if (!string.IsNullOrEmpty(label))
            {
                GameObject labelGO = new GameObject("Label");
                labelGO.transform.SetParent(gameObject.transform);
                labelGO.transform.localScale = Vector3.one;
                RectTransform labelRT = labelGO.TryAddComponent<RectTransform>();
                // Posicionar la etiqueta a la derecha del toggle.
                labelRT.anchorMin = new Vector2(1, 0);
                labelRT.anchorMax = new Vector2(1, 1);
                labelRT.pivot = new Vector2(0, 0.5f);
                labelRT.anchoredPosition = new Vector2(10, 0);
                labelRT.sizeDelta = new Vector2(100, size.Value.y);

                TextMeshProUGUI tmpro = labelGO.AddComponent<TextMeshProUGUI>();
                tmpro.text = label;
                tmpro.fontSize = fontSize ?? 14f;
                tmpro.color = labelColor ?? Color.black;
                tmpro.alignment = TextAlignmentOptions.Left;
            }

            return gameObject.GetComponent<RectTransform>();
        }
    }
}
