Shader "Custom/SpriteOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        
        // Делаем параметры обводки доступными для PropertyBlock
        [PerRendererData] _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        [PerRendererData] _OutlineWidth ("Outline Width", Float) = 1
        [PerRendererData] _TextureSize ("Texture Size", Float) = 512
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
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
            };

            fixed4 _Color;
            fixed4 _OutlineColor;
            float _OutlineWidth;
            float _TextureSize;
            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float _AlphaSplitEnabled;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap(OUT.vertex);
                #endif

                return OUT;
            }

            fixed4 SampleSpriteTexture (float2 uv)
            {
                fixed4 color = tex2D (_MainTex, uv);

                #if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
                if (_AlphaSplitEnabled)
                    color.a = tex2D (_AlphaTex, uv).r;
                #endif

                return color;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
                
                // Если пиксель прозрачный, проверяем соседей на наличие непрозрачных пикселей
                if (c.a == 0)
                {
                    // Рассчитываем размер одного пикселя в UV координатах
                    float pixelSize = 1.0 / _TextureSize;
                    float outlinePixelSize = pixelSize * _OutlineWidth;
                    
                    // Проверяем 8 соседних пикселей
                    half up = tex2D(_MainTex, IN.texcoord + float2(0, outlinePixelSize)).a;
                    half down = tex2D(_MainTex, IN.texcoord - float2(0, outlinePixelSize)).a;
                    half left = tex2D(_MainTex, IN.texcoord - float2(outlinePixelSize, 0)).a;
                    half right = tex2D(_MainTex, IN.texcoord + float2(outlinePixelSize, 0)).a;
                    
                    half upLeft = tex2D(_MainTex, IN.texcoord + float2(-outlinePixelSize, outlinePixelSize)).a;
                    half upRight = tex2D(_MainTex, IN.texcoord + float2(outlinePixelSize, outlinePixelSize)).a;
                    half downLeft = tex2D(_MainTex, IN.texcoord + float2(-outlinePixelSize, -outlinePixelSize)).a;
                    half downRight = tex2D(_MainTex, IN.texcoord + float2(outlinePixelSize, -outlinePixelSize)).a;
                    
                    // Если хотя бы один сосед непрозрачный, рисуем обводку
                    if (up + down + left + right + upLeft + upRight + downLeft + downRight > 0)
                    {
                        c.rgb = _OutlineColor.rgb;
                        c.a = _OutlineColor.a;
                    }
                }
                
                c.rgb *= c.a;
                return c;
            }
            ENDCG
        }
    }
}