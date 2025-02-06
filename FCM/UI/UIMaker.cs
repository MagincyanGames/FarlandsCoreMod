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
        public RElement root;
        public Stack<RElement> point = new();

        public UIMaker()
        {
            root = new RCanvas();
            point.Push(root);
        }
        public UIMaker(RCanvas canvas)
        {
            root = canvas;
            point.Push(root);
        }

        public void End()
        {
            root.RenderElement();
            root.SetPositions();
        }

        public UIMaker Open(RElement element)
        {
            point.Peek().childs.Add(element);
            point.Push(element);
            return this;
        }
        public UIMaker Point(string path)
        {
            var gameObject = GameObjects.Find(path);  
            return Point(gameObject);
        }
        public UIMaker Point(GameObject gameObject)
        {
            var element = new RNone() { baseGameObject=gameObject};
            point.Peek().childs.Add(element);
            point.Push(element);
            return this;
        }
        public UIMaker Close()
        {
            point.Pop();
            return this;
        }
    }
}
