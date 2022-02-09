using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class BallAccelerator : MonoBehaviour
{
    //ボールとバットの変数宣言
    public GameObject shuttle, bat;
    public float k = 1.0f;
       
    //シャトルのRigidbodyとラケットの当たり判定
    Rigidbody rb_shuttle;
    Collider col_bat;

    //シャトルとラケットの質量
    float m_shuttle = 0.0055f;
    float m_racket = 0.085f;

    //最終決定速度
    Vector3 force;
    
    //速度格納用
    VelocityEstimator VE;
    Vector3 velo,velo1,velo2 ;

    //衝突のフラグ
    private bool touch = false;
    
    //コンポーネントの取得
    void Start()
    {
        rb_shuttle = shuttle.GetComponent<Rigidbody>();
        col_bat = bat.GetComponent<Collider>();
        VE = GetComponent<VelocityEstimator>();
    }
    /// <動作概要>
    /// コントローラーの並進速度と回転速度を基にシャトルに与える速度を計算する
    /// <取得する値>
    /// velo1 : ラケットの並進速度ベクトル（ラケットそのものが動くときの速度）
    /// velo2 : ラケットの回転速度ベクトル（手首を軸として回転するときの速度）
    /// <出力する値>
    /// velo : バットの最終決定速度。衝突するフレームにおける両ベクトルを加算
    /// </summary>
    void Update()
    { 
        velo1 = VE.GetVelocityEstimate();//ラケットの並進速度
        velo2 = VE.GetAngularVelocityEstimate();//ラケットの回転速度
        //ラケットの並進速度と手首による回転速度を決定
        //回転速度は手～スイートスポットまでの長さが0.5mであるため0.5を乗算している
        velo = velo1 + velo2 * 0.5f;
        Debug.DrawRay(this.transform.localPosition, velo, Color.red);
    }
    /// <動作概要>
    /// 衝突時、最終決定速度を用いてシャトルを法線方向へ飛ばす
    /// <取得する値>
    /// e : シャトルとバット（本来はラケット）の接触点の法線
    /// v : シャトルの速度ベクトル。運動量保存則と力学的エネルギー保存則より計算
    /// <出力する値>
    /// force : シャトルに加える速度。法線方向に向かって計算した速さを加える
    /// <更新する値>
    /// touch : シャトルを一度打った時のフラグ。一度衝突したらフラグを立てることで現実では起こらない2度打ちを防ぐ
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter(Collision col)
    {
        if(touch != true)
        {
            if (col.gameObject.tag == "Shuttlecock")
            {
                foreach (ContactPoint contact in col.contacts)
                {
                    Vector3 e = contact.normal;//接触点の法線
                }
                //運動量から打たれた後のシャトルの速度を計算
                Vector3 v = ((m_shuttle - m_racket)*rb_shuttle.velocity + 2f * m_racket * velo) / (m_shuttle + m_racket);
                Debug.Log("シャトル速度：" + v);
                Debug.Log("velocity estimatorの速度:" + velo);
                //接触点の法線ベクトル x 速さ
                force = -e * k * v.magnitude;
                //決定した速度をシャトル速度に代入
                rb_shuttle.velocity = force;
                Debug.Log("force速度:" + force);
                Debug.Log("シャトルの更新後の速度:" + rb_shuttle.velocity.magnitude);
                touch = true;
                col_bat.enabled = false;
            }
        }
    }
}