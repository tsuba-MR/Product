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
            // _LightColor0���g������
            
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
            // �Ώ�Quad�̒��_�����������ăt���X�N���[���ɂ���
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = float4(v.vertex.xy * 2.0, 0.5, 1.0);
                o.uv = v.uv;

                // Direct3D�̂悤��UV�̏㉺�����]�����v���b�g�t�H�[�����l������
                #if UNITY_UV_STARTS_AT_TOP
                    o.uv.y = 1 - o.uv.y;
                #endif
                return o;
            }
            
            // ���������邽�ߎg���Bfmod�͓��͂��}�C�i�X���Əo�͂��}�C�i�X�ɂȂ邽�߁A�K��+�ɂȂ�mod������
            float mod(float x, float y)
            {
                return x - y * floor(x / y);
            }
            float2 mod(float2 x, float2 y)
            {
                return x - y * floor(x / y);
            }

            // �F����HSV����RGB�ɕϊ�
            float3 hsvToRgb(float3 c)
            {
                float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
            }

            // ��]������֐�
            // p�F���C�̐�[�Aangle�F��]������p�x�̒l�Aaxsis�Axyz���ɑ΂��Ăǂ̒��x�̉�]�������邩
            // x100%,y50%,z�Ȃ��Ƃ���ƁAaxis=float3(1,0.5,0.0)
            float3 rotate(float3 p, float angle, float3 axis)
            {
                float3 a = normalize(axis);// 0������ƕ\������Ȃ��Ȃ�
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
            // ����邽�߂̋����֐�
            float sdPlane(float3 p, float3 n, float h)
            {
                // n�͐��K�����ꂽ�@��
                // h�͌��_����̋���
                return dot(p, n) + h;
            }
            // Torus����邽�߂̋����֐�
            float sdTorus(float3 p, float2 t)
            {

                float2 q = float2(length(p.xz) - t.x, p.y);
                return length(q) - t.y;
            }
            // ���𐶐�����֐�
            float dFloor(float3 p)
            {
                return sdPlane(p, float3(0, 1, 0), 0);
            }
            // �����������邽�߂̊֐��B�����XZ���ʂŖ����ɍ��
            float2 opRep(float2 p, float2 interval)
            {
                return mod(p, interval) - interval * 0.5;
            }
            // �h�[�i�c�^�̃v���~�e�B�u������֐�
            float dDounut(float3 p)
            {
                float3 p0 = p - float3(0, 1, 0);
                float3 ax = (0.4, 0.2, 0.1);
                float3 p1 = p0;
                float3 p2 = rotate(p1, 30 * _Time, ax);
                float2 size1 = (0.1, 0.2);
                return sdTorus(p2, size1);
            }
            //�h�[�i�c�^�̃v���~�e�B�u�𖳌��������č��֐�
            float dDounuts(float3 p)
            {
                p.xz = opRep(p.xz, _TorusRepeat);
                float3 p0 = p - float3(_Toruspos, 1+0.3*sin(100*_Time.x), 0);
                float3 ax = (0, 0.2, 0.1);// 105 �s�ڂŃx�N�g���R���X�g���N�^���Ӑ}����Ă�����������Ȃ��ꏊ�ɃJ���}�����g�p����Ă��܂��B
                float3 p1 = rotate(p0, 30 * _Time, ax);//��]
                float3 p2 = p1;
                float2 size1 = (0.2, 0.2);// ��Ɠ����x�����o��
                return sdTorus(p2, size1);
            }
            // ���ƃh�[�i�c��\������
            float map(float3 p)
            {
                float d = dDounuts(p);
                d = min(d, dFloor(p));// 2�̋����֐��̍ŏ��l���a�W���ɂȂ�
                return d;
            }

            // �Δ�������@�����v�Z
            float3 calcNormal(float3 p)
            {
                float eps = 0.001;
                return normalize(float3(
                    map(p + float3(eps, 0.0, 0.0)) - map(p + float3(-eps, 0.0, 0.0)),
                    map(p + float3(0.0, eps, 0.0)) - map(p + float3(0.0, -eps, 0.0)),
                    map(p + float3(0.0, 0.0, eps)) - map(p + float3(0.0, 0.0, -eps))
                ));
            }
            // �A���r�G���g�I�N���[�W�����̌v�Z
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

            // �V���h�E�̌v�Z
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
            //�g�[���}�b�s���O�p�̊֐�
            float3 acesFilm(float3 x)
            {
                const float a = 2.51;
                const float b = 0.03;
                const float c = 2.43;
                const float d = 0.59;
                const float e = 0.14;
                return saturate((x * (a * x + b)) / (x * (c * x + d) + e));
            }
            
            // �s�N�Z���V�F�[�_
            float4 frag(v2f input): SV_Target
            {
                float3 col = float3(0.0, 0.0, 0.0);

                // UV�� -1�`1 �͈̔͂ɕϊ�
                float2 uv = 2.0 * input.uv - 1.0;

                // �J�����̈ʒu
                float3 cameraOrigin = _WorldSpaceCameraPos;

                // �J�����s�񂩂烌�C�𐶐�
                float4 clipRay = float4(uv, 1, 1);// �N���b�v��Ԃ̃��C
                float3 viewRay = normalize(mul(unity_CameraInvProjection, clipRay).xyz);// �r���[��Ԃ̃��C
                float3 ray = mul(transpose((float3x3)UNITY_MATRIX_V), viewRay);// ���[���h��Ԃ̃��C

                // ���C�}�[�`���O
                float t = 0.0;// ���C�̐i�񂾋���
                float3 p = cameraOrigin;// ���C�̐�[�̍��W
                int i = 0;// ���C�}�[�`���O�̃��[�v�J�E���^�[
                bool hit = false;// �I�u�W�F�N�g�ɏՓ˂������ǂ���

                for (i = 0; i < 500; i++)
                {
                    float d = map(p);// �ŒZ�������v�Z

                    // �ŒZ������0�ɋߎ��ł���Ȃ�I�u�W�F�N�g�ɏՓ˂����Ƃ݂Ȃ��ă��[�v�𔲂���
                    if (d < 0.0001)
                    {
                        hit = true;
                        break;
                    }

                    t += d;// �ŒZ�����������C��i�߂�
                    p = cameraOrigin + ray * t;// ���C�̐�[�̍��W���X�V
                }

                if (hit)
                {
                    // ���C�e�B���O�̃p�����[�^�[
                    float3 normal = calcNormal(p);// �@��
                    float3 light = _WorldSpaceLightPos0;// ���s�����̕����x�N�g��

                    // �}�e���A���̃p�����[�^�[
                    float3 albedo = float3(1, 1, 1);// �A���x�h
                    float metalness = 0.5;// �����̓x����

                    // �{�[���̃}�e���A����ݒ�
                    if (dDounuts(p) < 0.0001)
                    {
                        float2 grid = floor(p.xz / _TorusRepeat);
                        albedo = hsvToRgb(float3(dot(grid, _TorusAlbedoHue), 1.0, 1.0));
                        metalness = 0.8;
                        float f0 = 0.8;
                    }

                    // ���̃}�e���A����ݒ�
                    if (dFloor(p) < 0.0001)
                    {
                        float checker = mod(floor(p.x) + floor(p.z), 2.0);
                        albedo = lerp(_FloorAlbedoA, _FloorAlbedoB, checker);
                        metalness = 0.1;
                    }

                    // ���C�e�B���O�v�Z
                    float diffuse = saturate(dot(normal, light));// �����o�[�g�g�U����
                    float specular = pow(saturate(dot(reflect(light, normal), ray)), 10.0);// Phong�̃��f���̋��ʔ���
                    float ao = calcAO(p, normal);// AO
                    float shadow = calcSoftshadow(p, light, 0.25, 5);// �V���h�E

                    // ���C�e�B���O���ʂ̍���
                    col += albedo * diffuse * shadow * (1 - metalness) * _LightColor0.rgb;// ���ڌ��̊g�U����
                    col += albedo * specular * shadow * metalness * _LightColor0.rgb;// ���ڌ��̋��ʔ���
                    col += albedo * ao * lerp(_SkyBottomColor, _SkyTopColor, 0.3);// ����

                    // ���i�̃t�H�O
                    float invFog = exp(-0.02 * t);
                    col = lerp(_SkyBottomColor, col, invFog);
                }
                else
                {
                    // ��
                    col = lerp(_SkyBottomColor, _SkyTopColor, ray.y);
                }

                // �g�[���}�b�s���O
                col = acesFilm(col * 0.8);

                // �K���}�␳
                col = pow(col, 1 / 2.2);

                return float4(col, 1);
            }
            ENDCG
            
        }
    }
}
