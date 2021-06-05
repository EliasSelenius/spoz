using OpenTK.Graphics.OpenGL4;
using Engine;
using Nums;
using System.Linq;

/*

    point sprites
    transform feedback


    general gist
    API
    first iteration
    refactor

*/


class ParticleBatch {

    Shader shader;
    int vao, vbo;

    particle[] particles = new particle[1000];

    public ParticleBatch() {
        vbo = GLUtils.createBuffer();
        vao = GLUtils.createVertexArray<particle>(vbo);
    }

    public void render() {
        shader.use();

        var d = particles.Select(x => x.pos).ToArray();
        GLUtils.bufferdata(vbo, d);

        GL.BindVertexArray(vao);
        GL.DrawArrays(PrimitiveType.Points, 0, particles.Length);
    }

}

[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
struct particle {
    public vec3 pos, vel;

    public void step(float dt) {
        pos += vel * dt;
    }
}