#version 330 core

#include "Engine.data.shaders.noise.glsl"

uniform vec3 center;

void main() {
    gl_Position = vec4(hash(vec3(gl_VertexID)), 1.0);
}