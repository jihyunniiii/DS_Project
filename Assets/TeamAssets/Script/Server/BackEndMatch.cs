using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using BackEnd.Tcp;
using System.Linq;
using LitJson;
using UnityEngine.SceneManagement;

using static BackEnd.SendQueue;


public partial class BackEndMatch : MonoBehaviour
{
    private string inGameRoomToken = string.Empty;  // 게임 룸 토큰 (인게임 접속 토큰)
    public SessionId hostSession { get; private set; }  // 호스트 세션
    public bool isSandBoxGame { get; private set; } = false;

    private static BackEndMatch instance = null; // 인스턴스
    public GameObject loadingUI;
    public GameObject matchUI;
    public GameObject matchbutton;
    public GameObject matchbuttoncancel;
    public Text nickName;
    string mynickName;
    string myIndate;
    string indate;


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
        joinmatchserver();
        OnBackendUserInfo();
        SetNickName();
        Invoke("createMatch", 1f);
        checkBackEnd();
        
    }

    // Update is called once per frame
    void Update()
    {
        Backend.Match.Poll();
    }

    void joinmatchserver() {
        ErrorInfo errorInfo;
        Backend.Match.JoinMatchMakingServer(out errorInfo);
        Backend.Match.OnJoinMatchMakingServer = (JoinChannelEventArgs args) =>
        {
            Debug.Log("매칭 서버 접속 완료");
        };
    }

    void leaveMatch() {
        Backend.Match.LeaveMatchMakingServer();
        Backend.Match.OnLeaveMatchMakingServer = (LeaveChannelEventArgs args) =>
        {
            Debug.Log("매치 서버 종료, 로비로 돌아갑니다.");
        };
    }
    void createMatch() {
        Backend.Match.CreateMatchRoom();
        Backend.Match.OnMatchMakingRoomCreate = (MatchMakingInteractionEventArgs args) =>
        {
            Debug.Log("대기방이 생성되었습니다.");
        };
    }
    void OnBackendUserInfo() {
        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        mynickName = bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();
    }

    void SetNickName() {
        nickName.text = mynickName;
    }

    public void MacthingGame() {
        var bro = Backend.Match.GetMatchList();
        JsonData matchCardListJson = bro.FlattenRows();
        indate = matchCardListJson[0]["inDate"].ToString();
        Backend.Match.RequestMatchMaking(MatchType.Random,MatchModeType.Melee, indate);
    }

    public void MatcingGameCancel() {
        Backend.Match.CancelMatchMaking();
    }

    void  checkBackEnd() {
        Backend.Match.OnMatchMakingResponse = (MatchMakingResponseEventArgs args) => {
            // TODO
            switch (args.ErrInfo) {
                case ErrorCode.Success:
                    Debug.Log("매칭에 성공하였습니다.");
                    loadingUI.SetActive(false);
                    matchUI.SetActive(true);
                    break;
                case ErrorCode.Match_InProgress:
                    if (args.Reason == String.Empty) {
                        Debug.Log("매치 신청 성공");
                        loadingUI.SetActive(true);
                        matchbutton.SetActive(false);
                        matchbuttoncancel.SetActive(true);
                    }
                    break;
                case ErrorCode.Match_MatchMakingCanceled:
                    Debug.Log("매칭 신청 중단");
                    loadingUI.SetActive(false);
                    matchbutton.SetActive(true);
                    matchbuttoncancel.SetActive(false);
                    break;
            }
        };
    }

    public void GoLobby() {
        Backend.Match.LeaveMatchRoom();
        Backend.Match.LeaveMatchMakingServer();
        SceneManager.LoadScene("Title_DS");
    }
}
