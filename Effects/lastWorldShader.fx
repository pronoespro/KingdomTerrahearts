sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;

float4 ArmorLastWorldShader(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float2 noiseCoords = (coords * uImageSize0 - uSourceRect.xy) / uImageSize1;
    float4 noise = tex2D(uImage1, noiseCoords);
    float luminosity = (color.r + color.g + color.b) / 3;
	color.rg=(color.r+color.g)/2;
    color.b = 2 * luminosity*color.b;
	color.a=color.a*noise.a;
    return color * sampleColor * color.a;
}

technique Technique1
{
    pass ArmorMyShader
    {
        PixelShader = compile ps_2_0 ArmorLastWorldShader();
    }
}