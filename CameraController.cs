using Nums;
using Engine;

class CameraController : Component {
    
    Transform target;
    vec2 angle;
    float zoom = 10f;
    float turnSpeed = 3f;

    public CameraController(Transform target) {
        this.target = target;
    }

    protected override void onStart() {
    }

    protected override void onUpdate() {

        //Mouse.state = Keyboard.isDown(key.LeftAlt) ? MouseState.free : MouseState.disabled;
        Mouse.state = MouseState.disabled;
        
        vec3 campos, lookpoint;

        if (Keyboard.isDown(key.LeftAlt)) {
            lookpoint = target.position;
            
            var d = Mouse.delta.yx * Application.deltaTime;
            d.y = -d.y;
            angle += d; 

            var cos = math.cos(angle);
            var sin = math.sin(angle);
            vec3 relpos = (sin.y * cos.x, sin.x, cos.y * cos.x);
            target.getMatrix(out mat4 mat);
            relpos *= new mat3(mat);
            campos = target.position + relpos * zoom;
        } else {
            campos = target.position + target.backward * zoom;
            lookpoint = target.position + target.forward * 30f;
        }

        float t = Application.deltaTime * 4f; //0.01f;
        transform.position = math.lerp(in transform.position, in campos, t);

        transform.lookat(in lookpoint, target.up);


        // zooming
        zoom += Mouse.wheeldelta;

    }
}