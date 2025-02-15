using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FarlandsCoreMod.UI.Components
{
    public abstract class RElement : IRenderizable
    {
        public string? name;
        public Vector2? position;

        public GameObject gameObject;

        public abstract string type { get; }

        public List<RElement> childs = new List<RElement>();

        public GameObject gameObjectForRender()
        {
            if(gameObject == null)
                return new GameObject(name);

            return gameObject;
        }
        public abstract Component Render();
        public abstract Transform SubPoint();

        public Component RenderElement(Transform parent)
        {
            gameObject = gameObjectForRender();
            gameObject.transform.SetParent(parent);

            var r = Render();
            gameObject.AddComponent<UIMakerElementComponent>().element = this;
            return r;
        }

        public Action<GameObject> OnEnable;
        public Action<GameObject> OnReload;
        public Action<GameObject, PointerEventData> OnPointerClick;

        public Action<GameObject, PointerEventData> OnPointerEnter;

        public Action<GameObject, PointerEventData> OnPointerExit;
        public void Reload() { if (OnReload != null) OnReload(gameObject); }

       
    }

    public class UIMakerElementComponent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public RElement element;
        public void OnPointerClick(PointerEventData eventData) { if (element != null && element.OnPointerClick != null) { element.OnPointerClick(gameObject, eventData); } }
        public void OnPointerEnter(PointerEventData eventData) { if (element != null && element.OnPointerEnter != null) { element.OnPointerEnter(gameObject, eventData); } }
        public void OnPointerExit(PointerEventData eventData) { if (element != null && element.OnPointerExit != null) { element.OnPointerExit(gameObject, eventData); } }
   
        public void OnEnable() { if(element != null && element.OnEnable != null) { element.OnEnable(gameObject); }}

    }
}
