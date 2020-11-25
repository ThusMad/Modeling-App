using System;
using Modeler.Core.Enums;
using Modeler.Core.Utilities;
using Newtonsoft.Json;
using SharpDX.Mathematics.Interop;

namespace Modeler.Core.Shapes
{
    public class Circle : ShapeBase
    {
        private int _radius;
        private int _precision;

        [JsonProperty("radius")]
        public int Radius
        {
            get => _radius;
            set => BuildCircle(CenterX, CenterY, value);
        }

        public Circle(RawColor4 color, float thickness = 1f) : base(0, 0, ShapeType.Circle, color, thickness)
        {
            CenterX = 0;
            CenterY = 0;
            Radius = 0;
        }

        public Circle(int x, int y, int radius, RawColor4 color, int precision = 360, float thickness = 1f) : base(x, y, ShapeType.Circle, color, thickness)
        {
            _precision = precision;

            BuildCircle(x, y, radius);
        }

        private void BuildCircle(int x, int y, int radius)
        {
            Data.Clear();

            if (_precision < 4 || _precision > 360)
            {
                _precision = 360;
            }

            _radius = radius;

            var angle = 360 / _precision;

            for (var i = 0; i <= _precision; i++)
            {
                Data.Add(new RawVector2(radius * (float)Math.Cos(Utility.DegToRad(angle * i)) + x,
                    radius * (float)Math.Sin(Utility.DegToRad(angle * i)) + y));
            }

            CalculateOuterBox();
        }
    }
}