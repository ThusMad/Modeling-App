using Modeler.Core.Enums;
using SharpDX.Mathematics.Interop;

namespace Modeler.Core.Shapes
{
    public class Triangle : ShapeBase
    {
        public Triangle(RawColor4 color, float thickness = 1f)
            : base(0, 0, ShapeType.Triangle, color, thickness)
        {
            CenterX = 0;
            CenterY = 0;
        }

        public Triangle(int x1, int y1, int x2, int y2, int x3, int y3, RawColor4 color, float thickness = 1f) 
            : base(0, 0, ShapeType.Triangle, color, thickness)
        {
            CenterX = (x1 + x2 + x3) / 3;
            CenterY = (y1 + y2 + y3) / 3;

            Data.Add(new RawVector2(x1, y1));
            Data.Add(new RawVector2(x2, y2));
            Data.Add(new RawVector2(x3, y3));
            Data.Add(new RawVector2(x1, y1));

            CalculateOuterBox();
        }
    }
}