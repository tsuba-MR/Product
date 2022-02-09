using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_robo : MonoBehaviour
{
    public float diff = 0.01f;
    // Update is called once per frame
    void Update()
    {
        //キーボード入力によって前後左右に移動
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position = new Vector3(this.transform.position.x + diff, this.transform.position.y, this.transform.position.z);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position = new Vector3(this.transform.position.x - diff, this.transform.position.y, this.transform.position.z);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + diff);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - diff);
        }
    }
}
