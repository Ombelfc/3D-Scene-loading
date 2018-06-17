using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Utilities
{
    public struct Vector3
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 Zero => new Vector3(0, 0, 0);
        public static Vector3 One => new Vector3(1, 1, 1);

        public static Vector3 UnitX => new Vector3(1, 0, 0);
        public static Vector3 UnitY => new Vector3(0, 1, 0);
        public static Vector3 UnitZ => new Vector3(0, 0, 1);

        // Converts the vector into a unit vector
        // Direction is preserved but the length is 1
        public Vector3 Normalize()
        {
            return this * (1 / Length());
        }

        // Length of the vector
        public float Length()
        {
            return (float) Math.Sqrt(X*X + Y*Y + Z*Z);
        }

        // Converts the position vector using a tranformation matrix
        public static Vector3 TransformCoordinate(Vector3 coord, Matrix transf)
        {
            var x = coord.X * transf.Mat[0, 0] + coord.Y * transf.Mat[1, 0] + coord.Z * transf.Mat[2, 0] + transf.Mat[3, 0];
            var y = coord.X * transf.Mat[0, 1] + coord.Y * transf.Mat[1, 1] + coord.Z * transf.Mat[2, 1] + transf.Mat[3, 1];
            var z = coord.X * transf.Mat[0, 2] + coord.Y * transf.Mat[1, 2] + coord.Z * transf.Mat[2, 2] + transf.Mat[3, 2];
            var w = coord.X * transf.Mat[0, 3] + coord.Y * transf.Mat[1, 3] + coord.Z * transf.Mat[2, 3] + transf.Mat[3, 3];

            return new Vector3(x / w, y / w, z / w);
        }

        // Returns the dot product of 2 vectors
        public static float Dot(Vector3 left, Vector3 right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
        }

        // Returns the cross product of 2 vectors
        public static Vector3 Cross(Vector3 left, Vector3 right)
        {
            return new Vector3(
                left.Y * right.Z - left.Z * right.Y,
                left.Z * right.X - left.X * right.Z,
                left.X * right.Y - left.Y * right.X);
        }

        // Adding 2 vectors
        public static Vector3 operator + (Vector3 left, Vector3 right)
        {
            return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        // Subtracting 2 vectors
        public static Vector3 operator - (Vector3 left, Vector3 right)
        {
            return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        // Multiplies the vector with a scalar
        public static Vector3 operator * (Vector3 left, float value)
        {
            return new Vector3(left.X * value, left.Y * value, left.Z * value);
        }
    }
}
