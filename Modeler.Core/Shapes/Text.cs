using System.Collections.Generic;
using Modeler.Core.Enums;
using Newtonsoft.Json;
using SharpDX.Mathematics.Interop;

namespace Modeler.Core.Shapes
{
    public class Text : ShapeBase
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        public Text(RawColor4 color, float thickness = 1f) : base(0, 0, ShapeType.Text, color, thickness)
        {
            Data = new List<RawVector2>();
            CenterX = 0;
            CenterY = 0;
        }

        public Text(int x, int y, string text, RawColor4 color, float thickness = 1f) : base(x, y, ShapeType.Text, color, thickness)
        {
            Message = text;
            Data.Add(new RawVector2(x, y));
        }
    }
}