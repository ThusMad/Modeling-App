using System;

namespace Modeler.Core.Utilities
{
    public static class Utility
    {
        public static double DegToRad(double deg)
        {
            return deg * Math.PI / 180;
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static float GetDistance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }
    }
}