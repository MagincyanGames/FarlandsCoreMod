using FarlandsCoreMod.Extensors;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FarlandsCoreMod.UI.Components
{
    public class RInputText : RImage
    {
        public override string type => "textInput";
        public string text;
        public string placeholder;
        public Action<string> onValueChanged;
        public int? characterLimit;
        public float? fontSize;
        public Color? placeholderColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        public TMP_InputField.ContentType? contentType;
        public TMP_InputField.LineType? lineType;
        public bool? readOnly;

        public override Component Render()
        {
            base.Render();

            // Añadir el componente TMP_InputField
            TMP_InputField inputField = gameObject.AddComponent<TMP_InputField>();

            // Añadir el componente de texto (TextMeshPro) al InputField
            GameObject textGO = new GameObject("Text");
            textGO.transform.SetParent(gameObject.transform);
            TextMeshProUGUI text = textGO.AddComponent<TextMeshProUGUI>();

            // Configurar el componente TextMeshProUGUI
            text.alignment = TextAlignmentOptions.Center;  // Centrar el texto (opcional)
            text.text = "Escribe algo...";  // Texto inicial (opcional)

            // Asignar el componente de texto al TMP_InputField
            inputField.textComponent = text;

            // Crear el componente de placeholder para el TMP_InputField
            GameObject placeholderGO = new GameObject("Placeholder");
            placeholderGO.transform.SetParent(gameObject.transform);
            TextMeshProUGUI placeholder = placeholderGO.AddComponent<TextMeshProUGUI>();

            // Configurar el componente Placeholder
            //placeholder.font = font;  // Asigna la fuente (opcional)
            placeholder.text = "Placeholder";  // Texto del placeholder
            placeholder.color = new Color(0.6f, 0.6f, 0.6f);  // Color gris (opcional)
            inputField.placeholder = placeholder;

            // Configurar el tamaño y la posición (opcional)
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(400, 40);  // Tamaño del InputField
            rectTransform.localPosition = new Vector3(0, 0, 0);  // Posición en el lienzo

            // Configurar el InputField
            inputField.characterLimit = 20;  // Limitar el número de caracteres (opcional)
            inputField.lineType = TMP_InputField.LineType.SingleLine;  // Solo una línea (opcional

            return gameObject.GetComponent<RectTransform>();
        }
    }
}