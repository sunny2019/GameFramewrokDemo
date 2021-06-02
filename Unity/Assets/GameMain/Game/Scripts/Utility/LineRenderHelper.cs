
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{

    public static class LineRenderHelper
    {
        public static LineRenderer AddLineRenderer(this GameObject o,float width,Color color)
        {
            LineRenderer renderer= o.AddComponent<LineRenderer>();
            
            renderer.startWidth = width;
            renderer.endWidth = width;
            renderer.startColor = color;
            renderer.endColor = color;
            
            
            renderer.positionCount = 0;
            renderer.numCornerVertices = 30;
            renderer.numCapVertices = 30;
            Material mat = new Material(Shader.Find("UI/Unlit/Detail"));
            renderer.material = mat;

            return renderer;
        }
    }
}