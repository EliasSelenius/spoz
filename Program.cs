using System;
using Engine;
using Nums;


namespace spoz {
    class Program {

        public static Scene testScene;

        static void Main(string[] args) => Application.run(load);
        static void load() {
            Assets.load(new EmbeddedResourceProvider(typeof(Program).Assembly));

            
            { // generate sphere mesh
                var m = new Mesh<Vertex>(MeshFactory<Vertex>.genSphere(30, 1f));
                Assets.meshes.Add("sphere", m);
            }

            var ss = new SolarSystem(0);
            ss.view();

            createTestScene();
            
            ShipEditor.open();


            /*Scene.active.dirlights.Add(new Dirlight {
                dir = new vec3(7, 4, 5).normalized()
            });*/

        }


        static void createTestScene() {
            var s = testScene = new Scene();
            Scene.active = s;

            Player.enterScene(s);


            void addSphere(vec3 pos) {
                var g = new Gameobject(
                    new MeshRenderer {
                        mesh = Assets.getMesh("sphere"),
                        materials = new[] {
                            PBRMaterial.defaultMaterial
                        }
                    },
                    new SphereCollider {
                        radius = 10f
                    }
                );
                g.transform.position = pos;
                g.transform.scale *= 10;

                g.enterScene(s);
            }


            for (int i = 0; i < 100; i++) {
                addSphere(new vec3(math.rand(), math.rand() * 0.1f, math.rand()) * 300);
            }


            Assets.getPrefab("spoz.data.models.kitbash.sample").createInstance().enterScene(s);

            s.dirlights.Add(new Dirlight {
                dir = new vec3(7, 4, 5).normalized()
            });

        }

    }
}

static class MyMath {
    public static void slerp(in quat a, in quat b, float t, out quat q) {
        q = OpenTK.Mathematics.Quaternion.Slerp(a.toOpenTK(), b.toOpenTK(), t).toNums();
    }

    public static OpenTK.Mathematics.Quaternion toOpenTK(this quat q) => new OpenTK.Mathematics.Quaternion(q.x, q.y, q.z, q.w);
    public static quat toNums(this OpenTK.Mathematics.Quaternion q) => new quat(q.X, q.Y, q.Z, q.W);

    public static bool eq(in vec3 a, in vec3 b) => a.x == b.x && a.y == b.y && a.z == b.z;

}
