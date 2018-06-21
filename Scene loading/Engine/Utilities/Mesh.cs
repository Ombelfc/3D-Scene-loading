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
        public Vector3[] Vertices { get; set; }
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

    // Not used
    public class Cube1 : Mesh
    {
        public Cube1() : base(8, 12)
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

    public class Cube : Mesh
    {
        float length = 1f;
        float width = 1f;
        float height = 1f;

        public Cube(float length, float width, float height) : base(8, 10)
        {
            this.length = length;
            this.width = width;
            this.height = height;

            Vertices = GetVertices();
            Faces = GetFaces();
        }

        private Vector3[] GetVertices()
        {
            Vector3 p0 = new Vector3(-length * .5f, -width * .5f, height * .5f);
            Vector3 p1 = new Vector3(length * .5f, -width * .5f, height * .5f);
            Vector3 p2 = new Vector3(length * .5f, -width * .5f, -height * .5f);
            Vector3 p3 = new Vector3(-length * .5f, -width * .5f, -height * .5f);

            Vector3 p4 = new Vector3(-length * .5f, width * .5f, height * .5f);
            Vector3 p5 = new Vector3(length * .5f, width * .5f, height * .5f);
            Vector3 p6 = new Vector3(length * .5f, width * .5f, -height * .5f);
            Vector3 p7 = new Vector3(-length * .5f, width * .5f, -height * .5f);

            Vector3[] vertices = new Vector3[]
            {
	            // Bottom
        	    p0, p1, p2, p3,
 
	            // Left
	            p7, p4, p0, p3,
 
	            // Front
	            p4, p5, p1, p0,
 
	            // Back
	            p6, p7, p3, p2,
 
	            // Right
	            p5, p6, p2, p1,
 
	            // Top
	            p7, p6, p5, p4
            };

            return vertices;
        }

        private Face[] GetFaces()
        {
            Face[] faces = new Face[12];

            faces[0] = new Face(3, 1, 0);
            faces[1] = new Face(3, 2, 1);
            faces[2] = new Face(3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1);
            faces[3] = new Face(3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1);
            faces[4] = new Face(3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3);
            faces[5] = new Face(3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3);
            faces[6] = new Face(3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4);
            faces[7] = new Face(3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4);
            faces[8] = new Face(3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5);
            faces[9] = new Face(3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5);

            return faces;
        }
    }

    public class Sphere : Mesh
    {
        float radius = 1f;
        int nblong = 24;
        int nblat = 16;

        public Sphere(float radius, int numberLongtitude, int numberLatitude) : base(25 * 16 + 2, 2412)
        {
            this.radius = radius;
            this.nblong = numberLongtitude;
            this.nblat = numberLatitude;

            Vertices = GetVertices();
            Faces = GetFaces();
        }

        private Vector3[] GetVertices()
        {
            Vector3[] vertices = new Vector3[(nblong + 1) * nblat + 2];
            float _pi = (float) Math.PI;
            float _2pi = _pi * 2f;

            vertices[0] = Vector3.UnitX * radius;

            for(int lat = 0; lat < nblat; lat++)
            {
                float a1 = _pi * (float)(lat + 1) / (nblat + 1);
                float sin1 = (float) Math.Sin(a1);
                float cos1 = (float) Math.Cos(a1);

                for (int lon = 0; lon <= nblong; lon++)
                {
                    float a2 = _2pi * (float)(lon == nblong ? 0 : lon) / nblong;
                    float sin2 = (float) Math.Sin(a2);
                    float cos2 = (float) Math.Cos(a2);

                    vertices[lon + lat * (nblong + 1) + 1] = new Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius;
                }
            }

            vertices[vertices.Length - 1] = Vector3.UnitX * -radius;

            return vertices;
        }

        private Face[] GetFaces()
        {
            int nbfaces = Vertices.Length;
            int nbtriangles = nbfaces * 2;
            int nbindexes = nbtriangles * 3;

            //Console.WriteLine(nbindexes);
            Face[] faces = new Face[nbindexes];

            int i = 0;
            for(int lon = 0; lon < nblong; lon++)
            {
                faces[i++] = new Face(lon + 2, lon + 1, 0);
            }

            for(int lat = 0; lat < nblat - 1; lat++)
            {
                for(int lon = 0; lon < nblong; lon++)
                {
                    int current = lon + lat * (nblong + 1) + 1;
                    int next = current + nblong + 1;

                    faces[i++] = new Face(current, current + 1, next + 1);
                    faces[i++] = new Face(current, next + 1, next);
                }
            }

            for(int lon = 0; lon < nblong; lon++)
            {
                faces[i++] = new Face(Vertices.Length - 1, Vertices.Length - (lon + 2) - 1, Vertices.Length - (lon + 1) - 1);
            }

            return faces;
        }
    }

    public class Cone : Mesh
    {
        float height = 1f;
        float bottomRadius = .25f;
        float topRadius = .05f;

        static int nbsides = 18;
        int nbheightseg = 1;

        int nbverticescap = nbsides + 1;

        public Cone(float height, float bottomRadius, float topRadius, int numberSides, int numberHeightSeg) : base(76, 219)
        {
            this.height = height;
            this.bottomRadius = bottomRadius;
            this.topRadius = topRadius;
            nbsides = numberSides;
            this.nbheightseg = numberHeightSeg;

            Vertices = GetVertices();
            Faces = GetFaces();
        }

        private Vector3[] GetVertices()
        {
            Vector3[] vertices = new Vector3[nbverticescap + nbverticescap + nbsides * nbheightseg * 2 + 2];
            //Console.WriteLine(vertices.Length);
            int vert = 0;
            float _2pi = (float) Math.PI * 2f;

            // Bottom cap
            vertices[vert++] = new Vector3(0f, 0f, 0f);
            while(vert <= nbsides)
            {
                float rad = (float)vert / nbsides * _2pi;
                vertices[vert] = new Vector3((float) Math.Cos(rad) * bottomRadius, 0f, (float) Math.Sin(rad) * bottomRadius);
                vert++;
            }

            // Top cap
            vertices[vert++] = new Vector3(0f, height, 0f);
            while(vert < nbsides * 2 + 1)
            {
                float rad = (float)(vert - nbsides - 1) / nbsides * _2pi;
                vertices[vert] = new Vector3((float) Math.Cos(rad) * topRadius, height, (float) Math.Sin(rad) * topRadius);
                vert++;
            }

            // Sides
            int v = 0;
            while(vert <= vertices.Length - 4)
            {
                float rad = (float)v / nbsides * _2pi;
                vertices[vert] = new Vector3((float) Math.Cos(rad) * topRadius, height, (float) Math.Sin(rad) * topRadius);
                vertices[vert + 1] = new Vector3((float) Math.Cos(rad) * bottomRadius, 0, (float) Math.Sin(rad) * bottomRadius);
                vert += 2;
                v++;
            }

            vertices[vert] = vertices[nbsides * 2 + 2];
            vertices[vert + 1] = vertices[nbsides * 2 + 3];

            return vertices;
        }

        private Face[] GetFaces()
        {
            int nbtriangles = nbsides + nbsides + nbsides * 2;
            Face[] faces = new Face[nbtriangles * 3 + 3];
            //Console.WriteLine(nbtriangles * 3 + 3);

            // Bottom cap
            int tri = 0;
            int i = 0;
            while(tri < nbsides - 1)
            {
                faces[i] = new Face(0, tri + 1, tri + 2);
                tri++;
                i += 3;
            }
            faces[i] = new Face(0, tri + 1, 1);
            tri++;
            i += 3;

            // Top cap
            while(tri < nbsides * 2)
            {
                faces[i] = new Face(tri + 2, tri + 1, nbverticescap);
                tri++;
                i += 3;
            }
            faces[i] = new Face(nbverticescap + 1, tri + 1, nbverticescap);
            tri++;
            i += 3;
            tri++;

            // Sides
            while(tri <= nbtriangles)
            {
                faces[i] = new Face(tri + 2, tri + 1, tri);
                tri++;
                i += 3;
                faces[i] = new Face(tri + 1, tri + 2, tri);
                tri++;
                i += 3;
            }

            return faces;
        }
    }

    public class Cylinder : Mesh
    {
        float height = 1f;
        static int nbsides = 24;

        // Outer shell is at radius1 + radius2/2, inner shell is at radius 1 - radius 2/2;
        float bottomRadius1 = .5f;
        float bottomRadius2 = .15f;
        float topRadius1 = .5f;
        float topRadius2 = .15f;

        int nbVerticesCap = nbsides * 2 + 2;
        int nbVerticesSides = nbsides * 2 + 2;

        public Cylinder(float height, float bottomRadius1, float bottomRadius2, float topRadius1, float topRadius2, int numberSides) : base(200, 576)
        {
            this.height = height;
            this.bottomRadius1 = bottomRadius1;
            this.bottomRadius2 = bottomRadius2;
            this.topRadius1 = topRadius1;
            this.topRadius2 = topRadius2;
            nbsides = numberSides;

            Vertices = GetVertices();
            Faces = GetFaces();
        }

        private Vector3[] GetVertices()
        {
            Vector3[] vertices = new Vector3[nbVerticesCap * 2 + nbVerticesSides * 2];
            Console.WriteLine(vertices.Length);

            int vert = 0;
            float _2pi = (float) Math.PI * 2f;

            // Bottom cap
            int sideCounter = 0;
            while(vert < nbVerticesCap)
            {
                sideCounter = sideCounter == nbsides ? 0 : sideCounter;

                float r1 = (float)(sideCounter++) / nbsides * _2pi;
                float cos = (float) Math.Cos(r1);
                float sin = (float) Math.Sin(r1);
                vertices[vert] = new Vector3(cos * (bottomRadius1 - bottomRadius2 * .5f), 0f, sin * (bottomRadius1 - bottomRadius2 * .5f));
                vertices[vert + 1] = new Vector3(cos * (bottomRadius1 + bottomRadius2 * .5f), 0f, sin * (bottomRadius1 + bottomRadius2 * .5f));
                vert += 2;
            }

            // Top cap
            sideCounter = 0;
            while (vert < nbVerticesCap * 2)
            {
                sideCounter = sideCounter == nbsides ? 0 : sideCounter;

                float r1 = (float)(sideCounter++) / nbsides * _2pi;
                float cos = (float) Math.Cos(r1);
                float sin = (float) Math.Sin(r1);
                vertices[vert] = new Vector3(cos * (topRadius1 - topRadius2 * .5f), height, sin * (topRadius1 - topRadius2 * .5f));
                vertices[vert + 1] = new Vector3(cos * (topRadius1 + topRadius2 * .5f), height, sin * (topRadius1 + topRadius2 * .5f));
                vert += 2;
            }

            // Sides (out)
            sideCounter = 0;
            while (vert < nbVerticesCap * 2 + nbVerticesSides)
            {
                sideCounter = sideCounter == nbsides ? 0 : sideCounter;

                float r1 = (float)(sideCounter++) / nbsides * _2pi;
                float cos = (float) Math.Cos(r1);
                float sin = (float) Math.Sin(r1);

                vertices[vert] = new Vector3(cos * (topRadius1 + topRadius2 * .5f), height, sin * (topRadius1 + topRadius2 * .5f));
                vertices[vert + 1] = new Vector3(cos * (bottomRadius1 + bottomRadius2 * .5f), 0, sin * (bottomRadius1 + bottomRadius2 * .5f));
                vert += 2;
            }

            // Sides (in)
            sideCounter = 0;
            while (vert < vertices.Length)
            {
                sideCounter = sideCounter == nbsides ? 0 : sideCounter;

                float r1 = (float)(sideCounter++) / nbsides * _2pi;
                float cos = (float) Math.Cos(r1);
                float sin = (float) Math.Sin(r1);

                vertices[vert] = new Vector3(cos * (topRadius1 - topRadius2 * .5f), height, sin * (topRadius1 - topRadius2 * .5f));
                vertices[vert + 1] = new Vector3(cos * (bottomRadius1 - bottomRadius2 * .5f), 0, sin * (bottomRadius1 - bottomRadius2 * .5f));
                vert += 2;
            }

            return vertices;
        }

        private Face[] GetFaces()
        {
            int nbFace = nbsides * 4;
            int nbTriangles = nbFace * 2;
            int nbIndexes = nbTriangles * 3;
            Face[] faces = new Face[nbIndexes];
            Console.WriteLine(nbIndexes);

            // Bottom cap
            int i = 0;
            int sideCounter = 0;
            while (sideCounter < nbsides)
            {
                int current = sideCounter * 2;
                int next = sideCounter * 2 + 2;

                faces[i++] = new Face(next + 1, next, current);
                faces[i++] = new Face(current + 1, next + 1, current);

                sideCounter++;
            }

            // Top cap
            while (sideCounter < nbsides * 2)
            {
                int current = sideCounter * 2 + 2;
                int next = sideCounter * 2 + 4;

                faces[i++] = new Face(current, next, next + 1);
                faces[i++] = new Face(current, next + 1, current + 1);

                sideCounter++;
            }

            // Sides (out)
            while (sideCounter < nbsides * 3)
            {
                int current = sideCounter * 2 + 4;
                int next = sideCounter * 2 + 6;

                faces[i] = new Face(current, next, next + 1);
                faces[i] = new Face(current, next + 1, current + 1);

                sideCounter++;
            }


            // Sides (in)
            while (sideCounter < nbsides * 4)
            {
                int current = sideCounter * 2 + 6;
                int next = sideCounter * 2 + 8;

                faces[i++] = new Face(next + 1, next, current);
                faces[i++] = new Face(current + 1, next + 1, current);

                sideCounter++;
            }

            return faces;
        }
    }
}
