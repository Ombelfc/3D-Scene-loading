using Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Components
{
    public class Scene
    {
        // Camera that observes the scene.
        public Camera Camera;

        // List of models appearing on the stage.
        public List<Mesh> Meshes = new List<Mesh>();
    }
}
