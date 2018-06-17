using Engine.Utilities;
using Scene_loading.Models;
using Scene_loading.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Scene_loading.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ISceneViewModel
    {
        public MainWindow()
        {
            InitializeComponent();
            new SceneViewModel(this);
        }

        public Image Render => render;

        public bool IsCameraVisible => render.IsVisible;

        #region Events

        event KeyEventHandler ISceneViewModel.KeyDown
        {
            add
            {
                KeyDown += (sender, args) =>
                {
                    value.Invoke(sender, args);
                };
            }

            remove { }
        }

        #endregion

        #region Helpers

        public void SetCameraParams(Vector3 position, Quaternion rotation, float fov)
        {
            SetCoordinates(position, pCamPosX, pCamPosY, pCamPosZ);
            SetCoordinates(rotation, pCamRotX, pCamRotY, pCamRotZ);
            pZoom.Text = Math.Round(fov).ToString("0.0", CultureInfo.InvariantCulture);
        }

        private static void SetCoordinates(Vector3 vector, TextBlock x, TextBlock y, TextBlock z)
        {
            x.Text = vector.X.ToString("0.000", CultureInfo.InvariantCulture);
            y.Text = vector.Y.ToString("0.000", CultureInfo.InvariantCulture);
            z.Text = vector.Z.ToString("0.000", CultureInfo.InvariantCulture);
        }

        private static void SetCoordinates(Quaternion vector, TextBlock x, TextBlock y, TextBlock z)
        {
            x.Text = RadianToDegree(vector.Yaw).ToString("0.000", CultureInfo.InvariantCulture);
            y.Text = RadianToDegree(vector.Pitch).ToString("0.000", CultureInfo.InvariantCulture);
            z.Text = RadianToDegree(vector.Roll).ToString("0.000", CultureInfo.InvariantCulture);
        }

        private static double RadianToDegree(double angle)
        {
            return Math.Round(angle * (180.0f / Math.PI));
        }

        #endregion
    }
}