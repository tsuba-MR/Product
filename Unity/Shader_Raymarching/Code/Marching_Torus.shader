Shader "Unlit/Marching_Torus"
{
    Properties
    {
        _TorusAlbedoHue("Torus Albedo Hue", Vector) = (0.1, 0.1, 1, 1)
        _Toruspos("Torus Position", Range(0,1)) = 0.5
        _TorusRepeat("Torus Repeat", Range(0, 4)) = 4
        _FloorAlbedoA("Floor Albedo A", Color) = (0, 0, 0, 1)
        _FloorAlbedoB("Floor Albedo B", Color) = (1, 1, 1, 1)
        _SkyTopColor("Sky Top Color", Color) = (1, 1, 1, 1)
        _SkyBottomColor("Sky Bottom Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        Pass
        {
            Cull Off
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            # include "UnityCG.cginc"
            # include "UnityLightingCommon.cginc" 
            // _LightColor0を使うため
            
            float3 _TorusAlbedoHue;
            float _Toruspos;
            float _TorusRepeat;
            float3 _FloorAlbedoA;
            float3 _FloorAlbedoB;
            float3 _SkyTopColor;
            float3 _SkyBottomColor;


            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
            };

            struct v2f
            {
                float2 uv: TEXCOORD0;
                float4 vertex: SV_POSITION;
            };
            // 対象Quadの頂点情報をいじってフルスクリーンにする
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = float4(v.vertex.xy * 2.0, 0.5, 1.0);
                o.uv = v.uv;

                // Direct3DのようにUVの上下が反転したプラットフォームを考慮する
                #if UNITY_UV_STARTS_AT_TOP
                    o.uv.y = 1 - o.uv.y;
                #endif
                return o;
            }
            
            // 複製をするため使う。fmodは入力がマイナスだと出力もマイナスになるため、必ず+になるmodを自作
            float mod(float x, float y)
            {
                return x - y * floor(x / y);
            }
            float2 mod(float2 x, float2 y)
            {
                return x - y * floor(x / y);
            }

            // 色情報をHSVからRGBに変換
            float3 hsvToRgb(float3 c)
            {
                float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
            }

            // 回転させる関数
            // p：レイの先端、angle：回転させる角度の値、axsis、xyz軸に対してどの程度の回転を加えるか
            // x100%,y50%,zなしとすると、axis=float3(1,0.5,0.0)
            float3 rotate(float3 p, float angle, float3 axis)
            {
                float3 a = normalize(axis);// 0割すると表示されなくなる
                float s = sin(angle);
                float c = cos(angle);
                float r = 1.0 - c;
                float3x3 m = float3x3(
                    a.x * a.x * r + c,
                    a.y * a.x * r + a.z * s,
                    a.z * a.x * r - a.y * s,
                    a.x * a.y * r - a.z * s,
                    a.y * a.y * r + c,
                    a.z * a.y * r + a.x * s,
                    a.x * a.z * r + a.y * s,
                    a.y * a.z * r - a.x * s,
                    a.z * a.z * r + c
                );
                return mul(m, p);
            }
            // 板を作るための距離関数
            float sdPlane(float3 p, float3 n, float h)
            {
                // nは正規化された法線
                // hは原点からの距離
                return dot(p, n) + h;
            }
            // Torusを作るための距離関数
            float sdTorus(float3 p, float2 t)
            {

                float2 q = float2(length(p.xz) - t.x, p.y);
                return length(q) - t.y;
            }
            // 床を生成する関数
            float dFloor(float3 p)
            {
                return sdPlane(p, float3(0, 1, 0), 0);
            }
            // 無限複製するための関数。今回はXZ平面で無限に作る
            float2 opRep(float2 p, float2 interval)
            {
                return mod(p, interval) - interval * 0.5;
            }
            // ドーナツ型のプリミティブを一つ作る関数
            float dDounut(float3 p)
            {
                float3 p0 = p - float3(0, 1, 0);
                float3 ax = (0.4, 0.2, 0.1);
                float3 p1 = p0;
                float3 p2 = rotate(p1, 30 * _Time, ax);
                float2 size1 = (0.1, 0.2);
                return sdTorus(p2, size1);
            }
            //ドーナツ型のプリミティブを無限複製して作る関数
            float dDounuts(float3 p)
            {
                p.xz = opRep(p.xz, _TorusRepeat);
                float3 p0 = p - float3(_Toruspos, 1+0.3*sin(100*_Time.x), 0);
                float3 ax = (0, 0.2, 0.1);// 105 行目でベクトルコンストラクタが意図されていたかもしれない場所にカンマ式が使用されています。
                float3 p1 = rotate(p0, 30 * _Time, ax);//回転
                float3 p2 = p1;
                float2 size1 = (0.2, 0.2);// 上と同じ警告が出る
                return sdTorus(p2, size1);
            }
            // 床とドーナツを表示する
            float map(float3 p)
            {
                float d = dDounuts(p);
                d = min(d, dFloor(p));// 2つの距離関数の最小値が和集合になる
                return d;
            }

            // 偏微分から法線を計算
            float3 calcNormal(float3 p)
            {
                float eps = 0.001;
                return normalize(float3(
                    map(p + float3(eps, 0.0, 0.0)) - map(p + float3(-eps, 0.0, 0.0)),
                    map(p + float3(0.0, eps, 0.0)) - map(p + float3(0.0, -eps, 0.0)),
                    map(p + float3(0.0, 0.0, eps)) - map(p + float3(0.0, 0.0, -eps))
                ));
            }
            // アンビエントオクルージョンの計算
            float calcAO(float3 pos, float3 nor)
            {
                float occ = 0.0;
                float sca = 1.0;
                for (int i = 0; i < 5; i++)
                {
                    float h = 0.01 + 0.12 * float(i) / 4.0;
                    float d = map(pos + h * nor).x;
                    occ += (h - d) * sca;
                    sca *= 0.95;
                    if (occ > 0.35) break;
                }
                return clamp(1.0 - 3.0 * occ, 0.0, 1.0) * (0.5 + 0.5 * nor.y);
            }

            // シャドウの計算
            float calcSoftshadow(float3 ro, float3 rd, float mint, float tmax)
            {
                // bounding volume
                float tp = (0.8 - ro.y) / rd.y;
                if (tp > 0.0) tmax = min(tmax, tp);

                float res = 1.0;
                float t = mint;
                for (int i = 0; i < 24; i++)
                {
                    float h = map(ro + rd * t).x;
                    float s = clamp(8.0 * h / t, 0.0, 1.0);
                    res = min(res, s * s * (3.0 - 2.0 * s));
                    t += clamp(h, 0.02, 0.2);
                    if (res < 0.004 || t > tmax) break;
                }
                return clamp(res, 0.0, 1.0);
            }
            //トーンマッピング用の関数
            float3 acesFilm(float3 x)
            {
                const float a = 2.51;
                const float b = 0.03;
                const float c = 2.43;
                const float d = 0.59;
                const float e = 0.14;
                return saturate((x * (a * x + b)) / (x * (c * x + d) + e));
            }
            
            // ピクセルシェーダ
            float4 frag(v2f input): SV_Target
            {
                float3 col = float3(0.0, 0.0, 0.0);

                // UVを -1～1 の範囲に変換
                float2 uv = 2.0 * input.uv - 1.0;

                // カメラの位置
                float3 cameraOrigin = _WorldSpaceCameraPos;

                // カメラ行列からレイを生成
                float4 clipRay = float4(uv, 1, 1);// クリップ空間のレイ
                float3 viewRay = normalize(mul(unity_CameraInvProjection, clipRay).xyz);// ビュー空間のレイ
                float3 ray = mul(transpose((float3x3)UNITY_MATRIX_V), viewRay);// ワールド空間のレイ

                // レイマーチング
                float t = 0.0;// レイの進んだ距離
                float3 p = cameraOrigin;// レイの先端の座標
                int i = 0;// レイマーチングのループカウンター
                bool hit = false;// オブジェクトに衝突したかどうか

                for (i = 0; i < 500; i++)
                {
                    float d = map(p);// 最短距離を計算

                    // 最短距離を0に近似できるならオブジェクトに衝突したとみなしてループを抜ける
                    if (d < 0.0001)
                    {
                        hit = true;
                        break;
                    }

                    t += d;// 最短距離だけレイを進める
                    p = cameraOrigin + ray * t;// レイの先端の座標を更新
                }

                if (hit)
                {
                    // ライティングのパラメーター
                    float3 normal = calcNormal(p);// 法線
                    float3 light = _WorldSpaceLightPos0;// 平行光源の方向ベクトル

                    // マテリアルのパラメーター
                    float3 albedo = float3(1, 1, 1);// アルベド
                    float metalness = 0.5;// 金属の度合い

                    // ボールのマテリアルを設定
                    if (dDounuts(p) < 0.0001)
                    {
                        float2 grid = floor(p.xz / _TorusRepeat);
                        albedo = hsvToRgb(float3(dot(grid, _TorusAlbedoHue), 1.0, 1.0));
                        metalness = 0.8;
                        float f0 = 0.8;
                    }

                    // 床のマテリアルを設定
                    if (dFloor(p) < 0.0001)
                    {
                        float checker = mod(floor(p.x) + floor(p.z), 2.0);
                        albedo = lerp(_FloorAlbedoA, _FloorAlbedoB, checker);
                        metalness = 0.1;
                    }

                    // ライティング計算
                    float diffuse = saturate(dot(normal, light));// ランバート拡散反射
                    float specular = pow(saturate(dot(reflect(light, normal), ray)), 10.0);// Phongのモデルの鏡面反射
                    float ao = calcAO(p, normal);// AO
                    float shadow = calcSoftshadow(p, light, 0.25, 5);// シャドウ

                    // ライティング結果の合成
                    col += albedo * diffuse * shadow * (1 - metalness) * _LightColor0.rgb;// 直接光の拡散反射
                    col += albedo * specular * shadow * metalness * _LightColor0.rgb;// 直接光の鏡面反射
                    col += albedo * ao * lerp(_SkyBottomColor, _SkyTopColor, 0.3);// 環境光

                    // 遠景のフォグ
                    float invFog = exp(-0.02 * t);
                    col = lerp(_SkyBottomColor, col, invFog);
                }
                else
                {
                    // 空
                    col = lerp(_SkyBottomColor, _SkyTopColor, ray.y);
                }

                // トーンマッピング
                col = acesFilm(col * 0.8);

                // ガンマ補正
                col = pow(col, 1 / 2.2);

                return float4(col, 1);
            }
            ENDCG
            
        }
    }
}
