using Engine.Components;
using Engine.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene_loading.Helpers
{
    public static class SceneImporter
    {
        public static Scene LoadJsonFile(string fileName)
        {
            var scene = new Scene
            {
                Meshes = new List<Mesh>()
            };

            var data = File.ReadAllText(fileName);
            var json = JObject.Parse(data);

            // Import polyhedrons.
            foreach(var jToken in (JArray) json["meshes"])
            {
                var meshJson = (JObject) jToken;

                if (meshJson["vertices"] == null)
                {
                    if(((string)meshJson["name"]).Equals("Cube"))
                    {
                        var mesh = new Cube((float) meshJson["length"], (float) meshJson["width"], (float) meshJson["height"])
                        {
                            Position = ParseVector3(meshJson["position"]),
                            Rotation = ParseQuaternion(meshJson["rotation"]),
                            Scaling = ParseVector3(meshJson["scaling"]),
                            Name = (string)meshJson["name"]
                        };

                        scene.Meshes.Add(mesh);
                    }
                    else if(((string)meshJson["name"]).Equals("Sphere"))
                    {
                        var mesh = new Sphere((float) meshJson["radius"], (int) meshJson["numberLongtitude"], (int) meshJson["numberLatitude"])
                        {
                            Position = ParseVector3(meshJson["position"]),
                            Rotation = ParseQuaternion(meshJson["rotation"]),
                            Scaling = ParseVector3(meshJson["scaling"]),
                            Name = (string)meshJson["name"]
                        };

                        scene.Meshes.Add(mesh);
                    }
                    else if (((string)meshJson["name"]).Equals("Cone"))
                    {
                        var mesh = new Cone((float) meshJson["height"], (float) meshJson["bottomRadius"], (float) meshJson["topRadius"], (int) meshJson["numberSides"], (int) meshJson["numberHeight"])
                        {
                            Position = ParseVector3(meshJson["position"]),
                            Rotation = ParseQuaternion(meshJson["rotation"]),
                            Scaling = ParseVector3(meshJson["scaling"]),
                            Name = (string)meshJson["name"]
                        };

                        scene.Meshes.Add(mesh);
                    }
                    else if (((string)meshJson["name"]).Equals("Cylinder"))
                    {
                        var mesh = new Cylinder((float) meshJson["height"], (float) meshJson["bottomRadius1"], (float)meshJson["bottomRadius2"], (float) meshJson["topRadius1"], (float) meshJson["topRadius2"], (int) meshJson["numberSides"])
                        {
                            Position = ParseVector3(meshJson["position"]),
                            Rotation = ParseQuaternion(meshJson["rotation"]),
                            Scaling = ParseVector3(meshJson["scaling"]),
                            Name = (string)meshJson["name"]
                        };

                        scene.Meshes.Add(mesh);
                    }
                }
                else
                {
                    // Plane
                    var verticesArray = (JArray)meshJson["vertices"];
                    var indicesArray = (JArray)meshJson["indices"];

                    var verticesCount = verticesArray.Count / 3;
                    var facesCount = indicesArray.Count / 3;

                    var mesh = new Mesh(verticesCount, facesCount)
                    {
                        Position = ParseVector3(meshJson["position"]),
                        Rotation = ParseQuaternion(meshJson["rotation"]),
                        Scaling = ParseVector3(meshJson["scaling"]),
                        Name = (string)meshJson["name"]
                    };

                    for (int i = 0; i < verticesCount; i++)
                    {
                        mesh.Vertices[i] = ParseVector3(verticesArray, i * 3);
                    }

                    // Import triangles.
                    for (int i = 0; i < facesCount; i++)
                    {
                        var a = (int)indicesArray[i * 3];
                        var b = (int)indicesArray[i * 3 + 1];
                        var c = (int)indicesArray[i * 3 + 2];

                        mesh.Faces[i] = new Face(a, b, c);
                    }

                    scene.Meshes.Add(mesh);
                }
            }

            // Import camera settings.
            var cameraTarget = ParseVector3(json["cameras"][0]["target"]);
            var cameraPosition = ParseVector3(json["cameras"][0]["position"]);

            scene.Camera = new Camera
            {
                Position = cameraPosition,
                //Rotation = ParseQuaternion(json["cameras"][0]["rotation"]),
                LookDirection = (cameraTarget - cameraPosition).Normalize(),
                FieldOfViewRadians = (float) json["cameras"][0]["fov"]
            };

            // Placing tthe "floor" grid hightest on the list.
            // The first one will be drawn, and thus it will be hidden behind the polyhedrons in the foreground.
            var plane = scene.Meshes.First(m => m.Name == "Plane");
            scene.Meshes.Remove(plane);
            scene.Meshes.Insert(0, plane);

            return scene;
        }

        private static Vector3 ParseVector3(JToken token, int offset = 0)
        {
            return new Vector3(
                (float) token[offset + 0],
                (float) token[offset + 1],
                (float) token[offset + 2]);
        }

        private static Quaternion ParseQuaternion(JToken token)
        {
            return Quaternion.RotationYawPitchRoll(
                (float) token[1],
                (float) token[0],
                (float) token[2]);
        }
    }
}
