// contorno (inverted hull) para o item mirado – URP
// desenha a malha um pouco "inflada" pelas normais, só as faces de trás, numa cor sólida
Shader "TCC/Silhueta"
{
    Properties
    {
        _Cor ("Cor", Color) = (1, 1, 1, 1)
        _Espessura ("Espessura", Range(0, 0.2)) = 0.03
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" }

        Pass
        {
            Name "Silhueta"
            Tags { "LightMode" = "SRPDefaultUnlit" }
            Cull Front
            ZWrite On

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _Cor;
                float _Espessura;
            CBUFFER_END

            struct Atributos
            {
                float4 posicao : POSITION;
                float3 normal : NORMAL;
            };

            struct Variaveis
            {
                float4 posicao : SV_POSITION;
            };

            Variaveis vert (Atributos entrada)
            {
                Variaveis saida;
                float3 posicaoMundo = TransformObjectToWorld(entrada.posicao.xyz);
                float3 normalMundo = TransformObjectToWorldNormal(entrada.normal);
                posicaoMundo += normalMundo * _Espessura;   // espessura em metros, independente da escala
                saida.posicao = TransformWorldToHClip(posicaoMundo);
                return saida;
            }

            half4 frag (Variaveis entrada) : SV_Target
            {
                return _Cor;
            }
            ENDHLSL
        }
    }
}
