using FarlandsCoreMod.UI.Components;
using FarlandsCoreMod.Utiles.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FarlandsCoreMod.UI
{
    public class UIMaker : MonoBehaviour
    {
        public Transform point;

        public UIMaker Point(string path)
        {
            this.point = GameObjects.Find(path).transform;
            return this;
        }
        public UIMaker Point(RElement point)
        {
            this.point = point.SubPoint();
            return this;
        }
        public UIMaker Point(GameObject point)
        {
            this.point = point.transform;
            return this;
        }
        public virtual UIMaker Render(RElement element)
        {
            var render = element.RenderElement(point);
            return this;
        }
        public virtual UIMaker RenderAndPoint(RElement element)
        {
            var render = element.RenderElement(point);
            this.point = element.SubPoint();
            return this;
        }
        public virtual UIMaker RenderAgainAndPoint(RElement element)
        {
            var render = RenderAgain(element);
            this.point = element.SubPoint();

            return this;
        }
        public virtual UIMaker RenderAgain(RElement element)
        {
            GameObject.DestroyImmediate(element.gameObject);
            Render(element);

            return this;
        }
    }
}
