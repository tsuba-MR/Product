using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//２つとも対応するオブジェクトにアタッチする
public class visible_switch : MonoBehaviour
{
    public SerialHandler serialHandler;
    
    public GameObject onigiri;

    void Start()
    {
        //信号を受信したときに、そのメッセージの処理を行う
        serialHandler.OnDataReceived += OnDataReceived;
    }

    //受信した信号(message)に対する処理
    void OnDataReceived(string message)
    {
        //分割した文字列を配列で入れている
        //今回は0か1を受け取る
        var data = message.Split(
                new string[] {""}, System.StringSplitOptions.None);
        
        //装置の信号によって、onigiriの表示を切り替える
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
            Debug.LogWarning(e.Message);//エラーを表示
        }
    }
}