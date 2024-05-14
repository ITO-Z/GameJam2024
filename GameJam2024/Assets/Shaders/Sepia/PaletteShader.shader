Shader "PaletteShader"
{
	Properties
	{
		[NoScaleOffset] _MainTex("MainTex", 2D) = "white" {}
		[NoScaleOffset]_GradientTex("GradientTex", 2D) = "white" {}
		_GradientIntens("GradientIntens", Float) = 1
		[HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
		[HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
		[HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}

		// required for UI.Mask
		 _StencilComp("Stencil Comparison", Float) = 8
		 _Stencil("Stencil ID", Float) = 0
		 _StencilOp("Stencil Operation", Float) = 0
		 _StencilWriteMask("Stencil Write Mask", Float) = 255
		 _StencilReadMask("Stencil Read Mask", Float) = 255
		 _ColorMask("Color Mask", Float) = 15
	}
		SubShader
		 {
			 Tags
			 {
				 "RenderPipeline" = "UniversalPipeline"
				 "RenderType" = "Transparent"
				 "UniversalMaterialType" = "Unlit"
				 "Queue" = "Transparent"
				 "ShaderGraphShader" = "true"
				 "ShaderGraphTargetId" = ""
			 }
			 // required for UI.Mask
 Stencil
 {
	 Ref[_Stencil]
	 Comp[_StencilComp]
	 Pass[_StencilOp]
	 ReadMask[_StencilReadMask]
	 WriteMask[_StencilWriteMask]
 }
 ColorMask[_ColorMask]
			 Pass
			 {
				 Name "Sprite Unlit"
				 Tags
				 {
					 "LightMode" = "Universal2D"
				 }

			 // Render State
			 Cull Off
		 Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
		 ZTest LEqual
		 ZWrite Off

			 // Debug
			 // <None>

			 // --------------------------------------------------
			 // Pass

			 HLSLPROGRAM

			 // Pragmas
			 #pragma target 2.0
		 #pragma exclude_renderers d3d11_9x
		 #pragma vertex vert
		 #pragma fragment frag

			 // DotsInstancingOptions: <None>
			 // HybridV1InjectedBuiltinProperties: <None>

			 // Keywords
			 #pragma multi_compile_fragment _ DEBUG_DISPLAY
			 // GraphKeywords: <None>

			 // Defines
			 #define _SURFACE_TYPE_TRANSPARENT 1
			 #define ATTRIBUTES_NEED_NORMAL
			 #define ATTRIBUTES_NEED_TANGENT
			 #define ATTRIBUTES_NEED_TEXCOORD0
			 #define ATTRIBUTES_NEED_COLOR
			 #define VARYINGS_NEED_POSITION_WS
			 #define VARYINGS_NEED_TEXCOORD0
			 #define VARYINGS_NEED_COLOR
			 #define FEATURES_GRAPH_VERTEX
			 /* WARNING: $splice Could not find named fragment 'PassInstancing' */
			 #define SHADERPASS SHADERPASS_SPRITEUNLIT
			 /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

			 // Includes
			 /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */

			 #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
		 #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
		 #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		 #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
		 #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
		 #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
		 #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			 // --------------------------------------------------
			 // Structs and Packing

			 /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

			 struct Attributes
		 {
			  float3 positionOS : POSITION;
			  float3 normalOS : NORMAL;
			  float4 tangentOS : TANGENT;
			  float4 uv0 : TEXCOORD0;
			  float4 color : COLOR;
			 #if UNITY_ANY_INSTANCING_ENABLED
			  uint instanceID : INSTANCEID_SEMANTIC;
			 #endif
		 };
		 struct Varyings
		 {
			  float4 positionCS : SV_POSITION;
			  float3 positionWS;
			  float4 texCoord0;
			  float4 color;
			 #if UNITY_ANY_INSTANCING_ENABLED
			  uint instanceID : CUSTOM_INSTANCE_ID;
			 #endif
			 #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
			  uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
			 #endif
			 #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
			  uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
			 #endif
			 #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
			  FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
			 #endif
		 };
		 struct SurfaceDescriptionInputs
		 {
			  float4 uv0;
		 };
		 struct VertexDescriptionInputs
		 {
			  float3 ObjectSpaceNormal;
			  float3 ObjectSpaceTangent;
			  float3 ObjectSpacePosition;
		 };
		 struct PackedVaryings
		 {
			  float4 positionCS : SV_POSITION;
			  float4 texCoord0 : INTERP0;
			  float4 color : INTERP1;
			  float3 positionWS : INTERP2;
			 #if UNITY_ANY_INSTANCING_ENABLED
			  uint instanceID : CUSTOM_INSTANCE_ID;
			 #endif
			 #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
			  uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
			 #endif
			 #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
			  uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
			 #endif
			 #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
			  FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
			 #endif
		 };

			 PackedVaryings PackVaryings(Varyings input)
		 {
			 PackedVaryings output;
			 ZERO_INITIALIZE(PackedVaryings, output);
			 output.positionCS = input.positionCS;
			 output.texCoord0.xyzw = input.texCoord0;
			 output.color.xyzw = input.color;
			 output.positionWS.xyz = input.positionWS;
			 #if UNITY_ANY_INSTANCING_ENABLED
			 output.instanceID = input.instanceID;
			 #endif
			 #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
			 output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
			 #endif
			 #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
			 output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
			 #endif
			 #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
			 output.cullFace = input.cullFace;
			 #endif
			 return output;
		 }

		 Varyings UnpackVaryings(PackedVaryings input)
		 {
			 Varyings output;
			 output.positionCS = input.positionCS;
			 output.texCoord0 = input.texCoord0.xyzw;
			 output.color = input.color.xyzw;
			 output.positionWS = input.positionWS.xyz;
			 #if UNITY_ANY_INSTANCING_ENABLED
			 output.instanceID = input.instanceID;
			 #endif
			 #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
			 output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
			 #endif
			 #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
			 output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
			 #endif
			 #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
			 output.cullFace = input.cullFace;
			 #endif
			 return output;
		 }


		 // --------------------------------------------------
		 // Graph

		 // Graph Properties
		 CBUFFER_START(UnityPerMaterial)
	 float4 _MainTex_TexelSize;
	 float4 _GradientTex_TexelSize;
	 float _GradientIntens;
	 CBUFFER_END

		 // Object and Global properties
		 SAMPLER(SamplerState_Linear_Repeat);
		 TEXTURE2D(_MainTex);
		 SAMPLER(sampler_MainTex);
		 TEXTURE2D(_GradientTex);
		 SAMPLER(sampler_GradientTex);

		 // Graph Includes
		 // GraphIncludes: <None>

		 // -- Property used by ScenePickingPass
		 #ifdef SCENEPICKINGPASS
		 float4 _SelectionID;
		 #endif

		 // -- Properties used by SceneSelectionPass
		 #ifdef SCENESELECTIONPASS
		 int _ObjectId;
		 int _PassValue;
		 #endif

		 // Graph Functions

	 void Unity_Multiply_float_float(float A, float B, out float Out)
	 {
		 Out = A * B;
	 }

	 void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
	 {
		 RGBA = float4(R, G, B, A);
		 RGB = float3(R, G, B);
		 RG = float2(R, G);
	 }

	 /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

	 // Graph Vertex
	 struct VertexDescription
 {
	 float3 Position;
	 float3 Normal;
	 float3 Tangent;
 };

 VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
 {
	 VertexDescription description = (VertexDescription)0;
	 description.Position = IN.ObjectSpacePosition;
	 description.Normal = IN.ObjectSpaceNormal;
	 description.Tangent = IN.ObjectSpaceTangent;
	 return description;
 }

	 #ifdef FEATURES_GRAPH_VERTEX
 Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
 {
 return output;
 }
 #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
 #endif

 // Graph Pixel
 struct SurfaceDescription
{
	float3 BaseColor;
	float Alpha;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
	SurfaceDescription surface = (SurfaceDescription)0;
	UnityTexture2D _Property_07faefce673f4410a2597a3de08a0cb3_Out_0 = UnityBuildTexture2DStructNoScale(_GradientTex);
	float _Property_9934d4862771493790a6bb54ef534744_Out_0 = _GradientIntens;
	UnityTexture2D _Property_f9b3322f712549a5bb753617ec724c7e_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
	float4 _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_f9b3322f712549a5bb753617ec724c7e_Out_0.tex, _Property_f9b3322f712549a5bb753617ec724c7e_Out_0.samplerstate, _Property_f9b3322f712549a5bb753617ec724c7e_Out_0.GetTransformedUV(IN.uv0.xy));
	float _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_R_4 = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0.r;
	float _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_G_5 = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0.g;
	float _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_B_6 = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0.b;
	float _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_A_7 = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0.a;
	float _Float_fc3e0b95bca54f3e91a91e967139b334_Out_0 = (_SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0).x;
	float _Multiply_fa70f1ca055241a1995c748de1e93df0_Out_2;
	Unity_Multiply_float_float(_Property_9934d4862771493790a6bb54ef534744_Out_0, _Float_fc3e0b95bca54f3e91a91e967139b334_Out_0, _Multiply_fa70f1ca055241a1995c748de1e93df0_Out_2);
	float2 _Vector2_b26b08054493422e9b0e8bfd4b57bc4a_Out_0 = float2(_Multiply_fa70f1ca055241a1995c748de1e93df0_Out_2, 0);
	float4 _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_RGBA_0 = SAMPLE_TEXTURE2D(_Property_07faefce673f4410a2597a3de08a0cb3_Out_0.tex, _Property_07faefce673f4410a2597a3de08a0cb3_Out_0.samplerstate, _Property_07faefce673f4410a2597a3de08a0cb3_Out_0.GetTransformedUV(_Vector2_b26b08054493422e9b0e8bfd4b57bc4a_Out_0));
	float _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_R_4 = _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_RGBA_0.r;
	float _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_G_5 = _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_RGBA_0.g;
	float _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_B_6 = _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_RGBA_0.b;
	float _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_A_7 = _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_RGBA_0.a;
	float4 _Combine_6c5ffcf8ece344148761471216d414a2_RGBA_4;
	float3 _Combine_6c5ffcf8ece344148761471216d414a2_RGB_5;
	float2 _Combine_6c5ffcf8ece344148761471216d414a2_RG_6;
	Unity_Combine_float(_SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_R_4, _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_G_5, _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_B_6, 0, _Combine_6c5ffcf8ece344148761471216d414a2_RGBA_4, _Combine_6c5ffcf8ece344148761471216d414a2_RGB_5, _Combine_6c5ffcf8ece344148761471216d414a2_RG_6);
	surface.BaseColor = _Combine_6c5ffcf8ece344148761471216d414a2_RGB_5;
	surface.Alpha = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_A_7;
	return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
	VertexDescriptionInputs output;
	ZERO_INITIALIZE(VertexDescriptionInputs, output);

	output.ObjectSpaceNormal = input.normalOS;
	output.ObjectSpaceTangent = input.tangentOS.xyz;
	output.ObjectSpacePosition = input.positionOS;

	return output;
}
	SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
	SurfaceDescriptionInputs output;
	ZERO_INITIALIZE(SurfaceDescriptionInputs, output);







	output.uv0 = input.texCoord0;
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

	return output;
}

	// --------------------------------------------------
	// Main

	#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"

	ENDHLSL
}
Pass
{
	Name "SceneSelectionPass"
	Tags
	{
		"LightMode" = "SceneSelectionPass"
	}

		// Render State
		Cull Off

		// Debug
		// <None>

		// --------------------------------------------------
		// Pass

		HLSLPROGRAM

		// Pragmas
		#pragma target 2.0
	#pragma exclude_renderers d3d11_9x
	#pragma vertex vert
	#pragma fragment frag

		// DotsInstancingOptions: <None>
		// HybridV1InjectedBuiltinProperties: <None>

		// Keywords
		// PassKeywords: <None>
		// GraphKeywords: <None>

		// Defines
		#define _SURFACE_TYPE_TRANSPARENT 1
		#define ATTRIBUTES_NEED_NORMAL
		#define ATTRIBUTES_NEED_TANGENT
		#define ATTRIBUTES_NEED_TEXCOORD0
		#define VARYINGS_NEED_TEXCOORD0
		#define FEATURES_GRAPH_VERTEX
		/* WARNING: $splice Could not find named fragment 'PassInstancing' */
		#define SHADERPASS SHADERPASS_DEPTHONLY
	#define SCENESELECTIONPASS 1

		/* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

		// Includes
		/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

		// --------------------------------------------------
		// Structs and Packing

		/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

		struct Attributes
	{
		 float3 positionOS : POSITION;
		 float3 normalOS : NORMAL;
		 float4 tangentOS : TANGENT;
		 float4 uv0 : TEXCOORD0;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : INSTANCEID_SEMANTIC;
		#endif
	};
	struct Varyings
	{
		 float4 positionCS : SV_POSITION;
		 float4 texCoord0;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : CUSTOM_INSTANCE_ID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		 uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		 uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
		#endif
	};
	struct SurfaceDescriptionInputs
	{
		 float4 uv0;
	};
	struct VertexDescriptionInputs
	{
		 float3 ObjectSpaceNormal;
		 float3 ObjectSpaceTangent;
		 float3 ObjectSpacePosition;
	};
	struct PackedVaryings
	{
		 float4 positionCS : SV_POSITION;
		 float4 texCoord0 : INTERP0;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : CUSTOM_INSTANCE_ID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		 uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		 uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
		#endif
	};

		PackedVaryings PackVaryings(Varyings input)
	{
		PackedVaryings output;
		ZERO_INITIALIZE(PackedVaryings, output);
		output.positionCS = input.positionCS;
		output.texCoord0.xyzw = input.texCoord0;
		#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
		#endif
		return output;
	}

	Varyings UnpackVaryings(PackedVaryings input)
	{
		Varyings output;
		output.positionCS = input.positionCS;
		output.texCoord0 = input.texCoord0.xyzw;
		#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
		#endif
		return output;
	}


	// --------------------------------------------------
	// Graph

	// Graph Properties
	CBUFFER_START(UnityPerMaterial)
float4 _MainTex_TexelSize;
float4 _GradientTex_TexelSize;
float _GradientIntens;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
TEXTURE2D(_GradientTex);
SAMPLER(sampler_GradientTex);

// Graph Includes
// GraphIncludes: <None>

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Graph Functions
// GraphFunctions: <None>

/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

// Graph Vertex
struct VertexDescription
{
	float3 Position;
	float3 Normal;
	float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
	VertexDescription description = (VertexDescription)0;
	description.Position = IN.ObjectSpacePosition;
	description.Normal = IN.ObjectSpaceNormal;
	description.Tangent = IN.ObjectSpaceTangent;
	return description;
}

	#ifdef FEATURES_GRAPH_VERTEX
Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
{
return output;
}
#define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
#endif

// Graph Pixel
struct SurfaceDescription
{
	float Alpha;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
	SurfaceDescription surface = (SurfaceDescription)0;
	UnityTexture2D _Property_f9b3322f712549a5bb753617ec724c7e_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
	float4 _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_f9b3322f712549a5bb753617ec724c7e_Out_0.tex, _Property_f9b3322f712549a5bb753617ec724c7e_Out_0.samplerstate, _Property_f9b3322f712549a5bb753617ec724c7e_Out_0.GetTransformedUV(IN.uv0.xy));
	float _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_R_4 = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0.r;
	float _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_G_5 = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0.g;
	float _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_B_6 = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0.b;
	float _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_A_7 = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0.a;
	surface.Alpha = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_A_7;
	return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
	VertexDescriptionInputs output;
	ZERO_INITIALIZE(VertexDescriptionInputs, output);

	output.ObjectSpaceNormal = input.normalOS;
	output.ObjectSpaceTangent = input.tangentOS.xyz;
	output.ObjectSpacePosition = input.positionOS;

	return output;
}
	SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
	SurfaceDescriptionInputs output;
	ZERO_INITIALIZE(SurfaceDescriptionInputs, output);







	output.uv0 = input.texCoord0;
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

	return output;
}

	// --------------------------------------------------
	// Main

	#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"

	ENDHLSL
}
Pass
{
	Name "ScenePickingPass"
	Tags
	{
		"LightMode" = "Picking"
	}

		// Render State
		Cull Back

		// Debug
		// <None>

		// --------------------------------------------------
		// Pass

		HLSLPROGRAM

		// Pragmas
		#pragma target 2.0
	#pragma exclude_renderers d3d11_9x
	#pragma vertex vert
	#pragma fragment frag

		// DotsInstancingOptions: <None>
		// HybridV1InjectedBuiltinProperties: <None>

		// Keywords
		// PassKeywords: <None>
		// GraphKeywords: <None>

		// Defines
		#define _SURFACE_TYPE_TRANSPARENT 1
		#define ATTRIBUTES_NEED_NORMAL
		#define ATTRIBUTES_NEED_TANGENT
		#define ATTRIBUTES_NEED_TEXCOORD0
		#define VARYINGS_NEED_TEXCOORD0
		#define FEATURES_GRAPH_VERTEX
		/* WARNING: $splice Could not find named fragment 'PassInstancing' */
		#define SHADERPASS SHADERPASS_DEPTHONLY
	#define SCENEPICKINGPASS 1

		/* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

		// Includes
		/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

		// --------------------------------------------------
		// Structs and Packing

		/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

		struct Attributes
	{
		 float3 positionOS : POSITION;
		 float3 normalOS : NORMAL;
		 float4 tangentOS : TANGENT;
		 float4 uv0 : TEXCOORD0;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : INSTANCEID_SEMANTIC;
		#endif
	};
	struct Varyings
	{
		 float4 positionCS : SV_POSITION;
		 float4 texCoord0;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : CUSTOM_INSTANCE_ID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		 uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		 uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
		#endif
	};
	struct SurfaceDescriptionInputs
	{
		 float4 uv0;
	};
	struct VertexDescriptionInputs
	{
		 float3 ObjectSpaceNormal;
		 float3 ObjectSpaceTangent;
		 float3 ObjectSpacePosition;
	};
	struct PackedVaryings
	{
		 float4 positionCS : SV_POSITION;
		 float4 texCoord0 : INTERP0;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : CUSTOM_INSTANCE_ID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		 uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		 uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
		#endif
	};

		PackedVaryings PackVaryings(Varyings input)
	{
		PackedVaryings output;
		ZERO_INITIALIZE(PackedVaryings, output);
		output.positionCS = input.positionCS;
		output.texCoord0.xyzw = input.texCoord0;
		#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
		#endif
		return output;
	}

	Varyings UnpackVaryings(PackedVaryings input)
	{
		Varyings output;
		output.positionCS = input.positionCS;
		output.texCoord0 = input.texCoord0.xyzw;
		#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
		#endif
		return output;
	}


	// --------------------------------------------------
	// Graph

	// Graph Properties
	CBUFFER_START(UnityPerMaterial)
float4 _MainTex_TexelSize;
float4 _GradientTex_TexelSize;
float _GradientIntens;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
TEXTURE2D(_GradientTex);
SAMPLER(sampler_GradientTex);

// Graph Includes
// GraphIncludes: <None>

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Graph Functions
// GraphFunctions: <None>

/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

// Graph Vertex
struct VertexDescription
{
	float3 Position;
	float3 Normal;
	float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
	VertexDescription description = (VertexDescription)0;
	description.Position = IN.ObjectSpacePosition;
	description.Normal = IN.ObjectSpaceNormal;
	description.Tangent = IN.ObjectSpaceTangent;
	return description;
}

	#ifdef FEATURES_GRAPH_VERTEX
Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
{
return output;
}
#define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
#endif

// Graph Pixel
struct SurfaceDescription
{
	float Alpha;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
	SurfaceDescription surface = (SurfaceDescription)0;
	UnityTexture2D _Property_f9b3322f712549a5bb753617ec724c7e_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
	float4 _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_f9b3322f712549a5bb753617ec724c7e_Out_0.tex, _Property_f9b3322f712549a5bb753617ec724c7e_Out_0.samplerstate, _Property_f9b3322f712549a5bb753617ec724c7e_Out_0.GetTransformedUV(IN.uv0.xy));
	float _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_R_4 = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0.r;
	float _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_G_5 = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0.g;
	float _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_B_6 = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0.b;
	float _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_A_7 = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0.a;
	surface.Alpha = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_A_7;
	return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
	VertexDescriptionInputs output;
	ZERO_INITIALIZE(VertexDescriptionInputs, output);

	output.ObjectSpaceNormal = input.normalOS;
	output.ObjectSpaceTangent = input.tangentOS.xyz;
	output.ObjectSpacePosition = input.positionOS;

	return output;
}
	SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
	SurfaceDescriptionInputs output;
	ZERO_INITIALIZE(SurfaceDescriptionInputs, output);







	output.uv0 = input.texCoord0;
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

	return output;
}

	// --------------------------------------------------
	// Main

	#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"

	ENDHLSL
}
Pass
{
	Name "Sprite Unlit"
	Tags
	{
		"LightMode" = "UniversalForward"
	}

		// Render State
		Cull Off
	Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
	ZTest LEqual
	ZWrite Off

		// Debug
		// <None>

		// --------------------------------------------------
		// Pass

		HLSLPROGRAM

		// Pragmas
		#pragma target 2.0
	#pragma exclude_renderers d3d11_9x
	#pragma vertex vert
	#pragma fragment frag

		// DotsInstancingOptions: <None>
		// HybridV1InjectedBuiltinProperties: <None>

		// Keywords
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		// GraphKeywords: <None>

		// Defines
		#define _SURFACE_TYPE_TRANSPARENT 1
		#define ATTRIBUTES_NEED_NORMAL
		#define ATTRIBUTES_NEED_TANGENT
		#define ATTRIBUTES_NEED_TEXCOORD0
		#define ATTRIBUTES_NEED_COLOR
		#define VARYINGS_NEED_POSITION_WS
		#define VARYINGS_NEED_TEXCOORD0
		#define VARYINGS_NEED_COLOR
		#define FEATURES_GRAPH_VERTEX
		/* WARNING: $splice Could not find named fragment 'PassInstancing' */
		#define SHADERPASS SHADERPASS_SPRITEFORWARD
		/* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

		// Includes
		/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

		// --------------------------------------------------
		// Structs and Packing

		/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

		struct Attributes
	{
		 float3 positionOS : POSITION;
		 float3 normalOS : NORMAL;
		 float4 tangentOS : TANGENT;
		 float4 uv0 : TEXCOORD0;
		 float4 color : COLOR;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : INSTANCEID_SEMANTIC;
		#endif
	};
	struct Varyings
	{
		 float4 positionCS : SV_POSITION;
		 float3 positionWS;
		 float4 texCoord0;
		 float4 color;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : CUSTOM_INSTANCE_ID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		 uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		 uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
		#endif
	};
	struct SurfaceDescriptionInputs
	{
		 float4 uv0;
	};
	struct VertexDescriptionInputs
	{
		 float3 ObjectSpaceNormal;
		 float3 ObjectSpaceTangent;
		 float3 ObjectSpacePosition;
	};
	struct PackedVaryings
	{
		 float4 positionCS : SV_POSITION;
		 float4 texCoord0 : INTERP0;
		 float4 color : INTERP1;
		 float3 positionWS : INTERP2;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : CUSTOM_INSTANCE_ID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		 uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		 uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
		#endif
	};

		PackedVaryings PackVaryings(Varyings input)
	{
		PackedVaryings output;
		ZERO_INITIALIZE(PackedVaryings, output);
		output.positionCS = input.positionCS;
		output.texCoord0.xyzw = input.texCoord0;
		output.color.xyzw = input.color;
		output.positionWS.xyz = input.positionWS;
		#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
		#endif
		return output;
	}

	Varyings UnpackVaryings(PackedVaryings input)
	{
		Varyings output;
		output.positionCS = input.positionCS;
		output.texCoord0 = input.texCoord0.xyzw;
		output.color = input.color.xyzw;
		output.positionWS = input.positionWS.xyz;
		#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
		#endif
		return output;
	}


	// --------------------------------------------------
	// Graph

	// Graph Properties
	CBUFFER_START(UnityPerMaterial)
float4 _MainTex_TexelSize;
float4 _GradientTex_TexelSize;
float _GradientIntens;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
TEXTURE2D(_GradientTex);
SAMPLER(sampler_GradientTex);

// Graph Includes
// GraphIncludes: <None>

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Graph Functions

void Unity_Multiply_float_float(float A, float B, out float Out)
{
	Out = A * B;
}

void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
{
	RGBA = float4(R, G, B, A);
	RGB = float3(R, G, B);
	RG = float2(R, G);
}

/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

// Graph Vertex
struct VertexDescription
{
	float3 Position;
	float3 Normal;
	float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
	VertexDescription description = (VertexDescription)0;
	description.Position = IN.ObjectSpacePosition;
	description.Normal = IN.ObjectSpaceNormal;
	description.Tangent = IN.ObjectSpaceTangent;
	return description;
}

	#ifdef FEATURES_GRAPH_VERTEX
Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
{
return output;
}
#define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
#endif

// Graph Pixel
struct SurfaceDescription
{
	float3 BaseColor;
	float Alpha;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
	SurfaceDescription surface = (SurfaceDescription)0;
	UnityTexture2D _Property_07faefce673f4410a2597a3de08a0cb3_Out_0 = UnityBuildTexture2DStructNoScale(_GradientTex);
	float _Property_9934d4862771493790a6bb54ef534744_Out_0 = _GradientIntens;
	UnityTexture2D _Property_f9b3322f712549a5bb753617ec724c7e_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
	float4 _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_f9b3322f712549a5bb753617ec724c7e_Out_0.tex, _Property_f9b3322f712549a5bb753617ec724c7e_Out_0.samplerstate, _Property_f9b3322f712549a5bb753617ec724c7e_Out_0.GetTransformedUV(IN.uv0.xy));
	float _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_R_4 = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0.r;
	float _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_G_5 = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0.g;
	float _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_B_6 = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0.b;
	float _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_A_7 = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0.a;
	float _Float_fc3e0b95bca54f3e91a91e967139b334_Out_0 = (_SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_RGBA_0).x;
	float _Multiply_fa70f1ca055241a1995c748de1e93df0_Out_2;
	Unity_Multiply_float_float(_Property_9934d4862771493790a6bb54ef534744_Out_0, _Float_fc3e0b95bca54f3e91a91e967139b334_Out_0, _Multiply_fa70f1ca055241a1995c748de1e93df0_Out_2);
	float2 _Vector2_b26b08054493422e9b0e8bfd4b57bc4a_Out_0 = float2(_Multiply_fa70f1ca055241a1995c748de1e93df0_Out_2, 0);
	float4 _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_RGBA_0 = SAMPLE_TEXTURE2D(_Property_07faefce673f4410a2597a3de08a0cb3_Out_0.tex, _Property_07faefce673f4410a2597a3de08a0cb3_Out_0.samplerstate, _Property_07faefce673f4410a2597a3de08a0cb3_Out_0.GetTransformedUV(_Vector2_b26b08054493422e9b0e8bfd4b57bc4a_Out_0));
	float _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_R_4 = _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_RGBA_0.r;
	float _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_G_5 = _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_RGBA_0.g;
	float _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_B_6 = _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_RGBA_0.b;
	float _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_A_7 = _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_RGBA_0.a;
	float4 _Combine_6c5ffcf8ece344148761471216d414a2_RGBA_4;
	float3 _Combine_6c5ffcf8ece344148761471216d414a2_RGB_5;
	float2 _Combine_6c5ffcf8ece344148761471216d414a2_RG_6;
	Unity_Combine_float(_SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_R_4, _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_G_5, _SampleTexture2D_b4422e0719e4448eb194710cd8d929bb_B_6, 0, _Combine_6c5ffcf8ece344148761471216d414a2_RGBA_4, _Combine_6c5ffcf8ece344148761471216d414a2_RGB_5, _Combine_6c5ffcf8ece344148761471216d414a2_RG_6);
	surface.BaseColor = _Combine_6c5ffcf8ece344148761471216d414a2_RGB_5;
	surface.Alpha = _SampleTexture2D_3db14f704ba044308fe7e7f430ebe0c8_A_7;
	return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
	VertexDescriptionInputs output;
	ZERO_INITIALIZE(VertexDescriptionInputs, output);

	output.ObjectSpaceNormal = input.normalOS;
	output.ObjectSpaceTangent = input.tangentOS.xyz;
	output.ObjectSpacePosition = input.positionOS;

	return output;
}
	SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
	SurfaceDescriptionInputs output;
	ZERO_INITIALIZE(SurfaceDescriptionInputs, output);







	output.uv0 = input.texCoord0;
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

	return output;
}

	// --------------------------------------------------
	// Main

	#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"

	ENDHLSL
}
		 }
			 CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
		FallBack "Hidden/Shader Graph/FallbackError"
}