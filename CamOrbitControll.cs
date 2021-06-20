using Nums;
using Engine;

class CamOrbitControll : Component {
    public Transform target;
    vec3 targetPoint => target?.position ?? vec3.zero;

    public float minZoom = 2;
    public float maxZoom = 200;
    public float zoom;

    vec2 camangle;

    protected override void onEnter() {
        target = null;
    }

    protected override void onUpdate() {

        if (Mouse.isDown(MouseButton.right)) {
            //Mouse.state = MouseState.disabled;
            var d = Mouse.delta.yx * Application.deltaTime;
            d.y = -d.y;
            camangle += d; 
        } else {
            Mouse.state = MouseState.free;
        }

        zoom = math.clamp(zoom - Mouse.wheeldelta, minZoom, maxZoom);

        const float a = math.half_pi - 0.001f;
        camangle.x = math.clamp(camangle.x, -a, a);
        var cos = math.cos(camangle);
        var sin = math.sin(camangle);
        vec3 relpos = (sin.y * cos.x, sin.x, cos.y * cos.x);

        transform.position = targetPoint + relpos * zoom;
        transform.lookat(targetPoint, vec3.unity);
    }
}