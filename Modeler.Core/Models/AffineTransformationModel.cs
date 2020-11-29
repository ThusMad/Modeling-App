using SharpDX.Mathematics.Interop;

namespace Modeler.Core.Models
{
    public class AffineTransformationModel
    {
        private RawVector2 _ry;
        private RawVector2 _rx;
        private RawVector2 _r0;

        public RawVector2 R0
        {
            get => _r0;
            set => _r0 = value;
        }

        public RawVector2 RX
        {
            get => _rx;
            set => _rx = value;
        }

        public RawVector2 RY
        {
            get => _ry;
            set => _ry = value;
        }

        public AffineTransformationModel()
        {
            R0 = new RawVector2(0, 0);
            RX = new RawVector2(0, 0);
            RY = new RawVector2(0, 0);
        }
    }
}