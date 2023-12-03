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

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 PlayerCoords = (coords * uImageSize0 - uSourceRect.xy) / uImageSize1;
    float4 BaseColor = tex2D(uImage0, coords);
    float4 FirstCandy = tex2D(uImage1, PlayerCoords + float2(0, uTime * -0.05));
    float4 SecondCandy = tex2D(uImage1, PlayerCoords + float2(50, uTime * -0.1));
    float4 ThirdCandy = tex2D(uImage1, PlayerCoords + float2(-50, uTime * -0.025));
    float PixelAlpha = (BaseColor.r + BaseColor.g + BaseColor.b) / 3;
    return BaseColor * sampleColor + (FirstCandy * sampleColor * PixelAlpha) + (SecondCandy * sampleColor * PixelAlpha) + (ThirdCandy * sampleColor * PixelAlpha);
}

technique Technique1
{
    pass CandyCornDyeShaderPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}