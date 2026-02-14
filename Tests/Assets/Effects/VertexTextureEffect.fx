// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
#include "Include.fxh"

matrix WorldViewProj;

float HeightMapSize;

Texture2D HeightMapTexture;

sampler HeightMapSampler = sampler_state
{
    Texture = (HeightMapTexture);
    MinFilter = POINT;
    MagFilter = POINT;
    MipFilter = NONE;
};

struct VSOutput
{
    float4 PositionPS : SV_Position;
    float4 Color : COLOR0;
};

VSOutput VS_Main(float2 xy : POSITION)
{
    float2 uv = (xy + float2(0.5, 0.5)) / HeightMapSize;
#if SM6 || SM4
    float height = HeightMapTexture.SampleLevel(HeightMapSampler, uv, 0).r;
#else
    float height = tex2Dlod(HeightMapSampler, float4(uv, 0, 0)).r;
#endif
    float3 worldPosition = float3(xy.x, height, xy.y);

    VSOutput output;
    output.PositionPS = mul(float4(worldPosition, 1), WorldViewProj);
    output.Color = float4(xy.x / HeightMapSize, xy.y / HeightMapSize, 0, 1);

    return output;
}

float4 PS_Main(VSOutput input) : SV_TARGET0
{
    return input.Color;
}

technique
{
    pass
    {
        VertexShader = compile VS_PROFILE VS_Main();
        PixelShader = compile PS_PROFILE PS_Main();
    }
}
