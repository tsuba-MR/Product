using UnityEngine;
using System;//Arrayを使うため
using System.Collections.Generic;

//２つとも対応するオブジェクトにアタッチする
public class SerialReceive_Button : MonoBehaviour
{
    //SerialHandlerを追加。
    public SerialHandler serialHandler;

    private int[] mode = new int[2];//ここはArduino側からの信号によって変更する必要あり

    public GameObject[] cube;//マテリアルを買えるゲームオブジェクト
    public Material[] materials;//マテリアルを格納するところ

    private bool frag = true;//解答権用のフラグ 

    public AudioClip sound1;
    private AudioSource audioSource;

    /// <関数概要>
    /// 必要なコンポーネントを取得し、メッセージ受信処理を始める
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //信号を受信したときに、そのメッセージの処理を行う
        serialHandler.OnDataReceived += OnDataReceived;
    }

    /// <関数概要>
    /// 文字列を受け取り、分割して配列にする
    /// <入力>
    /// message : Arduinoから受け取る文字列。今回は区切り文字":"として分割
    /// </summary>
    void OnDataReceived(string message)
    {
        var data = message.Split(
                new string[] { ":" }, System.StringSplitOptions.None);
        Array.Resize(ref mode, data.Length);
        //分割した文字列を配列で入れている
        try
        {
            //string型からfloat型に変換
            for (int i = 0; i < mode.Length; i++)
            {
                mode[i] = int.Parse(data[i]);//マテリアルチェンジの変数
            }
            MaterialChange(mode);//ボタンが押されたらマテリアルの種類を変更する
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);//エラーを表示
        }
    }
    /// <関数概要>
    /// 配列の中で1が立った番号のCubeのマテリアルを変更し、スポットライトをつけて光らせる
    /// <入力>
    /// mode : 配列。0or1であり、ボタンが押されると1が入る
    /// </summary>
    void MaterialChange(int[] mode)
    {
        int index = Array.IndexOf(mode, 1);//どこにも1がないときは-1が返される
        if (frag == true)
        {
            if (index == -1 || index == mode.Length - 1)//何も押していないときとリセットボタンを押したときのWarningを回避
                return;
            else
            {
                if (mode[index] == 1)//mode配列のどこかが1になったら
                {
                    cube[index].GetComponent<Renderer>().material = materials[2 * index + 1];//対応するcubeのマテリアルをLightに変更する
                    cube[index].transform.GetChild(0).gameObject.SetActive(true);//スポットライトをつける
                    audioSource.PlayOneShot(sound1);
                    frag = false;//一つだけつけるようにし、他のcubeは以降光らないようにする
                }
            }
        }
        //配列の末尾はリセットボタンの扱いにする
        if (mode[mode.Length - 1] == 1)
        {
            for (int i = 0; i < mode.Length - 1; i++)
            {
                cube[i].GetComponent<Renderer>().material = materials[2 * i];
                cube[i].transform.GetChild(0).gameObject.SetActive(false);
                frag = true;//フラグを元に戻す
            }
        }
        
    }
}