using System.Collections.Generic;
using SharpDX.Mathematics.Interop;

namespace Modeler.Core.Shapes
{
    public class Text : ShapeBase
    {
        public string Message { get; set; }

        public Text()
        {
            Data = new List<RawVector2>();
            CenterX = 0;
            CenterY = 0;
        }

        public Text(int x, int y, string text) : base(x, y)
        {
            Message = text;
            Data.Add(new RawVector2(x, y));
        }
    }
}