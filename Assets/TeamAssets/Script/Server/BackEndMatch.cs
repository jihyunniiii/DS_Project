using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using UnityEngine.SceneManagement;

public partial class BackEndMatch : MonoBehaviour
{
    private static BackEndMatch instance = null; // 인스턴스
    public Text nickName;
    string mynickName;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        OnBackendUserInfo();
        SetNickName();
    }
    public static BackEndMatch GetInstance()
    {
        if (!instance)
        {
            //Debug.LogError("BackEndMatchManager 인스턴스가 존재하지 않습니다.");
            return null;
        }

        return instance;
    }
    // Update is called once per frame
    void Update()
    {
        Backend.Match.Poll();
    }

    void OnBackendUserInfo() {
        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        mynickName = bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();
    }

    void SetNickName() {
        nickName.text = mynickName;
    }

    public void GoLobby() {
        Backend.BMember.Logout();
        SceneManager.LoadScene("Title_DS");
    }
}
