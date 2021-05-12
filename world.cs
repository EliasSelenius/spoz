using Engine;
using Nums;
using System;
using System.Collections.Generic;


class Galaxy {

}

class SolarSystem {
    readonly List<Sector> sectors = new();
    
    Scene scene;
    Gameobject sun;
    bool isGenerated => scene != null;

    readonly int seed;
    int accSeed;
    float rand() => math.rand(accSeed++);
    float rand01() => math.rand(accSeed++) * 0.5f + 0.5f;
    float range(float min, float max) => math.range(accSeed++, min, max);
    

    static readonly Camera camera = new Gameobject(new Camera(), new CamOrbitControll()).getComponent<Camera>();

    public SolarSystem(int seed) {
        this.seed = seed;
        accSeed = seed;
    }

    void generate() {
        scene = new();

        //var sunMat = Assets.getMaterial("sun");
        var sunMat = new Material(Assets.getShader("unlit"));
        var sunColor = new vec4(7,2, 0,1);
        sunMat.setdata(sunColor);
        sun = scene.createObject(
            new MeshRenderer {
                mesh = Assets.getMesh("sphere"),
                materials = new[] { sunMat }
            },
            new Pointlight {
                color = sunColor.xyz * 100f
            }
        );

        int planetCount = (int)range(3, 10);
        for (int i = 0; i < planetCount; i++) {
            genPlanet(range(0, math.tau), 3 + i * 3);
        }

    }

    Gameobject genPlanet(float angle, float dist) {
        var p = scene.createObject(
            new MeshRenderer {
                mesh = Assets.getMesh("sphere"),
                materials = new[] {
                    new PBRMaterial {
                        albedo = (0.3f, 0.5f, 0.7f),
                        roughness = 0.7f
                    }
                }
            }
        );

        const float planetMinSize = 0.2f;
        const float planetMaxSize = 0.5f;

        p.transform.scale = range(planetMinSize, planetMaxSize);
        p.transform.position.xz = new vec2(math.cos(angle), math.sin(angle)) * dist;
        return p;
    }

    public void view() {
        if (!isGenerated) generate();

        camera.gameobject.enterScene(scene);
        Scene.active = scene;
    }

}

class Sector {
    Scene scene;

    void generate() {
        scene = new();


    }

}