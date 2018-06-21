using Engine.Algorithms;
using Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Components
{
    public class Device
    {
        protected readonly IClippingAlgorithm ClippingAlgorithm;
        protected readonly ILineDrawingAlgorithm LineDrawingAlgorithm;
        protected IBufferedBitmap Bitmap;

        public Device(IBufferedBitmap bmp, ILineDrawingAlgorithm lineDrawing, IClippingAlgorithm clippingAlgorithm)
        {
            Bitmap = bmp;
            LineDrawingAlgorithm = lineDrawing;
            ClippingAlgorithm = clippingAlgorithm;
            ClippingAlgorithm.SetBoundingRectangle(new Vector2(0, 0), new Vector2(Bitmap.PixelWidth, Bitmap.PixelHeight));
        }

        // Converts 3D coordinates to 2D coordinates.
        // Using the transformation matrix for later rasterization.
        public Vector2 Project(Vector3 coord, Matrix transMat)
        {
            var point = Vector3.TransformCoordinate(coord, transMat);

            // We transform the point (based on the corrdiante system)
            // To have x:0, y:0 starting from the top left
            var x = Bitmap.PixelWidth * (point.X + 0.5f);
            var y = Bitmap.PixelHeight * (-point.Y + 0.5f);

            return new Vector2(x, y);
        }

        public void Render(Scene scene)
        {
            // Order of transformation:
            // 1. Object space: In this space there are models at the beginning, they have no position or rotation.
            // 2. World space: A common space in which the camera and all models are located, after giving them coordiantes and rotation.
            // 3. View space: Coordinate space with respect to the camera, which is located at (0, 0, 0)
            // 4. Projection space: After this transformation, the objects seen by the camera gain perspective.

            // Matrix transformation from 2. to 3.
            var viewMatrix = scene.Camera.LookAtLH();

            // Matrix transformation from 3. to 4.
            var projectionMatrix = Matrix.PrespectiveFovLH(
                scene.Camera.FieldOfViewRadians,
                Bitmap.AspectRatio,
                scene.Camera.ZNear,
                scene.Camera.ZFar);

            foreach(var mesh in scene.Meshes)
            {
                // Transformation matrix from 1. to 2.
                // First we apply the rotation and then the transformation
                var worldMatrix = Matrix.Scaling(mesh.Scaling) * Matrix.RotationQuaternion(mesh.Rotation) * Matrix.Translation(mesh.Position);

                // Matrix multiplication combining all transformations in the correct order.
                var transformMatrix = worldMatrix * viewMatrix * projectionMatrix;

                // 3D coordinantes to 2D coordinantes on a bitmap.
                var pixels = mesh.Vertices.Select(vertex => Project(vertex, transformMatrix)).ToArray();

                var vertices = mesh.Vertices.Select(vertex => Vector3.TransformCoordinate(vertex, worldMatrix * viewMatrix)).ToArray();

                var color32 = mesh.Color.ToColor32();

                // Iterate over the triangles.
                foreach(var face in mesh.Faces)
                {
                    if (vertices[face.A].Z < scene.Camera.ZNear || 
                        vertices[face.B].Z < scene.Camera.ZNear || 
                        vertices[face.C].Z < scene.Camera.ZNear) continue;

                    face.Edges((a, b) =>
                    {
                        var p1 = pixels[a];
                        var p2 = pixels[b];

                        if(ClippingAlgorithm.ClipLine(ref p1, ref p2))
                        {
                            // Draw the grid lines.
                            LineDrawingAlgorithm.DrawLine(p1, p2, (x, y) => Bitmap.DrawPoint(x, y, color32));
                        }                       
                    });
                }
            }
        }
    }
}
