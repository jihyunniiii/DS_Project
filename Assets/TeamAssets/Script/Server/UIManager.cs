using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using BackEnd;

public class UIManager : MonoBehaviour
{
   // [SerializeField]
    private Button startServerButton;
    [SerializeField]
    private Button startHostButton;
    [SerializeField]
    private Button startClientButton;
    [SerializeField]
    private TextMeshProUGUI playersInGameText;
    public GameObject HostButton;
    public GameObject ClientButton;
    public GameObject JoingCanvas;
    public GameObject Camera;
    public GameObject Camera2;
    public GameObject ServerUI;
    string Hostname;
    private bool isHost = false;
    // Start is called before the first frame update
    void Start()
    {
        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        Hostname = bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();
        if (Hostname.Equals("Bedford"))
            isHost = true;
        if (isHost)
        {
            HostButton.SetActive(true);
        }
        else { 
            ClientButton.SetActive(true);
        }

        startHostButton.onClick.AddListener(() => {
            if (NetworkManager.Singleton.StartHost())
            {
                JoingCanvas.SetActive(false);
                Camera.SetActive(false);
                ServerUI.SetActive(true);
                Camera2.SetActive(true);
                Logger.Instance.LogInfo("Host started....");
                
            }
            else {
                Logger.Instance.LogInfo("Host could not be started....");
            }
        });
        /*startServerButton.onClick.AddListener(() => {
            if (NetworkManager.Singleton.StartServer())
            {
                Logger.Instance.LogInfo("Server started....");
                JoingCanvas.SetActive(false);
                Camera.SetActive(false);
                ServerUI.SetActive(true);
                Camera2.SetActive(true);
            }
            else
            {
                Logger.Instance.LogInfo("Server could not be started....");
            }
        });*/
        startClientButton.onClick.AddListener(() => {
            if (NetworkManager.Singleton.StartClient())
            {
                JoingCanvas.SetActive(false);
                Camera.SetActive(false);
                ServerUI.SetActive(true);
                Camera2.SetActive(true);
                Logger.Instance.LogInfo("Client started....");
            }
            else
            {
                Logger.Instance.LogInfo("Client could not be started....");
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        playersInGameText.text = $"Players in game : {PlayerManager.Instance.PlayersInGame}";
    }
}
