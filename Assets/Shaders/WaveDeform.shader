// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/WaveDeform"
{
    Properties
    {
        _ColorTint("Color Tint",Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Side("Side",float) = 1
        _WaveSpeed("Wave Speed",float) = 2
        _WaveAmplitude("Wave Amplitude",float) = 2
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                float _Side;
                float _WaveSpeed;
                float _WaveAmplitude;
                fixed4 _ColorTint;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;

                float angle = _WaveSpeed*_Time * 50;

                v.vertex.y = v.uv.x * sin( v.vertex.x + angle);
                v.vertex.y *= (v.vertex.y) * -_WaveAmplitude;

                o.vertex = UnityObjectToClipPos(_Side*v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= _ColorTint;
                return col;
            }
            ENDCG
        }
    }
}
