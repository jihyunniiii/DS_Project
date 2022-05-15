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
    public GameObject RankUI;

    public AudioMixer masterMixer;
    public Slider audioSlider;
<<<<<<< Updated upstream
   
=======
    public AudioSource audioSource;

    public GameObject StoreBox;
    public GameObject StoreUI;
    public Camera getCamera;
    private RaycastHit hit;

    public TextMeshProUGUI CoinTxt;
    public TextMeshProUGUI LanternTxt;
    public int Coin;
    public int Lantern;

    public GameObject GameRoomBox;

    public TMP_InputField WishInput;
    private string Wish = null;
    public TextMeshProUGUI BoardLanternTxt;
    public TextMeshProUGUI ScrollViewTxt;
    public GameObject LanternBoardBox;
    public GameObject LanternBoardUI;

    public GameObject LanternOriginal;
    public int LanternNum = 0;


>>>>>>> Stashed changes
    // Start is called before the first frame update
    void Start()
    {
        UI.gameObject.SetActive(false);
<<<<<<< Updated upstream
=======
        Coin = 10;
        Lantern = 5;
        CoinTxt.text = ": " + Coin.ToString();
        LanternTxt.text = ": " + Lantern.ToString();
        BoardLanternTxt.text = ": " + Lantern.ToString();
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        if (Input.GetKeyUp(KeyCode.Escape))
=======

        if (StoreUI.activeSelf == false && LanternBoardUI.activeSelf == false && Input.GetKeyUp(KeyCode.Escape))
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = getCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // string objectName = hit.collider.gameObject.name;
                
                // Store UI 활성화
                if (hit.collider.gameObject == StoreBox && SettingSound.activeSelf == false && UI.activeSelf == false && SettingUI.activeSelf == false && LanternBoardUI.activeSelf == false)
                {
                    Debug.Log("상점에 들어와 물품을 구매합니다.");
                    StoreUI.gameObject.SetActive(true);
                    CoinTxt.text = ": " + Coin.ToString();
                    LanternTxt.text = ": " + Lantern.ToString();
                }

                // 게임룸 입장
                if (hit.collider.gameObject == GameRoomBox && SettingSound.activeSelf == false && UI.activeSelf == false && SettingUI.activeSelf == false && StoreUI.activeSelf == false && LanternBoardUI.activeSelf == false)
                {
                    Debug.Log("게임룸으로 이동합니다.");
                    SceneManager.LoadScene("GameRoom");
                }

                // 연등 소원 적기 + 설치 UI 활성화
                if (hit.collider.gameObject == LanternBoardBox && SettingSound.activeSelf == false && UI.activeSelf == false && SettingUI.activeSelf == false && StoreUI.activeSelf == false)
                {
                    Debug.Log("연등에 소원을 적고 연등을 설치합니다.");
                    LanternBoardUI.gameObject.SetActive(true);
                    BoardLanternTxt.text = ": " + Lantern.ToString();
                }
            }
        }
>>>>>>> Stashed changes
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

        else if (RankUI.activeSelf == true && UI.activeSelf == false)
        {
            RankUI.gameObject.SetActive(false);
            UI.gameObject.SetActive(true);
        }
<<<<<<< Updated upstream
=======

        else if (StoreUI.activeSelf == true)
        {
            StoreUI.gameObject.SetActive(false);
        }

        else if (LanternBoardUI.activeSelf == true)
        {
            LanternBoardUI.gameObject.SetActive(false);
        }
>>>>>>> Stashed changes
    }

    public void AudioControl(float sliderVal)
    {
        masterMixer.SetFloat("Music", Mathf.Log10(sliderVal) * 20);
    }

    public void Camera()
    {
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        string fileName = "DONGSIM-SCREENSHOT-" + timestamp + ".png";

        if (UI.activeSelf == true)
        {
            UI.gameObject.SetActive(false);
        }

        ScreenCapture.CaptureScreenshot(fileName);

        UI.gameObject.SetActive(true);
    }

    public void RankButtonClick()
    {
        if (UI.activeSelf == true)
        {
            RankUI.gameObject.SetActive(true);
            UI.gameObject.SetActive(false);
        }
    }
<<<<<<< Updated upstream
=======

    public void LanternBuy()
    {
        if (Coin >= 5)
        {
            Debug.Log("연등 1개를 구매하였습니다.");
            Coin = Coin - 5;
            Lantern = Lantern + 1;
            CoinTxt.text = ": " + Coin.ToString();
            LanternTxt.text = ": " + Lantern.ToString();
        }
        else
        {
            Debug.Log("코인이 부족하여 연등을 구매할 수 없습니다.");
        }
    }

    public void WriteWish()
    {
        if (Lantern > 0)
        {   
            Wish = WishInput.text;
            if (Wish.Length > 0)
            {
                Debug.Log("연등을 설치하였습니다.");
                // 소원 작성
                Lantern = Lantern - 1;
                BoardLanternTxt.text = ": " + Lantern.ToString();
                ScrollViewTxt.text = ScrollViewTxt.text + "\n" + WishInput.text;
                WishInput.text = "";

                // 연등 설치
                int x = Random.Range(0, 30);
                int z = Random.Range(0, 5);
                GameObject LanternClone = Instantiate(LanternOriginal, new Vector3(x, 5, z), Quaternion.identity);
                LanternClone.name = "LanternClone" + (LanternNum + 1);
                LanternNum = LanternNum + 1;
            }
            else
            {
                Debug.Log("소원을 입력하세요.");
            }
        }
        else
        {
            Debug.Log("연등이 없어서 설치할 수 없습니다. 상점에서 연등을 구매하세요.");
        }
    }
>>>>>>> Stashed changes
}
