using OpenTK.Graphics.OpenGL4;
using Engine;


/*

    point sprites
    transform feedback

*/


class ParticleRenderer {

    static ParticleRenderer r;
    public static void init() {
        r = new() {
            shader = Assets.getShader("particle")
        };

    }

    Shader shader;

    public void render() {
        shader.use();
        GL.DrawArrays(PrimitiveType.Points, 0, 1000);
    }


    public static void renderTest() {
        r.render();
    }
}