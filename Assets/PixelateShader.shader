Shader "Hidden/Custom/Pixel"
{
  HLSLINCLUDE
      #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
      TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
      int _Posterize;
      int _ColorQuality;

      #define ASPECT 16.f/9.f
      
      float4 Frag(VaryingsDefault i) : SV_Target
      {
          float2 uv = float2(i.texcoord.x * _Posterize * ASPECT, i.texcoord.y * _Posterize);
          uv = round(uv);
          uv.x = uv.x / (_Posterize * ASPECT);
          uv.y = uv.y / _Posterize;
          float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
          color *= _ColorQuality;
          color = round(color);
          color /= _ColorQuality;
          color.a = 1;
          
          return color;
      }
  ENDHLSL
  SubShader
  {
      Cull Off ZWrite Off ZTest Always
      Pass
      {
          HLSLPROGRAM
              #pragma vertex VertDefault
              #pragma fragment Frag
          ENDHLSL
      }
  }
}
