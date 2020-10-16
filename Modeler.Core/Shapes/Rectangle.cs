using SharpDX.Mathematics.Interop;

namespace Modeler.Core.Shapes
{
    public class Rectangle : ShapeBase
    {
        public Rectangle()
        {
            CenterX = 0;
            CenterY = 0;
        }

        public Rectangle(int x, int y, int height, int width)
        {
            CenterX = x + width / 2;
            CenterY = y + height / 2;

            Data.Add(new RawVector2(x, y));
            Data.Add(new RawVector2(x + width, y));
            Data.Add(new RawVector2(x + width, y + height));
            Data.Add(new RawVector2(x, y + height));
            Data.Add(new RawVector2(x, y));
        }
    }
}