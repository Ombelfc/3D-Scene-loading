using Engine.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Utilities
{
    // 4*4 matrix, with floating point type values.
    public class Matrix
    {
        private const int Size = 4;
        public readonly float[,] Mat = new float[Size, Size];

        // Identity matrix
        public static Matrix Identity => new Matrix
        {
            Mat =
            {
                [0, 0] = 1,
                [1, 1] = 1,
                [2, 2] = 1,
                [3, 3] = 1
            }
        };

        /// <summary>
        /// Creates a tranformation matrix from a 2D coordinate system to a 3D coordiante system
        /// </summary>
        /// <param name="cameraPosition">
        /// Coordinates of the space where the camera eye is placed
        /// </param>
        /// <param name="cameraTarget">
        /// The point at which the camera is pointing (used to determine the direction)
        /// </param>
        /// <param name="cameraUpVector">
        /// A vector whose arrow determines where the "top" is in the coordinate system
        /// </param>
        /// <returns></returns>
        public static Matrix LookAtLH(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
        {
            var zaxis = (cameraTarget - cameraPosition).Normalize();
            var xaxis = Vector3.Cross(cameraUpVector, zaxis);
            var yaxis = Vector3.Cross(zaxis, xaxis);

            var result = Identity;
            result.Mat[0, 0] = xaxis.X;
            result.Mat[1, 0] = xaxis.Y;
            result.Mat[2, 0] = xaxis.Z;
            result.Mat[3, 0] = -Vector3.Dot(xaxis, cameraPosition);

            result.Mat[0, 1] = yaxis.X;
            result.Mat[1, 1] = yaxis.Y;
            result.Mat[2, 1] = yaxis.Z;
            result.Mat[3, 1] = -Vector3.Dot(yaxis, cameraPosition);

            result.Mat[0, 2] = zaxis.X;
            result.Mat[1, 2] = zaxis.Y;
            result.Mat[2, 2] = zaxis.Z;
            result.Mat[3, 2] = -Vector3.Dot(zaxis, cameraPosition);

            return result;
        }

        // Coordinates of the selected axis
        public Vector3 GetAxis(Axis axis)
        {
            var n = (int) axis;
            return new Vector3(Mat[0, n], Mat[1, n], Mat[2, n]);
        }

        /// <summary>
        /// Creates a perspective transformation matrix from 3D space to a projection space
        /// </summary>
        /// <param name="fieldOfViewY">
        /// Filed of view in radians taking values from 0 to PI
        /// </param>
        /// <param name="aspectRatio">
        /// The ratio of the width to the height of the screen
        /// </param>
        /// <param name="znearPlane">
        /// Distance of the front plane cutting the pyramid from th camera
        /// </param>
        /// <param name="zfarPlane">
        /// Distance of the back plane cutting the pyramid from the camera
        /// </param>
        /// <returns></returns>
        public static Matrix PrespectiveFovLH(float fieldOfViewY, float aspectRatio, float znearPlane, float zfarPlane)
        {
            var cotTheta = (float) (1f / Math.Tan(fieldOfViewY * 0.5f));
            var q = zfarPlane / (zfarPlane - znearPlane);

            var result = new Matrix();
            result.Mat[0, 0] = cotTheta / aspectRatio;
            result.Mat[1, 1] = cotTheta;
            result.Mat[2, 2] = q;
            result.Mat[2, 3] = 1f;
            result.Mat[3, 3] = -q * znearPlane;

            return result;
        }
        
        // Creates a rotation matrix for a given quaternion.
        public static Matrix RotationQuaternion(Quaternion rotation)
        {
            var xx = rotation.X * rotation.X;
            var yy = rotation.Y * rotation.Y;
            var zz = rotation.Z * rotation.Z;

            var xy = rotation.X * rotation.Y;
            var zw = rotation.Z * rotation.W;
            var zx = rotation.Z * rotation.X;
            var yw = rotation.Y * rotation.W;
            var yz = rotation.Y * rotation.Z;
            var xw = rotation.X * rotation.W;

            var result = Identity;

            result.Mat[0, 0] = 1.0f - 2.0f * (yy + zz);
            result.Mat[0, 1] = 2.0f * (xy + zw);
            result.Mat[0, 2] = 2.0f * (zx - yw);

            result.Mat[1, 0] = 2.0f * (xy - zw);
            result.Mat[1, 1] = 1.0f - 2.0f * (zz + xx);
            result.Mat[1, 2] = 2.0f * (yz + xw);

            result.Mat[2, 0] = 2.0f * (zx + yw);
            result.Mat[2, 1] = 2.0f * (yz - xw);
            result.Mat[2, 2] = 1.0f - 2.0f * (yy + xx);

            return result;
        }

        // Creates a translation matrix for a vector.
        public static Matrix Translation(Vector3 vector)
        {
            var result = Identity;
            result.Mat[3, 0] = vector.X;
            result.Mat[3, 1] = vector.Y;
            result.Mat[3, 2] = vector.Z;

            return result;
        }

        // Creates a scaling matrix for a given vector.
        public static Matrix Scaling(Vector3 vector)
        {
            var result = Identity;
            result.Mat[0, 0] = vector.X;
            result.Mat[1, 1] = vector.Y;
            result.Mat[2, 2] = vector.Z;

            return result;
        }

        // Multiplies 2 matrices.
        public static Matrix operator * (Matrix left, Matrix right)
        {
            var result = new Matrix();

            for(var i = 0; i < Size; i++)
            {
                for(var j = 0; j < Size; j++)
                {
                    for(var k = 0; k < Size; k++)
                    {
                        result.Mat[i, j] += left.Mat[i, k] * right.Mat[k, j];
                    }
                }
            }

            return result;
        }

        // Calculates the inverse of the current matrix.
        public Matrix Invert()
        {
            var b0 = Mat[2, 0] * Mat[3, 1] - Mat[2, 1] * Mat[3, 0];
            var b1 = Mat[2, 0] * Mat[3, 2] - Mat[2, 2] * Mat[3, 0];
            var b2 = Mat[2, 3] * Mat[3, 0] - Mat[2, 0] * Mat[3, 3];
            var b3 = Mat[2, 1] * Mat[3, 2] - Mat[2, 2] * Mat[3, 1];
            var b4 = Mat[2, 3] * Mat[3, 1] - Mat[2, 1] * Mat[3, 3];
            var b5 = Mat[2, 2] * Mat[3, 3] - Mat[2, 3] * Mat[3, 2];

            var d11 = Mat[1, 1] * b5 + Mat[1, 2] * b4 + Mat[1, 3] * b3;
            var d12 = Mat[1, 0] * b5 + Mat[1, 2] * b2 + Mat[1, 3] * b1;
            var d13 = Mat[1, 0] * -b4 + Mat[1, 1] * b2 + Mat[1, 3] * b0;
            var d14 = Mat[1, 0] * b3 + Mat[1, 1] * -b1 + Mat[1, 2] * b0;

            var det = Mat[0, 0] * d11 - Mat[0, 1] * d12 + Mat[0, 2] * d13 - Mat[0, 3] * d14;

            if (Math.Abs(det) < float.Epsilon)
            {
                return new Matrix();
            }

            det = 1f / det;

            var a0 = Mat[0, 0] * Mat[1, 1] - Mat[0, 1] * Mat[1, 0];
            var a1 = Mat[0, 0] * Mat[1, 2] - Mat[0, 2] * Mat[1, 0];
            var a2 = Mat[0, 3] * Mat[1, 0] - Mat[0, 0] * Mat[1, 3];
            var a3 = Mat[0, 1] * Mat[1, 2] - Mat[0, 2] * Mat[1, 1];
            var a4 = Mat[0, 3] * Mat[1, 1] - Mat[0, 1] * Mat[1, 3];
            var a5 = Mat[0, 2] * Mat[1, 3] - Mat[0, 3] * Mat[1, 2];

            var d21 = Mat[0, 1] * b5 + Mat[0, 2] * b4 + Mat[0, 3] * b3;
            var d22 = Mat[0, 0] * b5 + Mat[0, 2] * b2 + Mat[0, 3] * b1;
            var d23 = Mat[0, 0] * -b4 + Mat[0, 1] * b2 + Mat[0, 3] * b0;
            var d24 = Mat[0, 0] * b3 + Mat[0, 1] * -b1 + Mat[0, 2] * b0;

            var d31 = Mat[3, 1] * a5 + Mat[3, 2] * a4 + Mat[3, 3] * a3;
            var d32 = Mat[3, 0] * a5 + Mat[3, 2] * a2 + Mat[3, 3] * a1;
            var d33 = Mat[3, 0] * -a4 + Mat[3, 1] * a2 + Mat[3, 3] * a0;
            var d34 = Mat[3, 0] * a3 + Mat[3, 1] * -a1 + Mat[3, 2] * a0;

            var d41 = Mat[2, 1] * a5 + Mat[2, 2] * a4 + Mat[2, 3] * a3;
            var d42 = Mat[2, 0] * a5 + Mat[2, 2] * a2 + Mat[2, 3] * a1;
            var d43 = Mat[2, 0] * -a4 + Mat[2, 1] * a2 + Mat[2, 3] * a0;
            var d44 = Mat[2, 0] * a3 + Mat[2, 1] * -a1 + Mat[2, 2] * a0;

            var result = new Matrix();
            result.Mat[0, 0] = +d11 * det;
            result.Mat[0, 1] = -d21 * det;
            result.Mat[0, 2] = +d31 * det;
            result.Mat[0, 3] = -d41 * det;
            result.Mat[1, 0] = -d12 * det;
            result.Mat[1, 1] = +d22 * det;
            result.Mat[1, 2] = -d32 * det;
            result.Mat[1, 3] = +d42 * det;
            result.Mat[2, 0] = +d13 * det;
            result.Mat[2, 1] = -d23 * det;
            result.Mat[2, 2] = +d33 * det;
            result.Mat[2, 3] = -d43 * det;
            result.Mat[3, 0] = -d14 * det;
            result.Mat[3, 1] = +d24 * det;
            result.Mat[3, 2] = -d34 * det;
            result.Mat[3, 3] = +d44 * det;

            return result;
        }
    }
}
