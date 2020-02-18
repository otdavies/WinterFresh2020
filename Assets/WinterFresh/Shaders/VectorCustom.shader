Shader "Unlit/VectorCustom"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[MaterialToggle] NoiseDeform ("Noise Deform", Float) = 0
		_NoiseTexture ("Noise Texture", 2D) = "white" {} 
		_NoiseSize("Noise Size", vector) = (1, 1, 1, 1)
		_NoiseAmp("Amplitude", float) = 1.0
		_NoiseSpeed("Noise Speed", float) = 1.0
		_HeightCutoff("Height Cutoff", float) = 1.0
		_HeightFactor("Height Factor", float) = 1.0
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

        Pass
        {
        CGPROGRAM
            #pragma vertex VectorVert
            #pragma fragment SpriteFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ NOISEDEFORM_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

			float4 _NoiseSize;
			float _NoiseAmp;
			float _NoiseSpeed;
			float _HeightCutoff;
			float _HeightFactor;
			sampler2D _NoiseTexture;

            v2f VectorVert(appdata_t IN)
            {
                v2f OUT;

				#ifdef NOISEDEFORM_ON
				float4 worldPos = mul(IN.vertex, unity_ObjectToWorld);
				float2 samplePos = (worldPos.xy / _NoiseSize.xy);
				fixed4 noiseSample = tex2Dlod(_NoiseTexture, float4(sin(samplePos.x + _Time.x * _NoiseSpeed), sin(samplePos.y + _Time.x * _NoiseSpeed), 0,0)) - 0.5;

				float heightFactor = max(pow(worldPos.y - _HeightCutoff, _HeightFactor), 0.001);
				float4 output = worldPos;
				output.x += sin(noiseSample.x * 0.5) * _NoiseAmp * heightFactor;
				output.y += cos(noiseSample.x) * _NoiseAmp * heightFactor * 0.2;
				IN.vertex = mul(output, unity_WorldToObject);
				#endif


                UNITY_SETUP_INSTANCE_ID (IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
                OUT.vertex = UnityObjectToClipPos(OUT.vertex);
                OUT.texcoord = IN.texcoord;

                #ifdef UNITY_COLORSPACE_GAMMA
                fixed4 color = IN.color;
                #else
                fixed4 color = fixed4(GammaToLinearSpace(IN.color.rgb), IN.color.a);
                #endif

                OUT.color = color * _Color * _RendererColor;

				#ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }
        ENDCG
        }
    }
}
