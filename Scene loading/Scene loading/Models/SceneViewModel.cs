using Engine.Algorithms;
using Engine.Components;
using Engine.Utilities;
using Scene_loading.Helpers;
using Scene_loading.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Scene_loading.Models
{
    internal class SceneViewModel
    {
        // Cached bitmap, storing the image seen on the screen.
        private readonly BufferedBitmap _bmp;

        // Class rendering the scene.
        private readonly Device _device;

        private readonly Scene _scene;

        // View class allowing communication with the GUI.
        private readonly ISceneViewModel _view;

        private Camera Camera => _scene.Camera;

        // Prepares the bitmap and the scene.
        public SceneViewModel(ISceneViewModel view)
        {
            _view = view;
            _view.Loaded += OnLoaded;

            _bmp = new BufferedBitmap(750, 450);

            _device = new Device(_bmp, new Bresenham(), new LiangBarskyClipping());

            _scene = new Scene
            {
                Camera = new Camera
                {
                    Position = Vector3.Zero,
                    LookDirection = Vector3.UnitZ,
                    FieldOfView = 60
                }
            };

            _scene = SceneImporter.LoadJsonFile(Path.Combine("Resources", "scene.unity.babylon"));
            _scene.Meshes.First(m => m.Name == "Plane").Color = Engine.Utilities.Colors.DarkGrey;

            //_scene.Meshes.Add(new Cube());
            //_scene.Meshes.Add(new Sphere());
            //_scene.Meshes.Add(new Cone());
            //_scene.Meshes.Add(new Tube());
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateDebugInfo();

            _view.Render.Source = _bmp.BitmapSource;
            _view.MouseWheel += OnMouseWheel;
            _view.KeyDown += OnKeyDown;

            CompositionTarget.Rendering += CompositionTargetOnRendering;
        }

        // Updates the position of the camera and its rotation in the GUI.
        private void UpdateDebugInfo()
        {
            _view.SetCameraParams(Camera.Position, Camera.Rotation, Camera.FieldOfView);
        }

        // Renders frames from the 3D camera.
        private void CompositionTargetOnRendering(object sender, EventArgs e)
        {
            if (!_view.IsCameraVisible) return;

            _bmp.Clear(Engine.Utilities.Colors.Black.ToColor32());
            _device.Render(_scene); // Renders the frame of the 3D scene.
            _bmp.Present(); // Pushes the rendered bitmap to the GUI.
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            const float step = 0.2f;
            const float rot = 1f;

            switch (e.Key)
            {
                case Key.W:
                    Camera.Move(new Vector3(0, 0, step));
                    break;
                case Key.S:
                    Camera.Move(new Vector3(0, 0, -step));
                    break;
                case Key.A:
                    Camera.Move(new Vector3(-step, 0, 0));
                    break;
                case Key.D:
                    Camera.Move(new Vector3(step, 0, 0));
                    break;
                case Key.E:
                    Camera.Move(new Vector3(0, step, 0));
                    break;
                case Key.C:
                    Camera.Move(new Vector3(0, -step, 0));
                    break;
                case Key.K:
                    Camera.Rotate(Axis.Y, -rot);
                    break;
                case Key.OemSemicolon:
                    Camera.Rotate(Axis.Y, rot);
                    break;
                case Key.I:
                    Camera.Rotate(Axis.Z, rot);
                    break;
                case Key.P:
                    Camera.Rotate(Axis.Z, -rot);
                    break;
                case Key.O:
                    Camera.Rotate(Axis.X, rot);
                    break;
                case Key.L:
                    Camera.Rotate(Axis.X, -rot);
                    break;
            }

            UpdateDebugInfo();
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var steps = e.Delta / 120f;
            var angleDelta = steps * 5;

            Camera.FieldOfView -= angleDelta;
        }
    }
}