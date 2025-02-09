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
            return Render();
        }

    }
}
