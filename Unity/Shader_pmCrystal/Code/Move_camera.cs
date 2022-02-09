using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_camera : MonoBehaviour
{
    public float diff = 0.01f;
    void Update()
    {
        Vector3 pos = this.transform.position;
        Vector3 angle = this.transform.eulerAngles;
        //�L�[�{�[�h���͂ɂ���đO�㍶�E�Ɉړ�
        if (Input.GetKey(KeyCode.A))
        {
            pos = new Vector3(pos.x + diff, pos.y, pos.z);
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos = new Vector3(pos.x - diff, pos.y, pos.z);
        }
        if (Input.GetKey(KeyCode.W))
        {
            pos = new Vector3(pos.x, pos.y, pos.z - diff);
        }
        if (Input.GetKey(KeyCode.S))
        {
            pos = new Vector3(pos.x, pos.y, pos.z + diff);
        }
        this.transform.position = pos;

        //�L�[�{�[�h���͂ɂ���ď㉺���E�ɉ�]
        if (Input.GetKey(KeyCode.Alpha6) == true | Input.GetKey(KeyCode.Keypad6) == true)
        {
            angle = new Vector3(angle.x, angle.y + diff * 10.0f, angle.z);
        }
        if (Input.GetKey(KeyCode.Alpha4) == true | Input.GetKey(KeyCode.Keypad4) == true)
        {
            angle = new Vector3(angle.x, angle.y - diff * 10.0f, angle.z);
        }
        if (Input.GetKey(KeyCode.Alpha8) == true | Input.GetKey(KeyCode.Keypad8) == true)
        {
            angle = new Vector3(angle.x - diff * 10.0f, angle.y, angle.z);
        }
        if (Input.GetKey(KeyCode.Alpha2) == true | Input.GetKey(KeyCode.Keypad2) == true)
        {
            angle = new Vector3(angle.x + diff * 10.0f, angle.y, angle.z);
        }
        this.transform.eulerAngles = angle;
    }
}
