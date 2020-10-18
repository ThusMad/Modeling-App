using System.Collections.Generic;
using Modeler.Core.Shapes;
using SharpDX.Mathematics.Interop;

namespace Modeler.Core.Utilities
{
    public static class GridGenerator
    {
        public static List<ShapeBase> GenerateGrid(int width, int height, int cellSize, IntPoint gridOffset)
        {
            var data = new List<ShapeBase>();
            var color = new RawColor4(0.94f, 0.94f, 0.94f, 1f);
            var axisColor = new RawColor4(0.54f, 0.54f, 0.54f, 1f);
            var gridSize = new IntPoint((int)(width * 0.98), (int)(height * 0.98));

            // draw Y axis
            data.Add(new Line(gridOffset.X + 1, gridOffset.Y, gridOffset.X + 1, gridSize.Y, axisColor));
            data.Add(new Line(gridOffset.X, gridOffset.Y + 1, gridOffset.X, gridSize.Y + 1, axisColor));
            data.Add(new Triangle(gridOffset.X, gridSize.Y + 22, gridOffset.X - 10, gridSize.Y, gridOffset.X + 10, gridSize.Y, axisColor));
            data.Add(new Text(gridOffset.X + 22, gridSize.Y + 5, "Y", new RawColor4(0f, 0f, 0f, 1)));

            // draw X axis
            data.Add(new Line(gridOffset.X, gridOffset.Y, gridSize.X, gridOffset.Y, axisColor));
            data.Add(new Triangle(gridSize.X + 20, gridOffset.Y, gridSize.X, gridOffset.Y + 10, gridSize.X, gridOffset.Y - 10, axisColor));
            data.Add(new Text(gridSize.X - 15, gridOffset.Y + 10, "X", new RawColor4(0f, 0f, 0f, 1)));

            for (var i = gridOffset.X; i <= gridSize.X; i += cellSize)
            {
                data.Add(new Line(i, gridOffset.Y, i, gridSize.Y, color));
            }

            for (var i = gridOffset.Y; i <= gridSize.Y; i += cellSize)
            {
                data.Add(new Line(gridOffset.X, i, gridSize.X, i, color));
            }

            return data;
        }
    }
}