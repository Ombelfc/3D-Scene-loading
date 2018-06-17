using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Utilities
{
    // Triangle with vertices in a 3D space.
    // Components A, B, C denote the vertex index in the Mesh class.
    public struct Face
    {
        public readonly int A;
        public readonly int B;
        public readonly int C;

        public Face(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }

        public void Edges(Action<int, int> action)
        {
            action.Invoke(A, B);
            action.Invoke(B, C);
            action.Invoke(A, C);
        }
    }

    // Represents a polyhedron with triangular walls.
    public class Mesh
    {
        #region Fields

        public string Name { get; set; }
        public Color Color { get; set; } = Colors.Yellow;
        public Vector3[] Vertices { get; }
        public Face[] Faces { get; set; }
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Quaternion Rotation { get; set; } = Quaternion.RotationYawPitchRoll(0, 0, 0);
        public Vector3 Scaling { get; set; } = Vector3.One;

        #endregion

        public Mesh(int verticesCount, int facesCount)
        {
            Vertices = new Vector3[verticesCount];
            Faces = new Face[facesCount];
        }
    }

    // Example of a polyhedron (cube) with 8 vertices and 12 walls.
    // The walls are twelve, because instead of squares the walls consist of triangles.
    public class Cube : Mesh
    {
        public Cube() : base(8, 12)
        {
            Vertices[0] = new Vector3(-1, 1, 1);
            Vertices[1] = new Vector3(1, 1, 1);
            Vertices[2] = new Vector3(-1, -1, 1);
            Vertices[3] = new Vector3(1, -1, 1);
            Vertices[4] = new Vector3(-1, 1, -1);
            Vertices[5] = new Vector3(1, 1, -1);
            Vertices[6] = new Vector3(1, -1, -1);
            Vertices[7] = new Vector3(-1, -1, -1);

            Faces[0] = new Face(0, 1, 2);
            Faces[1] = new Face(1, 2, 3);
            Faces[2] = new Face(1, 3, 6);
            Faces[3] = new Face(1, 5, 6);
            Faces[4] = new Face(0, 1, 4);
            Faces[5] = new Face(1, 4, 5);
            Faces[6] = new Face(2, 3, 7);
            Faces[7] = new Face(3, 6, 7);
            Faces[8] = new Face(0, 2, 7);
            Faces[9] = new Face(0, 4, 7);
            Faces[10] = new Face(4, 5, 6);
            Faces[11] = new Face(4, 6, 7);
        }
    }
}
