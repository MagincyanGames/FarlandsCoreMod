using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FarlandsCoreMod.UI.Components
{
    public abstract class RElement : IRenderizable
    {
        public string? name;
        public Vector2? position;

        public GameObject baseGameObject;

        public abstract string type { get; }
        public List<RElement> childs = new List<RElement>();

        public GameObject gameObjectForRender()
        {
            if(baseGameObject == null)
                return new GameObject(name);

            return baseGameObject;
        }
        public abstract Component Render();

        public Component RenderElement()
        {
            var render = Render();
            this.baseGameObject = render.gameObject;

            foreach (var child in childs)
            {
                var childBase = child.baseGameObject;
                var crender = child.RenderElement();

                if (childBase == null) crender.transform.SetParent(render.transform);
            }

            return render;
        }

        public void SetPositions()
        {
            if(baseGameObject == null) return;

            if (this is RRect rect) rect.RectPosition();

            if (position != null) baseGameObject.transform.localPosition = position.Value;
            this.baseGameObject.transform.localScale = new Vector3(1, 1, 1);

            foreach (var child in childs)
            {
                child.SetPositions();
            }
        }
    }
}
