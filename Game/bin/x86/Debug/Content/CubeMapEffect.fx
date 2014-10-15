float4x4 World 					: WORLD;
float4x4 View					: VIEW;
float4x4 Projection				: PROJECTION;	

float4 MaterialDiffuseColor		= { 0.8f, 0.8f, 0.8f, 1.0f };

float4 AmbientLight 			= { 0.5f, 0.5f, 0.5f, 1.0f };
float4 LightDiffuseColor		= { 0.5f, 0.5f, 0.5f, 1.0f };
float4 LightSpecularColor		= { 0.9f, 0.9f, 0.9f, 1.0f };
float SpecularPower	= 20.0f;

float4 CameraPosition 			= { 0.0f, 0.0f, -1.0f, 1.0f };
float4 DiffuseLightPosition		= { 0.0f, 150.0f, -50.0f, 0.0f };

texture2D ColorMap;
sampler ColorMapSampler			= sampler_state
{
	Texture = <ColorMap>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;   
	AddressU  = Clamp;
	AddressV  = Clamp;
};

textureCUBE CubeMap				: ENVIRONMENT; 
samplerCUBE CubeMapSampler		= sampler_state
{
	Texture = <CubeMap>;
   	MinFilter = Linear;
   	MagFilter = Linear;
   	MipFilter = Linear;   
   	AddressU  = Clamp;
   	AddressV  = Clamp;
};

struct VS_INPUT
{
	float4 position 		: POSITION;
	float3 normal 			: NORMAL;
	float2 textureUV		: TEXCOORD;
};

struct VS_OUTPUT
{
	float4 position 		: POSITION;
	float3 normal 			: TEXCOORD0;
	float3 viewDirection	: TEXCOORD1;
	float3 lightDirection	: TEXCOORD2;
	float2 textureUV		: TEXCOORD3;
	float3 reflection		: TEXCOORD4;
};

struct PS_Input
{
	float3 normal			: TEXCOORD0;
	float3 viewDirection	: TEXCOORD1;
	float3 lightDirection	: TEXCOORD2;
	float2 textureUV		: TEXCOORD3;
	float3 reflection		: TEXCOORD4;
};

VS_OUTPUT VS(VS_INPUT vs_Input)
{
	VS_OUTPUT vs_Output;

	float4x4 WorldViewProjection = mul(mul(World, View),  Projection);
	vs_Output.position = mul(vs_Input.position, WorldViewProjection);
	vs_Output.normal = mul(vs_Input.normal, World);
	float4 pos = mul(vs_Input.position, World);
	vs_Output.viewDirection = CameraPosition - pos;
	vs_Output.lightDirection = DiffuseLightPosition - pos;
	vs_Output.textureUV = vs_Input.textureUV;
	
	float3 incident = normalize(pos - CameraPosition);
	vs_Output.reflection = incident - 2 * vs_Output.normal * dot(incident, vs_Output.normal);
	
	return vs_Output;	
}

float4 PS(PS_Input ps_Input) : COLOR
{
	float4 color;

	float3 normal = normalize(ps_Input.normal);
	float3 cameraDirection = normalize(ps_Input.viewDirection);
	float3 lightDirection = normalize(ps_Input.lightDirection);

	float4 ambient = saturate(AmbientLight * dot(normal, cameraDirection));
	float NdotL = dot(normal, lightDirection);
	float4 diffuse =   saturate((MaterialDiffuseColor + LightDiffuseColor) * 0.5f * NdotL);

	float3 reflection = normalize(2.0f * NdotL * normal - lightDirection);
	float RdotV = max(0.0f, dot(reflection, cameraDirection));
	float4 specular = saturate(LightSpecularColor * pow(RdotV, SpecularPower));

	float4 colorMap = tex2D(ColorMapSampler, ps_Input.textureUV);
	float4 cubeMap = texCUBE(CubeMapSampler, ps_Input.reflection);

	color = ambient * 0.2 + diffuse * 0.1 + specular * 0.5 + colorMap * 0.3 + cubeMap * 0.2;
	return  color;
}

technique CarShader
{
	pass P0
	{
		VertexShader = compile vs_1_1 VS();
		PixelShader = compile ps_2_0 PS();
	}
}