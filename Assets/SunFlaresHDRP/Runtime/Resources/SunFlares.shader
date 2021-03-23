Shader "Hidden/Shader/Sun Flares HDRP"
{
    HLSLINCLUDE

    #pragma target 4.5
    #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/FXAA.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/RTUpscale.hlsl"

    ENDHLSL

    SubShader
    {
        ZWrite Off
        ZTest Always
        Blend Off
        Cull Off

        Pass
        {
            Name "Sun Flares"
            HLSLPROGRAM
                #pragma fragment FragSF
                #pragma vertex Vert
                #pragma multi_compile_local _ SF_HEXAGONAL_SHAPE SF_PENTAGONAL_SHAPE SF_OCTOGONAL_SHAPE
                #include "SunFlaresPass.hlsl"
            ENDHLSL
        }

        Pass
        {
            Name "Pass Through"
            HLSLPROGRAM
                #pragma fragment FragSFNull
                #pragma vertex Vert
                #include "SunFlaresPass.hlsl"
            ENDHLSL
        }
    }
    Fallback Off
}
