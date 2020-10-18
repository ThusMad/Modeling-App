using System;
using Modeler.Core.Enums;
using Modeler.Core.Utilities;
using SharpDX.Mathematics.Interop;

namespace Modeler.Core.Shapes
{
    public class Arc : ShapeBase
    {
        public Arc(int x1, int y1, int x2, int y2, double angle, int precision, RawColor4 color, float thickness = 1) :
            base(0, 0, ShapeType.Arc, color, thickness)
        {
            var rad = Utility.DegToRad(angle);

            var mx = (x1 + x2) / 2;
            var my = (y1 + y2) / 2;

            CenterX = mx;
            CenterY = my;

            var dx = (x2 - x1) / 2;
            var dy = (y2 - y1) / 2;

            var len = Math.Sqrt(dx * dx + dy * dy);

            var px = -dy / len;
            var py = dx / len;

            double t;

            if (angle == 180)
            {
                t = 0;
            }
            else
            {
                t = len / Math.Tan(rad / 2);
            }

            var cx = mx + px * t;
            var cy = my + py * t;

            var p0x = x1 - cx;
            var p0y = y1 - cy;

            Data.Add(new RawVector2(x1, y1));

            for (var i = 0; i <= precision; i++)
            {
                var an = i * rad / precision;
                var xx = Math.Round(cx + p0x * Math.Cos(an) - p0y * Math.Sin(an));
                var yy = Math.Round(cy + p0x * Math.Sin(an) + p0y * Math.Cos(an));

                Data.Add(new RawVector2((float)xx, (float)yy));
            }

            Data.Add(new RawVector2(x2, y2));
        }
    }
}