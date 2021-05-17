
using Nums;
using Engine;

static class Player {
    static Gameobject camera;
    public readonly static Gameobject ship;
    
    static Player() {
        ship = new Gameobject(new ShipController(), new PlayerController(), new SphereCollider {radius = 5f});
        //var playerMesh = Assets.getPrefab("spoz.data.models.TheFrog.Cube").createInstance(); 
        var playerMesh = Assets.getPrefab("spoz.data.models.kitbash.sample").createInstance();
        playerMesh.transform.rotate(vec3.unity, math.pi);
        playerMesh.transform.rotate(vec3.unitx, -math.pi / 2f);
        ship.addChild(playerMesh);
        ship.transform.rotation = quat.identity;

        // test marker:
        void makeMarker(float dst) {
            var marker = new Gameobject(
                new MeshRenderer() { mesh = Assets.getMesh("sphere"), materials = new[] { PBRMaterial.defaultMaterial }}
            );
            marker.transform.position = (0,0,dst);
            marker.transform.scale *= 0.1f;
            ship.addChild(marker);
        }
        makeMarker(10f);
        //makeMarker(20f);
        makeMarker(30f);
        //makeMarker(40f);
        makeMarker(50f);



        camera = new Gameobject(new Camera(), new CameraController(ship.transform));
    }

    public static void enterScene(Scene scene) {
        ship.enterScene(scene);
        camera.enterScene(scene);
    }
}

class PlayerController : Component {

    ShipController sc;

    protected override void onStart() {
        sc = gameobject.getComponent<ShipController>();
    }

    protected override void onUpdate() {
        
        // wasd move controll:

        sc.input(Keyboard.getAxis(key.S, key.W));

        if (!Keyboard.isDown(key.LeftAlt)) {
            var mousedelta = Mouse.delta;
            sc.rotate(
                yaw: mousedelta.x,
                pitch: -mousedelta.y,
                roll: Keyboard.getAxis(key.D, key.A)
            );
        }


        transform.rotation.normalize();

        //MyMath.slerp(transform.rotation, scene.camera.transform.rotation, Application.deltaTime * 10f, out transform.rotation);
        


    }
}