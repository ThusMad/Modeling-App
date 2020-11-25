using System;
using SharpDX.Direct2D1;

namespace Modeler.Core.Models
{
    public class DebugDrawingModel
    {
        public DebugDrawingModel(Action<RenderTarget> drawAction)
        {
            DrawAction = drawAction;
        }

        public Action<RenderTarget> DrawAction { get; set; }

    }
}