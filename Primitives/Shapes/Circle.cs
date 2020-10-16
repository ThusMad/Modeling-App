using System;
using Primitives.Utilities;
using SharpDX.Mathematics.Interop;

namespace Primitives.Shapes
{
    public class Circle : ShapeBase
    {
        private int _radius;
        private int _precision;

        public int Radius
        {
            get => _radius;
            set
            {
                BuildCircle(CenterX, CenterY, value);
            }
        }

        public Circle()
        {
            CenterX = 0;
            CenterY = 0;
            Radius = 0;
        }

        public Circle(int x, int y, int radius, int precision = 360) : base(x, y)
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
        }
    }
}