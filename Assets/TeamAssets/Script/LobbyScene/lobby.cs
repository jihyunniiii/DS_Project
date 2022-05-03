using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobby : MonoBehaviour
{
    public GameObject UI;
    public GameObject SettingUI;
   
    // Start is called before the first frame update
    void Start()
    {
        UI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (UI.activeSelf == true)
            {
                UI.gameObject.SetActive(false);
            }

            else if (UI.activeSelf == false && SettingUI.activeSelf == false)
            {
                UI.gameObject.SetActive(true);
            }
        }
    }

    public void SettingUIOn()
    {
        if (UI.activeSelf == true)
        {
            SettingUI.gameObject.SetActive(true);
            UI.gameObject.SetActive(false);
        }
    }

}
