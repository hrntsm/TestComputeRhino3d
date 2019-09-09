using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Compute;

namespace TestCompute {
    class Program {
        static void Main(string[] args) {
            ComputeServer.AuthToken = "Input your　AuthToken";

            var model = new Rhino.FileIO.File3dm();

            // Uses standard Rhino3dmIO methods to create a sphere and add Brep data to model
            var sphere = new Rhino.Geometry.Sphere(Rhino.Geometry.Point3d.Origin, 12);
            var sphereAsBrep = sphere.ToBrep();
            model.Objects.AddBrep(sphereAsBrep);

            // the following function calls compute.rhino3d to get access to something not
            // available in Rhino3dmIO. In this case send Brep to compute and get mesh back.
            var meshes = MeshCompute.CreateFromBrep(sphereAsBrep);

            // Use regular Rhino3dmIO local calls to count the vertices in the mesh
            Console.WriteLine($"got {meshes.Length} meshes");
            for (int i = 0; i < meshes.Length; i++) {
                Console.WriteLine($" {i + 1} mesh has {meshes[i].Vertices.Count} vertices");

                //Add meshes data to model
                model.Objects.AddMesh(meshes[i]);
            }

            // Output Rhino 3dm file
            var path = System.IO.Directory.GetCurrentDirectory() + "/Outputs/model.3dm";
            model.Write(path, 5);

            Console.WriteLine("press any key to exit");
            Console.ReadKey();
        }
    }
}
