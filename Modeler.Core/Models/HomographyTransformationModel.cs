using SharpDX.Mathematics.Interop;

namespace Modeler.Core.Models
{
    public class HomographyTransformationModel
    {
        private RawVector3 _ry;
        private RawVector3 _rx;
        private RawVector3 _r0;

        public RawVector3 R0
        {
            get => _r0;
            set => _r0 = value;
        }

        public RawVector3 RX
        {
            get => _rx;
            set => _rx = value;
        }

        public RawVector3 RY
        {
            get => _ry;
            set => _ry = value;
        }

        public HomographyTransformationModel()
        {
            R0 = new RawVector3(0, 0, 0);
            RX = new RawVector3(0, 0, 0);
            RY = new RawVector3(0, 0, 0);
        }
    }
}