using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;


public class Button : MonoBehaviour
{
    //トリガー
    private SteamVR_Action_Boolean Iui = SteamVR_Actions.default_InteractUI;
    //結果の格納用Boolean型関数interacrtui
    private bool interacrtui;
    //側面ボタン
    private SteamVR_Action_Boolean GrabG = SteamVR_Actions.default_GrabGrip;
    //結果の格納用Boolean型関数grapgrip
    private bool grapgrip;
    GameObject refobj;
    BallShoot_DL_modi_copy shoot_c;
    bool flag = true;
    private void Start()
    {
        refobj = GameObject.Find("shuttle");
        shoot_c = refobj.GetComponent<BallShoot_DL_modi_copy>();
    }

    /// <関数概要>
    /// HTCViveコントローラーの入力を読み取り、トリガーが押されたらシャトルが発射され、グリップであればシーンをリセットする
    /// </summary>
    void Update()
    {
        interacrtui = Iui.GetState(SteamVR_Input_Sources.RightHand);
        if (interacrtui)
        {
            if(flag == true)
            {
                shoot_c.Shoot();//シュートを実行
                flag = false;
            }
        }

        grapgrip = GrabG.GetState(SteamVR_Input_Sources.RightHand);
        if (grapgrip)
        {
            flag = true;
            SceneManager.LoadScene(0);//シーンをリセットする
        }
    }
}
