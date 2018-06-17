using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Utilities
{
    // An extension that keeps a floating a point values to a range of 0-1.
    public static class SaturateExtensions
    {
        public static float Saturate(this float x)
        {
            if (x < 0) return 0;
            return x > 1 ? 1 : x;
        }

        public static double Saturate(this double x)
        {
            if (x < 0) return 0;
            return x > 1 ? 1 : x;
        }

        public static Color Saturate(this Color vector)
        {
            return new Color(
                Saturate(vector.R),
                Saturate(vector.G),
                Saturate(vector.B),
                Saturate(vector.A));
        }
    }
}
