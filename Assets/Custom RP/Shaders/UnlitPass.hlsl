#ifndef CRP_UNLIT_PASS_HLSL
#define CRP_UNLIT_PASS_HLSL

#include "../ShaderLibrary/Common.hlsl"

float4 _BaseColor;

float4 UnlitPassVertex(float3 positionOS : POSITION) : SV_POSITION {
    float3 positionWS = TransformObjectToWorld(positionOS.xyz);
    return TransformWorldToHClip(positionWS);
}

float4 UnlitPassFragment() : SV_TARGET {
    return _BaseColor;
}

#endif