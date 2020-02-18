Shader "Unlit/VectorGradientCustom"
{
    Properties
    {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
		_ColorTop ("ColorTop", Color) = (1,1,1,1)
        _ColorBottom ("ColorBottom", Color) = (0,0,0,0)
		_FadePixelFalloff("Fade Height", Range(0, 0.01)) = 100
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "PreviewType" = "Plane"
        }
        LOD 100

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex GradientVert
            #pragma fragment GradientFrag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            #ifdef UNITY_INSTANCING_ENABLED
            UNITY_INSTANCING_BUFFER_START(PerDrawSprite)
                UNITY_DEFINE_INSTANCED_PROP(fixed4, unity_SpriteRendererColorArray)
            UNITY_INSTANCING_BUFFER_END(PerDrawSprite)
            #define _RendererColor  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteRendererColorArray)
            #endif

            #ifndef UNITY_INSTANCING_ENABLED
            fixed4 _RendererColor;
            #endif

            struct appdata
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
                float2 settingIndex : TEXCOORD2;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0; // uv.z is used for setting index
                float2 settingIndex : TEXCOORD2;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
			float _FadePixelFalloff;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            fixed4 _ColorTop;
            fixed4 _ColorBottom;

            v2f GradientVert (appdata IN)
            {
                v2f OUT;

                UNITY_SETUP_INSTANCE_ID (IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                #ifdef UNITY_COLORSPACE_GAMMA
                OUT.color = IN.color;
                #else
                OUT.color = fixed4(GammaToLinearSpace(IN.color.rgb), IN.color.a);
                #endif
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.settingIndex = IN.settingIndex;
                return OUT;
            }

            fixed4 GradientFrag (v2f i) : SV_Target
            {
                fixed4 gradColor = lerp(_ColorBottom, _ColorTop, i.vertex.y * _FadePixelFalloff);
                
                #ifndef UNITY_COLORSPACE_GAMMA
                gradColor = fixed4(GammaToLinearSpace(gradColor.rgb), gradColor.a);
                #endif

                fixed4 finalColor = gradColor;
                finalColor.rgb *= finalColor.a;

                return finalColor;
            }
            ENDCG
        }
    }
}
