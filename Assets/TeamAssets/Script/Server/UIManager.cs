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
    public GameObject ClientServerJoincode;
    public InputField joinCodeInput;
    public GameObject loUI;
    public GameObject CurUI;
    public GameObject LoadingUI; 
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
            ClientServerJoincode.SetActive(true);
        }

        startHostButton.onClick.AddListener(async () => {
            Where where = new Where();
            var bros = Backend.GameData.GetMyData("user", where);
            string inDate = bros.Rows()[0]["inDate"]["S"].ToString();
            Param param = new Param();
            param.Add("ServerJoin", "O");
            Backend.GameData.Update("user", inDate, param);
            LoadingUI.SetActive(true);
            if (RelayManager.Instance.IsRelayEnabled)
                await RelayManager.Instance.SetupRelay();

            if (NetworkManager.Singleton.StartHost())
            {
                CurUI.SetActive(true);
                loUI.SetActive(true);
                JoingCanvas.SetActive(false);
                Camera.SetActive(false);
                ServerUI.SetActive(true);
                Camera2.SetActive(true);
               // Logger.Instance.LogInfo("Host started....");
                LoadingUI.SetActive(false);
            }
            else {
                LoadingUI.SetActive(false);
                //Logger.Instance.LogInfo("Host could not be started....");
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
        startClientButton.onClick.AddListener(async () => {
            Where where = new Where();
            var bross = Backend.GameData.GetMyData("user", where);
            string inDate = bross.Rows()[0]["inDate"]["S"].ToString();
            Param param = new Param();
            param.Add("ServerJoin", "O");
            Backend.GameData.Update("user", inDate, param);
            LoadingUI.SetActive(true);
            string[] select = { "ServerPort" };
            var bros = Backend.GameData.Get("user", "2022-05-30T13:23:21.416Z", select);
            joinCodeInput.text = bros.GetReturnValuetoJSON()["row"][0]["S"].ToString();
            
            if (RelayManager.Instance.IsRelayEnabled && !string.IsNullOrEmpty(joinCodeInput.text))
                await RelayManager.Instance.JoinRelay(joinCodeInput.text);

            if (NetworkManager.Singleton.StartClient())
            {
                CurUI.SetActive(true);
                LoadingUI.SetActive(false);
                loUI.SetActive(true);
                JoingCanvas.SetActive(false);
                Camera.SetActive(false);
                ServerUI.SetActive(true);
                Camera2.SetActive(true);
                ClientServerJoincode.SetActive(false);
               // Logger.Instance.LogInfo("Client started....");
            }
            else
            {
                LoadingUI.SetActive(false);
                // Logger.Instance.LogInfo("Client could not be started....");
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        playersInGameText.text = $"Players in game : {PlayerManager.Instance.PlayersInGame}";
    }

    private void OnApplicationQuit()
    {
        if (Hostname.Equals("Bedford")) {
            Param param = new Param();
            string temp = "";
            param.Add("ServerPort", temp);
            Backend.GameData.Update("user", "2022-05-30T13:23:21.416Z", param);
        }
    }
}
