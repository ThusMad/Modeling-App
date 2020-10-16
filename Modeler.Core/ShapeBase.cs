using System.Collections.Generic;
using System.Linq;
using SharpDX.Mathematics.Interop;

namespace Modeler.Core
{
    public abstract class ShapeBase
    {
        private int _centerX;
        private int _centerY;

        protected ShapeBase()
        {
            Data = new List<RawVector2>();
        }

        protected ShapeBase(int centerX, int centerY)
        {
            Data = new List<RawVector2>();

            _centerX = centerX;
            _centerY = centerY;
        }

        public List<RawVector2> Data { get; set; }
        public int CenterX {
            get => _centerX;
            set
            {
                var diff = _centerX - value;
                _centerX = value;

                for (var i = 0; i < Data.Count; i++)
                {
                    Data[i] = new RawVector2(Data[i].X + diff, Data[i].Y);
                }
            }
        }
        public int CenterY
        {
            get => _centerY;
            set
            {
                var diff = _centerY - value;
                _centerY = value;

                for (var i = 0; i < Data.Count; i++)
                {
                    Data[i] = new RawVector2(Data[i].X, Data[i].Y + diff);
                }
            }
        }

        public override string ToString()
        {
            return string.Join(";", Data.Select(d => $"{{{d.X}, {d.Y}}}").ToArray());
        }
    }
}