using SharpDX.Mathematics.Interop;

namespace Primitives.Shapes
{
    public class Line : ShapeBase
    {
        public Line()
        {
            CenterX = 0;
            CenterY = 0;
        }

        public Line(int x1, int y1, int x2, int y2)
        {
            CenterX = (x1 + x2) / 2;
            CenterY = (y1 + y2) / 2;

            Data.Add(new RawVector2(x1, y1));
            Data.Add(new RawVector2(x2, y2));
        }
    }
}