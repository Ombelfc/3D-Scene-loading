using Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Components
{
    // Axis of rotation.
    public enum Axis
    {
        X = 0,
        Y = 1,
        Z = 2
    }

    // Relative space
    public enum RelativeSpace
    {
        World,
        Camera
    }

    public class Camera
    {
        // The field of view of the camera in degrees.
        private float _fieldOfView = 60;

        // The position of the camera in the world space (World space).
        public Vector3 Position { get; set; }

        // The relative direction of the camera.
        public Vector3 LookDirection { get; set; } = Vector3.UnitZ;

        // Direction indicating the top (in the model space).
        public Vector3 UpDirection { get; set; } = Vector3.UnitY;

        // Rotation of the camera's viewing direction.
        public Quaternion Rotation { get; set; } = Quaternion.RotationYawPitchRoll(0, 0, 0);

        // Field of view. As values, it takes numbers between 0 and 180 degrees.
        public float FieldOfView
        {
            get { return _fieldOfView; }
            set
            {
                if (value > 0 && value < 180) _fieldOfView = value;
            }
        }

        // Field of view in radians.
        public float FieldOfViewRadians
        {
            get { return (float)(FieldOfView / 180 * Math.PI); }
            set { FieldOfView = (float) (value * 180 / Math.PI); }
        }

        // The distance of the front plane cutting the pyramid from the camera.
        public float ZNear { get; set; } = 0.01f;

        // The distance of the back plane cutting the pyramid from the camera.
        public float ZFar { get; set; } = 100f;

        // Creates a transformation matrix from 2D world space to 3D view space.
        public Matrix LookAtLH()
        {
            var cameraRotationMatrix = Matrix.RotationQuaternion(Rotation);

            var cameraLookDirection = Vector3.TransformCoordinate(LookDirection, cameraRotationMatrix);
            var cameraUpDirection = Vector3.TransformCoordinate(UpDirection, cameraRotationMatrix);

            return Matrix.LookAtLH(Position, Position + cameraLookDirection, cameraUpDirection);
        }

        // Moves the camera on any axis relative to the current position.
        public void Move(Vector3 move)
        {
            var viewMatrix = LookAtLH();
            var relativePosition = Vector3.TransformCoordinate(Position, viewMatrix);

            Position = Vector3.TransformCoordinate(relativePosition + move, viewMatrix.Invert());
        }

        // Rotates the camera relative to the current position.
        public void Rotate(Axis axisEnum, float angle, RelativeSpace space = RelativeSpace.Camera)
        {
            var radians = (float) Math.PI * angle / 180;

            Vector3 axis;
            switch (space)
            {
                case RelativeSpace.World:
                    switch (axisEnum)
                    {
                        case Axis.X:
                            axis = Vector3.UnitX;
                            break;

                        case Axis.Y:
                            axis = Vector3.UnitY;
                            break;

                        case Axis.Z:
                            axis = Vector3.UnitZ;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(axisEnum), axisEnum, null);
                    }
                    break;

                case RelativeSpace.Camera:
                    axis = LookAtLH().GetAxis(axisEnum);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(space), space, null);
            }

            Rotation = Quaternion.RotationAxis(axis, radians) * Rotation;
        }
    }
}
