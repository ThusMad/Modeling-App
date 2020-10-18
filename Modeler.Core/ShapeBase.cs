using System.Collections.Generic;
using System.Linq;
using Modeler.Core.Converters;
using Modeler.Core.Enums;
using Newtonsoft.Json;
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

        protected ShapeBase(int centerX, int centerY, ShapeType type, RawColor4 color, float thickness = 1)
        {
            Data = new List<RawVector2>();

            Type = type;
            Color = color;
            Thickness = thickness;
            _centerX = centerX;
            _centerY = centerY;
        }

        [JsonProperty("data")]
        [JsonConverter(typeof(VectorConverter))]
        public List<RawVector2> Data { get; set; }
        [JsonProperty("center_y")]
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
        [JsonProperty("center_y")]
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
        [JsonProperty("type")]
        public ShapeType Type { get; private set; }
        [JsonProperty("color")]
        [JsonConverter(typeof(ColorConverter))]
        public RawColor4 Color { get; set; } = new RawColor4(0f, 0f, 0f, 1f);
        [JsonProperty("line_thickness")]
        public float Thickness { get; set; } = 1;
    }
}