using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Mathematics.Interop;

namespace Modeler.VectorExtension
{
    public struct RawVector2
    {
        public static RawVector2 operator +(RawVector2 c1, RawVector2 c2)
        {
            return new RawVector2 { X = c1.Value + c2.Value };
        }
    }
}
