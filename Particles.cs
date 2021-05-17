using OpenTK.Graphics.OpenGL4;
using Engine;


/*

    point sprites
    transform feedback

*/


class ParticleRenderer {

    Shader shader;

    public void render() {
        shader.use();
        GL.DrawArrays(PrimitiveType.Points, 0, 1000);
    }
}