sampler uImage0 : register(s0); // Texture
sampler uImage1 : register(s1); // Noise
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
float2 uTargetPosition;
float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;

float4 PixelShaderFunction(float4 color : COLOR0, float2 uv : TEXCOORD0) : COLOR0
{
	/*
    float const0 = 1 / uSourceRect.w;
	float const1 = uTime * 2.5;
	float const3 = uTime * -0.2;
	float2 const2;
	const2.x = 1 / uImageSize1.x;
	const2.y = 1 / uImageSize1.y;
	float4 const8 = float4(10, 0.15915494, 0.5, 0.013333334);
	float4 const9 = float4(6.2831855, -3.1415927, 0.06666667, 0.8);
	float4 const10 = float4(0.04, 0.18333334, 0.3, -0.3);
	float4 const11 = float4(2.5, -2, 3, 0.13333334);
	float3 const12 = float4(0.85, 0.15, 0.7, 0);
	*/

	const float Pi = 3.1415927;
	const float TwoPi = 6.2831855;
	const float Radian = 1.0 / TwoPi; // 0.15915494

	// float4 tmp0 = tex2D(uImage0, uv);
	float4 sampleColor = tex2D(uImage0, uv);

	/*
	float4 tmp1;
	tmp1.xy = uImageSize0;
	tmp1.z = (uv.y * tmp1.y) - uSourceRect.y;
	tmp1.z = tmp1.z * const0;
	tmp1.w = const8.x;
	tmp1.z = (tmp1.z * tmp1.w) + const1;
	tmp1.z = (tmp1.z * const8.y) + const8.z;
	tmp1.z = frac(tmp1.z);
	tmp1.z = (tmp1.z * const9.x) + const9.y;
	float4 tmp2;
	tmp2.y = sin(tmp1.z);
	tmp1.z = (tmp2.y * const8.z) + const8.z;
	*/
	float shape; // Used just for red and green channels
	shape = (uv.y * uImageSize0.y - uSourceRect.y) / uSourceRect.w;
	shape = shape * 10 + uTime * 2.5; // 1x const8 [10, 0.15915494, 0.5, 0.013333334]
	shape = shape * Radian + 0.5; // 2x const8 [10, 0.15915494, 0.5, 0.013333334]
	shape = frac(shape);
	shape = shape * TwoPi - Pi; // 1x const9 [6.2831855, -3.1415927, 0.06666667, 0.8]
	shape = sin(shape) * 0.5 + 0.5; // 2x const8 [10, 0.15915494, 0.5, 0.013333334]
	
	/*
	tmp1.w = tmp0.y + tmp0.x;
	tmp1.w = tmp0.z + tmp1.w;
	tmp0.x = tmp1.w * const8.w;
	tmp0.x = (tmp1.z * const10.x) + tmp0.x;
	tmp0.yz = (uv * tmp1.xy) - uSourceRect.xy;
	tmp0.yz = tmp0.yz + tmp0.yz;
	*/
	float totalColor = sampleColor.b + sampleColor.g + sampleColor.r; // tmp1.w
	sampleColor.r = totalColor * 0.013333334; // 1x const8 [10, 0.15915494, 0.5, 0.013333334]
	sampleColor.r = shape * 0.04 + sampleColor.r; // 1x const10 [0.04, 0.18333334, 0.3, -0.3]
	sampleColor.gb = (uv * uImageSize0 - uSourceRect.xy) * 2;

	/*
	tmp0.x = (tmp0.y * const2.x) + tmp0.x;
	tmp1.y = const2.y;
	tmp0.y = (tmp0.z * tmp1.y) + const3;
	*/
	float noiseX = sampleColor.g / uImageSize1.x + sampleColor.r;
	float noiseY = sampleColor.b / uImageSize1.y - uTime * 0.2;

	/*
	tmp2 = tex2D(uImage1, tmp0.xy);
	tmp0.x = tmp1.w * const10.y;
	tmp0.x = (tmp2.x * const10.z) + tmp0.x;
	tmp0.y = (tmp1.w * const11.w) + tmp2.x;
	tmp0.z = (tmp1.w * const9.z) + const9.w;
	*/
	float noiseChannel = tex2D(uImage1, float2(noiseX, noiseY)).r; // tmp2.x
	sampleColor.r = totalColor * 0.18333334; // 1x const10 [0.04, 0.18333334, 0.3, -0.3]
	sampleColor.r = noiseChannel * 0.3 + sampleColor.r; // 1x const10 [0.04, 0.18333334, 0.3, -0.3]
	sampleColor.g = totalColor * 3 + noiseChannel; // 1x const11 [2.5, -2, 3, 0.13333334]
	sampleColor.b = totalColor * 0.06666667 + 0.8; // 2x const9 [6.2831855, -3.1415927, 0.06666667, 0.8]

	/*
	tmp1.xyz = tmp0.zzz * uSecondaryColor;
	tmp1.xyz = tmp1.xyz * color.xyz;
	*/
	float4 tmp1; // Eh....
	tmp1.xyz = sampleColor.bbb * uSecondaryColor;
	tmp1.xyz = tmp1.xyz * color.rgb;

	/*
	tmp2.xyz = color.xyz * const12.xxx;
	tmp2.xyz = (color.www * const12.yyy) + tmp2.xyz;
	tmp2.xyz = (uColor * tmp2.xyz) - tmp1.xyz;
	*/
	float4 noiseTexture;
	noiseTexture.rgb = color.rgb * 0.85; // 1x const12 [0.85, 0.15, 0.7, 0]
	noiseTexture.rgb = (color.aaa * 0.15) + noiseTexture.rgb; // 1x const12 [0.85, 0.15, 0.7, 0]
	noiseTexture.rgb = (uColor * noiseTexture.rgb) - tmp1.xyz;

	// tmp0.xyz = (tmp0.xxx * tmp2.xyz) + tmp1.xyz;
	float3 lerpAmount = sampleColor.rrr * noiseTexture + tmp1.xyz;

	/*
	tmp1.w = tmp0.y - const9.w;
	tmp1.w = tmp1.w * const8.x;
	tmp0.x = tmp0.x + const10.w;
	tmp0.x = tmp0.x * const11.x;
	tmp0.y = (tmp0.x * const11.y) + const11.z;
	tmp0.x = tmp0.x * tmp0.x;
	*/
	tmp1.w = lerpAmount.y - 0.8; // 1x const9 [6.2831855, -3.1415927, 0.06666667, 0.8]
	tmp1.w = tmp1.w * 10; // 1x const8 [10, 0.15915494, 0.5, 0.013333334]
	lerpAmount.x -= 0.3; // 1x const10 [0.04, 0.18333334, 0.3, -0.3]
	lerpAmount.x *= 2.5; // 1x const11 [2.5, -2, 3, 0.13333334]
	lerpAmount.y = lerpAmount.x * -2 + 3;  // 2x const11 [2.5, -2, 3, 0.13333334]
	lerpAmount.x = lerpAmount.y * lerpAmount.y;

	/*
	tmp0.xyz = (tmp0.xxx * tmp2.xyz) + tmp1.xyz;
	tmp1.x = (tmp1.w * const11.y) + const11.z;
	tmp1.y = tmp1.w * tmp1.w;
	tmp1.x = tmp1.x * tmp1.y;
	*/
	lerpAmount = lerpAmount * noiseTexture.rgb + tmp1.rgb;
	tmp1.x = tmp1.w * -2 + 3; // 2x const11 [2.5, -2, 3, 0.13333334]
	tmp1.y = tmp1.w * tmp1.w;
	tmp1.x *= tmp1.y;

	/*
	tmp1.yzw = color.zyx * const12.zzz;
	tmp1.yzw = (color.www * const10.zzz) + tmp1.yzw;
	tmp2.xyz = lerp(tmp1.xxx, tmp1.wzy, tmp0.xyz);
	tmp2.w = color.w;
	*/
	tmp1.yzw = color.bgr * 0.7; // 1x const12 [0.85, 0.15, 0.7, 0]
	tmp1.yzw = (color.aaa * 0.3) + tmp1.yzw; // 1x const10 [0.04, 0.18333334, 0.3, -0.3]
	noiseTexture.rgb = lerp(tmp1.xxx, tmp1.wzy, lerpAmount);
	noiseTexture.a = color.a;

	/*
	tmp0 = tmp0.w * tmp2;
	return tmp0;
	*/
	return noiseTexture * sampleColor.a;
}

technique Technique1
{
    pass ShiftingCreamsandDyeShaderPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}