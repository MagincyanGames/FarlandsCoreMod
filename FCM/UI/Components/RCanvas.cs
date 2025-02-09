using FarlandsCoreMod.Extensors;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FarlandsCoreMod.UI.Components
{
    public class RCanvas : RElement
    {
        public override string type => "canvas";
        public int? renderOrder = null;
        public float? scaleFactor = null;
        public bool? pixelPerfect = null;
        public RenderMode? renderMode = null;

        public float? CanvasScaler_scaleFactor = null;
        public CanvasScaler.ScaleMode? CanvasScaler_uiScaleMode = null;
        public CanvasScaler.ScreenMatchMode? CanvasScaler_screenMatchMode = null;
        public float? CanvasScaler_referencePixelPerUnit = null;
        public Vector2? CanvasScaler_referenceResolution = null;
       
        //Terminar de agregar atributos

        public override Component Render()
        {

            var canvas = gameObjectForRender().TryAddComponent<Canvas>();
            var canvasScaler = canvas.gameObject.TryAddComponent<CanvasScaler>();
            if (renderOrder != null) canvas.sortingOrder = renderOrder.Value;
            if (renderMode != null) canvas.renderMode = renderMode.Value;
            if (pixelPerfect != null) canvas.pixelPerfect = pixelPerfect.Value;
            if (scaleFactor != null) canvas.scaleFactor = scaleFactor.Value;

            if(CanvasScaler_scaleFactor != null) canvasScaler.scaleFactor = CanvasScaler_scaleFactor.Value;
            if (CanvasScaler_uiScaleMode != null) canvasScaler.uiScaleMode = CanvasScaler_uiScaleMode.Value;
            if (CanvasScaler_screenMatchMode != null) canvasScaler.screenMatchMode = CanvasScaler_screenMatchMode.Value;
            if(CanvasScaler_referencePixelPerUnit != null) canvasScaler.referencePixelsPerUnit = CanvasScaler_referencePixelPerUnit.Value;
            if(CanvasScaler_referenceResolution != null) canvasScaler.referenceResolution = CanvasScaler_referenceResolution.Value;
            return canvas;
        }

        public override Transform SubPoint() => gameObject.transform;
    }
}
