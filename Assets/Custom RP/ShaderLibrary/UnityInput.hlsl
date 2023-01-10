/*
 * 该文件内包含了由 Unity 生成并传递给 Shader 的数据，要使用这些数据，
 * 只需要包含当前文件就行，不用自己设置
 */

#ifndef CRP_UNITY_INPUT_HLSL
#define CRP_UNITY_INPUT_HLSL

float4x4 unity_ObjectToWorld;
float4x4 unity_WorldToObject;

// 该向量里包含了一些转换信息，real4 是通过宏定义的类型，具体类型视平台而定，所以在
// 引入当前文件前，需要先引入 Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl
real4 unity_WorldTransformParams;

float4x4 unity_MatrixVP;
float4x4 unity_MatrixV;
float4x4 glstate_matrix_projection;

#endif