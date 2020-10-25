namespace Modeler.Core.Models
{
    public class ShapePreviewModel
    {
        public ShapePreviewModel(ShapeBase shape)
        {
            Shape = shape;
        }

        public ShapeBase Shape { get; set; }
    }
}