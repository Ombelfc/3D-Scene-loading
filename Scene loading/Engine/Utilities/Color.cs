﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Utilities
{
    // Template representing the RGBA color.
    public class Color<T>
    {
        private readonly T[] _components;

        public Color(T r, T g, T b, T a)
        {
            _components = new[] { r, g, b, a };
        }

        public T R => _components[0];

        public T G => _components[1];

        public T B => _components[2];

        public T A => _components[3];
    }

    // Template representing the RGBA color with 8-bit components.
    public class Color32 : Color<byte>
    {
        public Color32(byte r, byte g, byte b, byte a) : base(r, g, b, a) { }
    }

    // Template representing the RGBA color with floating point components.
    public class Color : Color<float>
    {
        public Color() : base(0, 0, 0, 0) { }

        public Color(float r, float g, float b, float a) : base(r.Saturate(), g.Saturate(), b.Saturate(), a.Saturate()) { }

        protected static Color Map(Color left, Color right, Func<float, float, float> func)
        {
            return new Color(
                func.Invoke(left.R, right.R),
                func.Invoke(left.G, right.G),
                func.Invoke(left.B, right.B),
                func.Invoke(left.A, right.A));
        }

        protected static Color Mix(Color left, Color right)
        {
            var func = new Func<float, float, float>((l, r) => l * left.A * (1 - right.A) + r * right.A);

            return new Color(
                func.Invoke(left.R, right.R),
                func.Invoke(left.G, right.G),
                func.Invoke(left.B, right.B),
                func.Invoke(1, 1));
        }

        protected static Color Map(float left, Color right, Func<float, float, float> func)
        {
            return new Color(
                func.Invoke(left, right.R),
                func.Invoke(left, right.G),
                func.Invoke(left, right.B),
                func.Invoke(left, right.A));
        }

        public static Color operator * (Color left, Color right) => Map(left, right, (a, b) => a * b);
        public static Color operator + (Color left, Color right) => Map(left, right, (a, b) => a + b);
        public static Color operator - (Color left, Color right) => Map(left, right, (a, b) => a - b);
        public static Color operator * (float left, Color right) => Map(left, right, (a, b) => a * b);
        public static Color operator + (float left, Color right) => Map(left, right, (a, b) => a * b);
        public static Color operator - (float left, Color right) => Map(left, right, (a, b) => a * b);

        public Color32 ToColor32()
        {
            return new Color32(
                (byte) (byte.MaxValue * R.Saturate()),
                (byte) (byte.MaxValue * G.Saturate()),
                (byte) (byte.MaxValue * B.Saturate()),
                (byte) (byte.MaxValue * A.Saturate()));
        }
    }

    // A collection of sample colors.
    public static class Colors
    {
        public static Color Black => new Color(0, 0, 0, 1);
        public static Color Yellow => new Color(1, 1, 0, 1);
        public static Color Red => new Color(1, 0, 0, 1);
        public static Color Blue => new Color(0, 0, 1, 1);
        public static Color Green => new Color(0, 1, 0, 1);
        public static Color DarkGrey => new Color(0.3f, 0.3f, 0.3f, 1);
    }
}
