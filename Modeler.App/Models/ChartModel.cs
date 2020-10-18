using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;
using System.Windows.Shapes;
using Modeler.Core;

namespace Modeler.App.Models
{
    public class ChartModel
    {
        private StreamWriter _stream;
        private int _cellSize = 10;

        public ChartModel(StreamWriter fileStream)
        {
            _stream = fileStream;
        }

        public ChartModel(StreamWriter fileStream, int cellSize)
        {
            _stream = fileStream;
            _cellSize = cellSize;
        }

        public List<ShapeBase> Shapes { get; set; }
        public List<ShapeBase> Grid { get; set; }

        public void OnSizeChanged(int newWidth, int newHeight)
        {

        }
    }
}