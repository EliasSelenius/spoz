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
    public float rand() => math.rand(accSeed++);
    public float rand01() => math.rand(accSeed++) * 0.5f + 0.5f;
    public float range(float min, float max) => math.range(accSeed++, min, max);
    

    static readonly Camera camera = new Gameobject(new Camera(), new CamOrbitControll()).getComponent<Camera>();

    public SolarSystem(int seed) {
        this.seed = seed;
        accSeed = seed;
    }

    void generate() {
        scene = new();

        // sun:
        //var sunMat = Assets.getMaterial("sun");
        var sunMat = new Material(Assets.getShader("unlit"));
        var sunColor = new vec4(10, 4, 1,1);
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


        // planets:
        int planetCount = (int)range(3, 10);
        float accDist = 1; // sun radius
        for (int i = 0; i < planetCount; i++) {
            scene.createObject(new Planet(this, i, ref accDist));
        }

    }


    public void view() {
        if (!isGenerated) generate();

        camera.gameobject.enterScene(scene);
        Scene.active = scene;
    }

}

class Planet : Component {
    
    public const float planetMinSize = 0.2f;
    public const float planetMaxSize = 0.5f;

    SolarSystem ss;

    public float angle, distance, radius;

    public Planet(SolarSystem ss, int planetIndex, ref float accDist) {
        this.ss = ss;

        radius = ss.range(planetMinSize, planetMaxSize);

        angle = ss.range(0, math.tau);

        //distance = 3 + planetIndex * 3;
        distance = accDist += radius * (4 + ss.range(0, 3));
    }

    protected override void onStart() {
        
        
        transform.scale = radius;


        var mr = gameobject.requireComponent<MeshRenderer>();
        mr.mesh = Assets.getMesh("sphere");
        mr.materials = new[] {
            new PBRMaterial {
                albedo = (0.3f, 0.5f, 0.7f),
                roughness = 0.7f
            }
        };
    }

    protected override void onUpdate() {


        var speed = Application.deltaTime; 
        angle += speed / distance;

        transform.position.xz = new vec2(math.cos(angle), math.sin(angle)) * distance;

        Engine.Toolset.Gizmo.color(in color.gray);
        Engine.Toolset.Gizmo.circle(in vec3.zero, in vec3.unity, distance);

    }
}

class Sector {
    Scene scene;

    void generate() {
        scene = new();


    }

}