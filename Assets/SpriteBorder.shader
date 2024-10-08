Shader "Custom/SpriteBorder"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _BorderColor ("Border Color", Color) = (1,1,1,1)
        _BorderSize ("Border Size", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _BorderColor;
            float _BorderSize;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);

                float dist = min(i.uv.x, 1.0 - i.uv.x);
                dist = min(dist, min(i.uv.y, 1.0 - i.uv.y));

                float border = smoothstep(_BorderSize, _BorderSize - 0.02, dist);

                return lerp(col, _BorderColor, border);
            }
            ENDCG
        }
    }
}
