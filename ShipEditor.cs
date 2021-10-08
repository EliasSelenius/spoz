using Nums;
using Engine;
using Engine.Gui;
using System.Collections.Generic;
using System.Linq;

/*
    snap points 
    mirror
    rotate part
    scale part
    hilight selection
*/

class ShipEditor : Component {

    static new Scene scene;
    static Canvas canvas = new(100, 100);

    static Gameobject selectedObj, selectedObjParent;
    static Dictionary<string, Prefab> parts;

    
    static bool isValidPlace;

    static ShipEditor() {
        scene = new();
        scene.createObject(new Camera { canvas = canvas }, new CamOrbitControll());
        scene.createObject(new ShipEditor());


        parts = Assets.colladaFiles["spoz.data.models.kitbash.dae"].prefabs;

        ScreenRaycast.filter = r => r.gameobject.rootParent != selectedObj;


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

        selectedObjParent = Player.ship;
    }

    void drawPartSelectionMenu() {
        int i = 0;
        foreach (var partKV in parts) {
            int fontSize = 40;
            var size = new vec2(Text.length(partKV.Key, 0, partKV.Key.Length, fontSize, Font.arial), fontSize);
            var pos = new vec2(100, 50 + (size.y + 10) * i);
            canvas.rect(pos, size, in color.white);
            canvas.text(pos, Font.arial, fontSize, partKV.Key, in color.black);

            if (Utils.insideBounds(Mouse.position - pos, size) && Mouse.isPressed(MouseButton.left)) {

                selectedObj = partKV.Value.createInstance();
                selectedObj.transform.rotate(quat.fromAxisangle(vec3.unity, math.pi) * quat.fromAxisangle(vec3.unitx, -math.pi / 2f));
                selectedObj.enterScene(scene);
            }
            i++;
        }
    }

    protected override void onUpdate() {

        // controll for getting out of editor
        if (Keyboard.isPressed(key.Escape)) {
            Player.enterScene(spoz.Program.testScene);
            Scene.active = spoz.Program.testScene;
        }


        if (selectedObj == null) {
            drawPartSelectionMenu();

            // select obj
            if (Mouse.isPressed(MouseButton.left)) {
                ScreenRaycast.onHit(hit => {
                    selectedObj = hit.renderer.gameobject;                    
                    selectedObj.parent.removeChild(selectedObj, preserveTransform: true);
                });
            }
        } else {

            if (!isValidPlace) {
                // TODO: place object under cursor
                //selectedObj.transform.position = (new vec4(Mouse.ndcPosition, 1f, 1f) * scene.camera.viewMatrix).xyz;
            }

            if (Mouse.isReleased(MouseButton.left)) {
                if (isValidPlace) {
                    selectedObjParent.addChild(selectedObj, preserveTransform: true);
                } else selectedObj.destroy();
                selectedObj = null;
                selectedObjParent = Player.ship;
            }

            isValidPlace = false;

            ScreenRaycast.onHit(hit => {
                isValidPlace = true;
                selectedObjParent = hit.renderer.gameobject;

                if (selectedObj == null) return;

                selectedObj.transform.position = hit.position;

                quat r = quat.fromAxisangle(vec3.unity, math.pi) * quat.fromAxisangle(vec3.unitx, -math.pi / 2f);
                r.normalize();
                selectedObj.transform.rotation = r;


                // take surface normal of hit, and project it along the xy plane 
                var projNormal = hit.normal;
                projNormal.z = 0;
                projNormal.normalize();

                // set rotation of object acording to the projected surface normal
                /*var rotm = new mat3(
                    -projNormal, 
                    projNormal.cross(vec3.unitz), 
                    vec3.unitz);*/
                var rotm = new mat3(
                    projNormal.cross(vec3.unitz), 
                    projNormal, 
                    vec3.unitz);

                quat.fromMatrix(rotm, out r);
                selectedObj.transform.rotate(r);

            });
        }

    }


}