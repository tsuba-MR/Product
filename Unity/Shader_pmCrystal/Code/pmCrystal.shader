Shader "Unlit/pmCrystal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        //ワールド座標のxの値が+だったら通常通りのテクスチャの表示を行う
        Pass
        {
            Tags { "RenderType"="Transparent" "Queue" = "Geometry-1"}
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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
                float3 worldPos : WORLD_POS;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;//座標をワールド空間に変換して変数に格納
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                clip(i.worldPos.x);//ワールド空間が0より小さかったら表示しない
                return col;
            }
            ENDCG
        }
        //マイナスだったら氷にする
         Pass
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Geometry-1"}
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
                        #pragma vertex vert
            #pragma fragment frag

            # include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : WORLD_POS;
                float3 normal : NORMAL;//法線
                float3 viewDir : TEXCOORD2;//視線ベクトル
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;//座標をワールド空間に変換して変数に格納
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.viewDir = normalize(UnityWorldSpaceViewDir(o.worldPos));//視線ベクトルをワールド空間の座標でワールド座標から生成。単位ベクトル
                o.normal = UnityObjectToWorldNormal(v.normal);//法線をオブジェクト空間からワールド空間に座標変換
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.x = 1;
                col.y = 1;
                col.z = 1;//最大反射では白を描画
                float alpha = 1 - (abs(dot(i.viewDir, i.normal)));
                col.w = alpha*1.5;//ワールド空間のx座標値を基に、透明にする
                clip(-1.0 * i.worldPos.x);
                return col;
            }
            ENDCG
        }

        // 影の描画
        Pass
        {
            Tags{ "LightMode" = "ShadowCaster"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };
            struct v2f
            {
                V2F_SHADOW_CASTER;
            };
            v2f vert(appdata v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o);
                return o;
            }
            fixed4 frag(v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i);
            }
            ENDCG
        }
    }
}//Shader
