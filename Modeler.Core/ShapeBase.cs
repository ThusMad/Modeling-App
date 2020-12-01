using System;
using System.Collections.Generic;
using System.Linq;
using Modeler.Core.Attributes;
using Modeler.Core.Converters;
using Modeler.Core.Enums;
using Modeler.Core.Shapes;
using Modeler.Core.Utilities;
using Newtonsoft.Json;
using SharpDX.Mathematics.Interop;
using Helper = Modeler.Core.Utilities.Utility;

namespace Modeler.Core
{
    public abstract class ShapeBase : ICloneable
    {
        public bool IsMouseOver { get; set; }

        public bool IsPrimary { get; set; }

        private int _centerX;
        private int _centerY;

        public RawRectangle OuterBox { get; protected set; }

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

        public void CalculateOuterBox()
        {
            var left = (int)Data.Min(a => a.X) - 2;
            var right = (int)Data.Max(a => a.X) + 2;
            var top = (int)Data.Min(a => a.Y) - 2;
            var bottom = (int)Data.Max(a => a.Y) + 2;

            OuterBox = new RawRectangle(left, top, right, bottom);
        }

        public IList<RawVector2> SplitLine()
        {
            var points = new List<RawVector2>();

            for (int i = 1; i < Data.Count; i++)
            {
                var a = Data[i - 1];
                var b = Data[i];

                var distance = Helper.GetDistance(a.X, a.Y, b.X, b.Y);

                if (Math.Abs(distance) > 2)
                {
                    var count = (int)(distance / 2);

                    var d = Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y)) / count;
                    var fi = Math.Atan2(b.Y - a.Y, b.X - a.X);

                    for (int j = 0; j <= count; ++j)
                        points.Add(new RawVector2((int)(a.X + j * d * Math.Cos(fi)), (int)(a.Y + j * d * Math.Sin(fi))));
                }
                else
                {
                    points.Add(a);
                    points.Add(b);
                }
            }

            return points;
        }

        [JsonProperty("data")]
        [JsonConverter(typeof(VectorConverter))]
        public List<RawVector2> Data { get; set; }

        [ShapeEdit("Center Y", ShapeViewEditor.Text)]
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

        [ShapeEdit("Center X", ShapeViewEditor.Text)]
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

        public object Clone()
        {
            var newObj = new Line(0, 0, 0, 0, Color, Thickness);
            newObj.Data.Clear();
            this.Data.ForEach(dat =>
            {
                newObj.Data.Add(dat);
            });

            return newObj;
        }

        public virtual ICollection<RawVector2> BuildTangen(RawVector2 point, int length)
        {
            return new List<RawVector2>();
        }

        public virtual ICollection<RawVector2> BuildNormal(RawVector2 point, int length)
        {
            return new List<RawVector2>();
        }
    }
}