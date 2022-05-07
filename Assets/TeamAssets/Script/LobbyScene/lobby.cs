using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;
using UnityEngine.UI;

public class lobby : MonoBehaviour
{
    public GameObject UI;
    public GameObject SettingUI;
    public GameObject SettingSound;
    public AudioMixer masterMixer;
    public Slider audioSlider;
   
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

    public void SettingSoundOn()
    {
        if (SettingUI.activeSelf == true && UI.activeSelf == false)
        {
            SettingUI.gameObject.SetActive(false);
            SettingSound.gameObject.SetActive(true);
        }
    }

    public void SettingGraphicOn()
    {
        if (SettingSound.activeSelf == true && UI.activeSelf == false)
        {
            SettingSound.gameObject.SetActive(false);
            SettingUI.gameObject.SetActive(true);
        }
    }

    public void CheckButtonClick()
    {
        if (SettingUI.activeSelf == true && UI.activeSelf == false)
        {
            SettingUI.gameObject.SetActive(false);
            UI.gameObject.SetActive(true);
        }

        else if (SettingSound.activeSelf == true && UI.activeSelf == false)
        {
            SettingSound.gameObject.SetActive(false);
            UI.gameObject.SetActive(true);
        }
    }

    public void AudioControl(float sliderVal)
    {
        masterMixer.SetFloat("Music", Mathf.Log10(sliderVal) * 20);
    }

    public void Camera()
    {

    }
}
