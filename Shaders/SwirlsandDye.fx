sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float2 uTargetPosition;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;
float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;

float PI = 3.1415926535;
float4 hsv_to_rgb(float h, float s, float v, float a)
{
    float c = v * s;
    h = fmod((h * 6.0), 6.0);
    float x = c * (1.0 - abs(fmod(h, 2.0) - 1.0));
    float4 color = float4(0., 0., 0., 0.);
    color.rgb += v - c;
    color = lerp(color, float4(0.4, 0.1, 0., 1.), 0.5);
    return color;
}
float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 col2 = tex2D(uImage0, coords);
    col2.rgb *= float3(0.55, 0.35, 0.25);
    float2 uv = (coords * uImageSize0 - uSourceRect.xy) / uSourceRect.zw;
    uv = uv * 2 - 1;
    float r = length(float2(uv.x, uv.y));
    float angle = atan2(uv.x, uv.y) - r / 200.0 + 1.0 * uTime;
    float intensity = 0.5 + 0.25 * sin(15.0 * angle); 
    float4 col = hsv_to_rgb(angle / PI, intensity, 1.0, 0.5);
    float alpha = (col2.r + col2.g + col2.b) / 3;
    float4 color = col * sampleColor * alpha;
    return col2 * sampleColor + color;
}
technique Technique1
{
    pass SwirlsandDyeShaderPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}