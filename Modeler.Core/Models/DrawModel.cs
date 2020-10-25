using System.Collections.Generic;

namespace Modeler.Core.Models
{
    public class DrawModel
    {
        public List<ShapeBase> Shapes { get; set; }
        public List<ShapeBase> Grid { get; set; }

        public DrawModel(List<ShapeBase> shapes, List<ShapeBase> grid)
        {
            Shapes = shapes;
            Grid = grid;
        }
    }
}