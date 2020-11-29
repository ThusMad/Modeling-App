using System.Collections.Generic;
using Modeler.Core.Utilities;

namespace Modeler.Core.Models
{
    public class DrawModel
    {
        public List<ShapeBase> Shapes { get; set; }
        public List<ShapeBase> Grid { get; set; }
        public int XShift { get; set; }
        public int YShift { get; set; }
        public int RotationAngle { get; set; }
        public AffineTransformationModel AffineTransformation { get; set; }
        public HomographyTransformationModel HomographyTransformation { get; set; }
        public bool IsAffineActive { get; set; }
        public bool IsHomographyActive { get; set; }
        public IntPoint RotationPoint { get; set; }

        public DrawModel(List<ShapeBase> shapes, List<ShapeBase> grid)
        {
            AffineTransformation = new AffineTransformationModel();
            Shapes = shapes;
            Grid = grid;
        }
    }
}