using System.Threading;
using Modeler.Core.Enums;
using SharpDX.Mathematics.Interop;

namespace Modeler.Core.Shapes
{
    public class Rectangle : ShapeBase
    {
        private int _height;
        private int _width;

        public Rectangle(RawColor4 color, float thickness = 1f) : base(0, 0, ShapeType.Rectangle, color, thickness)
        {
            CenterX = 0;
            CenterY = 0;
        }

        public Rectangle(int x, int y, int width, int height, RawColor4 color, float thickness = 1f)
            : base(0, 0, ShapeType.Rectangle, color, thickness)
        {
            CenterX = x + width / 2;
            CenterY = y + height / 2;

            _height = height;
            _width = width;

            Data.Add(new RawVector2(x, y));
            Data.Add(new RawVector2(x + width, y));
            Data.Add(new RawVector2(x + width, y + height));
            Data.Add(new RawVector2(x, y + height));
            Data.Add(new RawVector2(x, y));

            CalculateOuterBox();
        }

        public int Height
        {
            get => _height;
            set
            {
                _height = value;
                RecalculatePoints();
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                RecalculatePoints();
            }
        }

        private void RecalculatePoints()
        {
            var x = (int)Data[0].X;
            var y = (int)Data[0].Y;

            Data.Clear();

            CenterX = x + _width / 2;
            CenterY = y + _height / 2;

            Data.Add(new RawVector2(x, y));
            Data.Add(new RawVector2(x + _width, y));
            Data.Add(new RawVector2(x + _width, y + _height));
            Data.Add(new RawVector2(x, y + _height));
            Data.Add(new RawVector2(x, y));

            CalculateOuterBox();
        }
    }
}