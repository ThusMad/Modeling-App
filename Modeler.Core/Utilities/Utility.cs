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
    }
}