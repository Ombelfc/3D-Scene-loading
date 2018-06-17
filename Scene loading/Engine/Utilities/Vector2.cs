using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Utilities
{
    public struct Vector2
    {
        public readonly float X;
        public readonly float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        // Length of the vector
        public float Length()
        {
            return (float) Math.Sqrt(X*X + Y*Y);
        }

        // Adding 2 vectors
        public static Vector2 operator + (Vector2 left, Vector2 right)
        {
            return new Vector2(left.X + right.X, left.Y + right.Y);
        }
        
        // Substracting 2 vectors
        public static Vector2 operator - (Vector2 left, Vector2 right)
        {
            return new Vector2(left.X - right.X, left.Y - right.Y);
        }
        
        // Multiplies the vector with a scalar
        public static Vector2 operator * (Vector2 left, float value)
        {
            return new Vector2(left.X * value, left.Y * value);
        }
    }
}
