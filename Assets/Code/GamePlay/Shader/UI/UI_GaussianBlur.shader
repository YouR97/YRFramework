Shader "YR/UI_GaussianBlur"
{
    Properties
    {
        [PerRendererData] _MainTex("MainTex", 2D) = "white" {}
        //值越大，正态分布图像越扁，距离远的像素的权重越高，颗粒感越明显
        _Color("Color", color) = (1,1,1,1)
        _Sigma("Sigma", Range(1, 30)) = 20
        //多少倍的Sigma，值越大，采样越多，越细腻，会循环(2n+1)(2n+1)次，所以这个值不要太大，太大也没用
        _Ratio("Sigma Ratio", Range(1, 4)) = 2
    }

    SubShader
    {
        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma target 2.0
 
            #include "UnityCG.cginc"

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
            fixed4 _Color;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
 
            float _Sigma;
            float _Ratio;

            // 计算高斯权重
            float computerBluGauss(float x, float sigma)
            {
                return 0.39894 * exp(-0.5 * x * x / (0.20 * sigma)) / sigma * sigma;
            }
 
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float sum = 0;
                fixed4 col = fixed4(0, 0, 0, 0);
                for (float j = -_Ratio * _Sigma; j <= _Ratio * _Sigma; j += _Sigma)
                {
                    for (float k = -_Ratio * _Sigma; k <= _Ratio * _Sigma; k += _Sigma)
                    {
                        float2 _offset = float2(j, k) * _MainTex_TexelSize.xy;
                        float2 blurUV = i.uv + _offset;
                        if(blurUV.x >= 0 && blurUV.x <= 1 && blurUV.y >= 0 && blurUV.y <= 1)
                        {
                            //二维正态分布，公式：G(x,y)=exp(-(x²+y²)/(2σ²))/(2πσ²)
                            float weight = exp((_offset.x * _offset.x + _offset.y * _offset.y)/(-2 * _Sigma * _Sigma))/(6.2831852 * _Sigma * _Sigma);
                            col += tex2D(_MainTex, blurUV) * weight;
                            sum += weight;
                        }
                    }
                }  

                return (col / sum) * _Color;
            }
            ENDCG
        }
        
    }
    
    Fallback "YR/UI_GaussianBlur_Low"
}
