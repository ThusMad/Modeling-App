using Modeler.Core.Shapes;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using Brush = SharpDX.Direct2D1.Brush;

namespace Modeler.Core.Utilities
{
    public static class DrawingExtensions
    {
        public static RenderTarget DrawShape(this RenderTarget target, ShapeBase shape, int stroke = 1,
            SolidColorBrush brush = null, int xshift = 0, int yshift = 0)
        {
            if (brush == null)
            {
                brush = new SolidColorBrush(target, new RawColor4(0, 0, 0, 1));
            }

            if (shape is Text text)
            {
                using (var factoryDWrite = new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared))
                {
                    var textFormat = new TextFormat(factoryDWrite, "Tahoma", SharpDX.DirectWrite.FontWeight.DemiBold, SharpDX.DirectWrite.FontStyle.Normal, SharpDX.DirectWrite.FontStretch.Normal, 12);
                    target.DrawText(text.Message, textFormat, new RawRectangleF(text.Data[0].X, text.Data[0].Y, text.Data[0].X + (text.Message.Length * 20), text.Data[0].Y + (text.Message.Length * 20)), new SolidColorBrush(target, text.Color));
                }

                return target;
            }

            for (var i = 1; i < shape.Data.Count; i++)
            {
                target.DrawLine(
                    new RawVector2(shape.Data[i - 1].X + xshift, shape.Data[i - 1].Y + yshift), 
                    new RawVector2(shape.Data[i].X + xshift, shape.Data[i].Y + yshift), 
                    brush, 
                    stroke);
            }

            return target;
        }
    }
}