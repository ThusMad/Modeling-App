using System.Collections.Generic;

namespace Modeler.Core.Models
{
    public class ChartModel
    {
        public GridSettings GridSettings { get; set; }
        public ICollection<ShapeBase> Figures { get; set; }
    }
}