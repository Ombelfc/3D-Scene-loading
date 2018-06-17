using Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Scene_loading.Views
{
    public interface ISceneViewModel
    {
        Image Render { get; }
        bool IsCameraVisible { get; }
        void SetCameraParams(Vector3 position, Quaternion rotation, float fav);

        event RoutedEventHandler Loaded;
        event MouseWheelEventHandler MouseWheel;
        event KeyEventHandler KeyDown;
    }
}
