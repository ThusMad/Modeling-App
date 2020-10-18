using Modeler.Core.Enums;
using SharpDX.Mathematics.Interop;

namespace Modeler.Core.Shapes
{
    public class Line : ShapeBase
    {
        public Line(RawColor4 color, float thickness = 1f) : base(0,0, ShapeType.Line, color, thickness)
        {
            CenterX = 0;
            CenterY = 0;
        }

        public Line(int x1, int y1, int x2, int y2, RawColor4 color, float thickness = 1f) : base(0, 0, ShapeType.Line, color, thickness)
        {
            CenterX = (x1 + x2) / 2;
            CenterY = (y1 + y2) / 2;

            Data.Add(new RawVector2(x1, y1));
            Data.Add(new RawVector2(x2, y2));
        }
    }
}