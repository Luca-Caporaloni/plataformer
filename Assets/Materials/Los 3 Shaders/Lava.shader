Shader "Unlit/Lava"
{
   Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TimeScale ("Time Scale", Float) = 0.1
        _FlowSpeed1 ("Flow Speed 1", Float) = 0.6
        _FlowSpeed2 ("Flow Speed 2", Float) = 1.9
        _Intensity ("Intensity", Float) = 1.4
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _TimeScale;
            float _FlowSpeed1;
            float _FlowSpeed2;
            float _Intensity;

            float2 iResolution = float2(1.0, 1.0);

            float hash21(float2 n)
            {
                return frac(sin(dot(n, float2(12.9898, 4.1414))) * 43758.5453);
            }

            // HLSL correction for rotation matrix creation
            float2x2 makem2(float theta)
            {
                float c = cos(theta);
                float s = sin(theta);
                return float2x2(c, -s, s, c);
            }

            float noise(float2 x)
            {
                return tex2D(_MainTex, x * 0.01).x;
            }

            float2 gradn(float2 p)
            {
                float ep = 0.09;
                float gradx = noise(float2(p.x + ep, p.y)) - noise(float2(p.x - ep, p.y));
                float grady = noise(float2(p.x, p.y + ep)) - noise(float2(p.x, p.y - ep));
                return float2(gradx, grady);
            }

            float flow(float2 p, float time)
            {
                float z = 2.0;
                float rz = 0.0;
                float2 bp = p;
                for (float i = 1.0; i < 7.0; i++)
                {
                    p += time * _FlowSpeed1;
                    bp += time * _FlowSpeed2;
                    float2 gr = gradn(i * p * 0.34 + time * 1.0);
                    gr = mul(gr, makem2(time * 6.0 - (0.05 * p.x + 0.03 * p.y) * 40.0));
                    p += gr * 0.5;
                    rz += (sin(noise(p) * 7.0) * 0.5 + 0.5) / z;
                    p = lerp(bp, p, 0.77);
                    z *= _Intensity;
                    p *= 2.0;
                    bp *= 1.9;
                }
                return rz;
            }

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float time = _Time.y * _TimeScale;
                float2 p = i.uv * iResolution - 0.5;
                p.x *= iResolution.x / iResolution.y;
                p *= 3.0;
                float rz = flow(p, time);
                float3 col = float3(0.2, 0.07, 0.01) / rz;
                col = pow(col, float3(1.4, 1.4, 1.4)); // ensure the pow function receives correct arguments
                return float4(col, 1.0);
            }
            ENDCG
        }
    }
}