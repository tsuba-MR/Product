using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitController : MonoBehaviour
{
    public GameObject ok;
    //public GameObject no;
    //public GameObject image;
    // Start is called before the first frame update

    private void Start()
    {
        ok.SetActive(false);
        //no.SetActive(false);
        //image.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)==true)
        {
            ok.SetActive(true);
            //no.SetActive(true);
            //image.SetActive(true);
        }
    }
    public void ExitClick()
    {
        Debug.Log("ƒ{ƒ^ƒ“‚ð‰Ÿ‚µ‚½");
        Application.Quit();
    }
    public void CanselClick()
    {
        ok.SetActive(false);
        //no.SetActive(false);
        //image.SetActive(false);
        return;
    }

}
