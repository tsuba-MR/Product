using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Q�Ƃ��Ή�����I�u�W�F�N�g�ɃA�^�b�`����
public class visible_switch : MonoBehaviour
{
    public SerialHandler serialHandler;
    
    public GameObject onigiri;

    void Start()
    {
        //�M������M�����Ƃ��ɁA���̃��b�Z�[�W�̏������s��
        serialHandler.OnDataReceived += OnDataReceived;
    }

    //��M�����M��(message)�ɑ΂��鏈��
    void OnDataReceived(string message)
    {
        //���������������z��œ���Ă���
        //�����0��1���󂯎��
        var data = message.Split(
                new string[] {""}, System.StringSplitOptions.None);
        
        //���u�̐M���ɂ���āAonigiri�̕\����؂�ւ���
        try
        {
            if(data[0] == "1")
            {
                onigiri.SetActive(true);
            }
            else
            {
                onigiri.SetActive(false);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);//�G���[��\��
        }
    }
}