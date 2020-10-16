using System;
using System.Linq;
using SharpDX.Mathematics.Interop;

namespace Modeler.Core.Utilities
{
    public static class TransformationExtensions
    {
        public static ShapeBase Rotate(this ShapeBase shape, double angle)
        {
            return shape.Rotate(angle, shape.CenterX, shape.CenterY);
        }

        public static ShapeBase Rotate(this ShapeBase shape, double angle, int x, int y)
        {
            var rad = (float)Utility.DegToRad(angle);

            for (var i = 0; i < shape.Data.Count; i++)
            {
                var data = shape.Data[i];
                shape.Data[i] = new RawVector2(x + (data.X - x) * (float)Math.Cos(rad) - (data.Y - y) * (float)Math.Sin(rad),
                    y + (data.Y - y) * (float)Math.Cos(rad) + (data.X - x) * (float)Math.Sin(rad));
            }

            return shape;
        }

        public static void Transform(this ShapeBase shape, int x, int y)
        {
            shape.CenterX -= x;
            shape.CenterY -= y;
        }


        public static void TransformX(this ShapeBase shape, int x)
        {
            shape.CenterX += x;
        }

        public static void TransformY(this ShapeBase shape, int y)
        {
            shape.CenterY += y;
        }

        public static void AffineTransformation(this ShapeBase shape, RawVector2 vector0, RawVector2 vectorX, RawVector2 vectorY)
        {
            var newData = shape.Data.Select(point =>
                    new RawVector2(vector0.X + (point.X * vectorX.X) + (point.Y * vectorY.X), vector0.Y - (point.Y * vectorX.Y) + point.Y * vectorY.Y))
                .ToList();

            shape.Data = newData;
        }

        public static void ProjectiveTransformation(this ShapeBase shape, RawVector3 vector0, RawVector3 vectorX, RawVector3 vectorY)
        {
            var newData = shape.Data.Select(point =>
                    new RawVector2((vector0.X * vector0.Z + vectorX.X * vectorX.Z * point.X + vectorY.X * vectorY.Z * point.Y) / (vector0.Z + vectorX.Z * point.X + vectorY.Z * point.Y),
                        (vector0.Y * vector0.Z + vectorX.Y * vectorX.Z * point.X + vectorY.Y * vectorY.Z * point.Y) / (vector0.Z + vectorX.Z * point.X + vectorY.Z * point.Y)))
                .ToList();

            shape.Data = newData;
        }
    }
}