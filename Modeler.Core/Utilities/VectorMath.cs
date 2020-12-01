using SharpDX.Mathematics.Interop;

namespace Modeler.Core.Utilities
{
    public static class VectorMath
    {
        public static RawVector2 Add(RawVector2 vec1, RawVector2 vec2)
        {
            return new RawVector2(vec1.X + vec2.X, vec1.Y + vec2.Y);
        }

        public static RawVector2 Add(RawVector2 vec1, float con)
        {
            return new RawVector2(vec1.X + con, vec1.Y + con);
        }

        public static RawVector2 Sub(RawVector2 vec1, float con)
        {
            return new RawVector2(vec1.X - con, vec1.Y - con);
        }

        public static RawVector2 Sub(RawVector2 vec1, RawVector2 vec2)
        {
            return new RawVector2(vec1.X - vec2.X, vec1.Y - vec2.Y);
        }

        public static RawVector2 Mul(RawVector2 vec1, float con)
        {
            return new RawVector2(vec1.X * con, vec1.Y * con);
        }

        public static RawVector2 Mul(RawVector2 vec1, RawVector2 vec2)
        {
            return new RawVector2(vec1.X * vec2.X, vec1.Y * vec2.Y);
        }
    }
}