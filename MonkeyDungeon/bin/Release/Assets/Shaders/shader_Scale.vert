#version 450 core

layout (location = 0) in vec2 aPosition;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec4 aColor;

uniform float scale;
uniform vec4 reColor;

out vec2 texCoord;
out vec4 color;

vec2 pos;

uniform mat4 transform;

void main()
{
	texCoord = aTexCoord;
	color = aColor + reColor;

	pos = vec2(aPosition.x * scale, aPosition.y);
	gl_Position = vec4(pos, 0.0f, 1.0f) * scale * transform;
}