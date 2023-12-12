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

const float a = 1.0;
const float b = 0.1759;
const float Pi = 3.14159265359;
const float Tau = 6.28318530718;

float spiralSDF(float2 p) {
    float t = atan2(p.y, p.x) + uTime * 8.0;
    float r = length(p);
    
    float n = (log(r / a) / b - t) / 2.0 / Pi;

    float upper_r = a * exp(b * (t + Tau * ceil(n)));
    float lower_r = a * exp(b * (t + Tau * floor(n)));
    
    float val = min(abs(upper_r - r), abs(r - lower_r));
    
    return val;
}

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 playerCoords = coords * uImageSize0 - uSourceRect.xy;
    float d = spiralSDF(playerCoords);
    
    float4 color = tex2D(uImage0, coords);
    color.rgb = saturate(color.rgb * pow(d, 1.52) * 0.28);
    color.rgb *= uColor;
    
    color *= sampleColor;
    
    return color;
}

technique Technique1
{
    pass SwirlsandDyeShaderPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}