// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/SpritesOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _OutlineThickness ("Thickness", Float) = 0.1
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        CGPROGRAM
        #pragma surface surf NoLighting vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
        #pragma multi_compile_local _ PIXELSNAP_ON
        #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
        #include "UnitySprites.cginc"

        float _OutlineThickness;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_0;
            fixed4 color;
        };

        void vert (inout appdata_full v, out Input o)
        {
            v.vertex = UnityFlipSprite(v.vertex, _Flip);

            #if defined(PIXELSNAP_ON)
            v.vertex = UnityPixelSnap (v.vertex);
            #endif

            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.color = v.color * _Color * _RendererColor;
        }


         fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
             {
                 fixed4 c;
                 c.rgb = s.Albedo; 
                 c.a = s.Alpha;
                 return c;
             }

        void surf (Input IN, inout SurfaceOutput o)
        {
            float2 uv = IN.uv_MainTex;
            float2 uvU = uv + float2(0, _OutlineThickness);
            float2 uvD = uv + float2(0, -_OutlineThickness);
            float2 uvL = uv + float2(_OutlineThickness, 0);
            float2 uvR = uv + float2(-_OutlineThickness, 0);

            
            //fixed4 c = SampleSpriteTexture (uv);

            fixed4 c0 = SampleSpriteTexture (uvD);
            fixed4 c1 = SampleSpriteTexture (uvU);
            fixed4 c2 = SampleSpriteTexture (uvL);
            fixed4 c3 = SampleSpriteTexture (uvR);

            c0 = max(c0, c2);
            c1 = max(c1, c3);
            c0 = max(c0, c1);

            //c0 = fixed4(IN.color.rgb, c0.a);
            
            o.Albedo = c0.aaa * IN.color;
            o.Alpha = c0.a;
        }
        ENDCG
    }

Fallback "Transparent/VertexLit"
}