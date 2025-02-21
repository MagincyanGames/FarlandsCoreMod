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
            GameObject textGO = new GameObject("Text");
            textGO.transform.SetParent(gameObject.transform);
            textGO.transform.localScale = Vector3.one;

            RectTransform textRect = textGO.AddComponent<RectTransform>();
            textRect.sizeDelta = size.Value;
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            TextMeshProUGUI tmpro = textGO.AddComponent<TextMeshProUGUI>();
            tmpro.alignment = TextAlignmentOptions.MidlineLeft;  // Alinear a la izquierda para mejor visibilidad del cursor
            tmpro.text = text ?? ""; // Establecer texto inicial
            tmpro.fontSize = fontSize ?? 14f;
            tmpro.color = Color.black;

            // Asignar el componente de texto al TMP_InputField
            inputField.textComponent = tmpro;

            // Crear y configurar el placeholder para el InputField
            GameObject placeholderGO = new GameObject("Placeholder");
            placeholderGO.transform.SetParent(gameObject.transform);
            placeholderGO.transform.localScale = Vector3.one;

            RectTransform placeholderRect = placeholderGO.AddComponent<RectTransform>();
            placeholderRect.sizeDelta = size.Value;
            placeholderRect.anchorMin = Vector2.zero;
            placeholderRect.anchorMax = Vector2.one;
            placeholderRect.offsetMin = Vector2.zero;
            placeholderRect.offsetMax = Vector2.zero;

            TextMeshProUGUI placeholderText = placeholderGO.AddComponent<TextMeshProUGUI>();
            placeholderText.alignment = TextAlignmentOptions.MidlineLeft;
            placeholderText.text = placeholder;
            placeholderText.fontSize = fontSize ?? 14f;
            placeholderText.color = placeholderColor ?? new Color(0.6f, 0.6f, 0.6f);

            // Asignar placeholder al InputField
            inputField.placeholder = placeholderText;

            // Habilitar el Caret
            inputField.caretBlinkRate = 0.85f;
            inputField.caretWidth = 2;

            // Configurar el InputField
            inputField.characterLimit = characterLimit ?? 20;
            inputField.lineType = lineType ?? TMP_InputField.LineType.SingleLine;
            inputField.onEndEdit.AddListener(value => onEndEdit?.Invoke(value));

            inputField.selectionColor = Color.gray; // Asegúrate de que el color de selección no sea igual al fondo.
            inputField.caretColor = Color.black; // Asegúrate de que el color del cursor sea visible.
            inputField.enabled = false;
            inputField.enabled = true;


            if (contentType.HasValue) inputField.contentType = contentType.Value;
            if (text != null) inputField.text = text;
            return gameObject.GetComponent<RectTransform>();
        }
    }
}