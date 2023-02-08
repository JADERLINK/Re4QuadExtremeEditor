#version 330

out vec4 outputColor;

in vec2 texCoord;

uniform sampler2D texture0;

void main()
{
vec4 texColor = mix(texture(texture0, texCoord), vec4(0.0,0.0,0.0,1.0), 0.05);
    if(texColor.a < 0.1)
        discard;
    outputColor = texColor;
}