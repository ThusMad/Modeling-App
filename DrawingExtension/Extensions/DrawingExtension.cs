using System;
using Primitives;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DrawingExtension.Extensions
{
    public static class DrawingExtension
    {
        public static void DrawShape(this RenderTarget target, ShapeBase shape, int stroke = 1, SolidColorBrush brush = null)
        {
            if (brush == null)
            {
                brush = new SolidColorBrush(target, new RawColor4(0, 0, 0, 1));
            }
            for (var i = 1; i < shape.Data.Count; i++)
            {
                target.DrawLine(shape.Data[i - 1], shape.Data[i], brush, stroke);
            }
        }
    }
}