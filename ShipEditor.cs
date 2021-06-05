using Nums;
using Engine;
using Engine.Gui;
using System.Collections.Generic;
using System.Linq;

class ShipEditor : Component {

    static new Scene scene;
    static Canvas canvas = new(100, 100);

    static Gameobject objToBePlaced;
    static Dictionary<string, Prefab> parts;

    static ShipEditor() {
        scene = new();
        scene.createObject(new Camera { canvas = canvas }, new CamOrbitControll());

        var t = Assets.getPrefab("Engine.data.models.Ships.Brig").createInstance();
        //t.enterScene(scene);
        t.transform.position = vec3.zero;

        scene.createObject(new ShipEditor());


        parts = Assets.colladaFiles["spoz.data.models.kitbash.dae"].prefabs;

        ScreenRaycast.filter = r => r.gameobject != objToBePlaced;


        scene.dirlights.Add(new Dirlight {
            dir = vec3.unity
        });
    }

    public static void open() {
        Scene.active = scene;
        Player.ship.enterScene(scene);

        // TODO: make the editor camera be a child of ship instead, so to not override the transform, or maybe not?
        Player.ship.transform.position = vec3.zero;
        Player.ship.transform.rotation = quat.identity;

        //Player.ship.getComponent<ShipController>().disable();
        //Player.ship.getComponent<PlayerController>().disable();
        foreach (var c in Player.ship.components) c.disable();
    }


    protected override void onUpdate() {
        if (Keyboard.isPressed(key.Escape)) {
            Player.enterScene(spoz.Program.testScene);
            Scene.active = spoz.Program.testScene;
        }

        if (objToBePlaced == null) {
            objToBePlaced = parts.ElementAt(0).Value.createInstance();
            objToBePlaced.transform.rotate(quat.fromAxisangle(vec3.unity, math.pi) * quat.fromAxisangle(vec3.unitx, -math.pi / 2f));
            objToBePlaced.enterScene(scene);
        } else {

            ScreenRaycast.onHit(hit => {
                objToBePlaced.transform.position = hit.position;
                if (Mouse.isPressed(MouseButton.left)) {
                    Player.ship.addChild(objToBePlaced);
                    objToBePlaced = null;
                }
            });
        }

    }


}