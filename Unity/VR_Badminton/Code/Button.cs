using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;


public class Button : MonoBehaviour
{
    //�g���K�[
    private SteamVR_Action_Boolean Iui = SteamVR_Actions.default_InteractUI;
    //���ʂ̊i�[�pBoolean�^�֐�interacrtui
    private bool interacrtui;
    //���ʃ{�^��
    private SteamVR_Action_Boolean GrabG = SteamVR_Actions.default_GrabGrip;
    //���ʂ̊i�[�pBoolean�^�֐�grapgrip
    private bool grapgrip;
    GameObject refobj;
    BallShoot_DL_modi_copy shoot_c;
    bool flag = true;
    private void Start()
    {
        refobj = GameObject.Find("shuttle");
        shoot_c = refobj.GetComponent<BallShoot_DL_modi_copy>();
    }

    /// <�֐��T�v>
    /// HTCVive�R���g���[���[�̓��͂�ǂݎ��A�g���K�[�������ꂽ��V���g�������˂���A�O���b�v�ł���΃V�[�������Z�b�g����
    /// </summary>
    void Update()
    {
        interacrtui = Iui.GetState(SteamVR_Input_Sources.RightHand);
        if (interacrtui)
        {
            if(flag == true)
            {
                shoot_c.Shoot();//�V���[�g�����s
                flag = false;
            }
        }

        grapgrip = GrabG.GetState(SteamVR_Input_Sources.RightHand);
        if (grapgrip)
        {
            flag = true;
            SceneManager.LoadScene(0);//�V�[�������Z�b�g����
        }
    }
}
