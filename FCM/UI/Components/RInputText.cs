using FarlandsCoreMod.Extensors;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FarlandsCoreMod.UI.Components
{
    public class RInputText : RRect
    {
        public override string type => "textInput";
        public string? text;
        public string placeholder;
        public Action<string> onEndEdit;
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
            inputField.targetGraphic = gameObject.GetComponent<Image>();
            // Añadir el componente de texto (TextMeshPro) al InputField
            GameObject textGO = new GameObject("Text");
            textGO.transform.SetParent(gameObject.transform);
            textGO.transform.localScale = new Vector3(1, 1, 1);
            textGO.TryAddComponent<RectTransform>().sizeDelta = size.Value;

            TextMeshProUGUI tmpro = textGO.AddComponent<TextMeshProUGUI>();

            // Configurar el componente TextMeshProUGUI
            tmpro.alignment = TextAlignmentOptions.Center;  // Centrar el texto (opcional)
            tmpro.text = "Escribe algo...";  // Texto inicial (opcional)
            tmpro.fontSize = fontSize.Value;
            // Asignar el componente de texto al TMP_InputField
            tmpro.color = Color.black;
            inputField.textComponent = tmpro;

            // Crear el componente de placeholder para el TMP_InputField
            GameObject placeholderGO = new GameObject("Placeholder");
            placeholderGO.transform.SetParent(gameObject.transform);
            placeholderGO.transform.localScale = new Vector3(1, 1, 1);
            placeholderGO.TryAddComponent<RectTransform>().sizeDelta = size.Value;
            TextMeshProUGUI placeholder = placeholderGO.AddComponent<TextMeshProUGUI>();

            placeholder.fontSize = fontSize.Value;
            placeholder.alignment = TextAlignmentOptions.Center; 
            placeholder.text = "Placeholder";  // Texto del placeholder
            placeholder.color = new Color(0.6f, 0.6f, 0.6f);  // Color gris (opcional)
            inputField.placeholder = placeholder;

            // Configurar el InputField
            inputField.characterLimit = 20;  // Limitar el número de caracteres (opcional)
            inputField.lineType = TMP_InputField.LineType.SingleLine;  // Solo una línea (opcional

            inputField.onEndEdit.AddListener((string value) => onEndEdit?.Invoke(value));  // Evento de cambio de valor

            if (contentType.HasValue) inputField.contentType = contentType.Value;  // Tipo de contenido (opcional)
            if (text != null) inputField.text = text;
            return gameObject.GetComponent<RectTransform>();
        }
    }
}