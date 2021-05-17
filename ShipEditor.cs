using Nums;
using Engine;

class ShipEditor : Component {

    static new Scene scene;

    static ShipEditor() {
        scene = new();
        scene.createObject(new Camera(), new CamOrbitControll());

        var t = Assets.getPrefab("Engine.data.models.Ships.Brig").createInstance();
        //t.enterScene(scene);
        t.transform.position = vec3.zero;

        scene.createObject(new ShipEditor());

        scene.dirlights.Add(new Dirlight {
            dir = vec3.unity
        });
    }

    public static void open() {
        Scene.active = scene;
        Player.ship.enterScene(scene);
        Player.ship.getComponent<ShipController>().disable();
    }


    protected override void onUpdate() {
        ScreenRaycast.onHit(hit => {

        });
    }


}