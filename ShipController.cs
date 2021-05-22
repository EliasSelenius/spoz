using Nums;
using Engine;

class ShipController : Component {

    Rigidbody rb;

    float inputScaler;

    public float yaw, pitch, roll;

    float shipTorque = 0.01f;
    float thrustForce = 100f;
    float maxSpeed = 60;

    public void input(float dir) {
        inputScaler = dir;
    }

    public void rotate(float yaw, float pitch, float roll) {
        // note: angle must be in [-1 .. 1] range
        (this.yaw, this.pitch, this.roll) = (yaw, pitch, roll);

        /*var absyaw = math.abs(yaw);
        if (absyaw > 1f) yawPitchRoll.x /= absyaw;

        var abspitch = math.abs(pitch);
        if (abspitch > 1f) yawPitchRoll.y /= abspitch;*/
    }


    protected override void onStart() {
        rb = gameobject.requireComponent<Rigidbody>();
    }

    protected override void onUpdate() {
        var force = thrustForce * Application.deltaTime;
        var torque = shipTorque * Application.deltaTime;

        // thrust input
        rb.addForce(transform.forward * inputScaler * force);


        // yaw
        rb.addTorque(transform.up, yaw * torque);
        // pitch
        rb.addTorque(transform.left, pitch * torque);
        // roll
        rb.addTorque(transform.forward, roll * torque);


        // clamp to maxspeed
        if (rb.velocity.sqlength > maxSpeed * maxSpeed) {
            rb.velocity.normalize();
            rb.velocity *= maxSpeed;
        }

        // dampning
        rb.velocity -= rb.velocity * Application.deltaTime;

        // TODO: some other angular dampning method
        quat.toAxisangle(in rb.angularVelocity, out vec3 axis, out float angle);
        if (angle > 0) {
        }

            angle *= 0.999f;
            rb.angularVelocity = quat.fromAxisangle(in axis, angle);
    }

}