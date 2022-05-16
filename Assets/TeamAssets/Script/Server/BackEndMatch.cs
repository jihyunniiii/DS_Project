using System;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;
using System.Linq;


public partial class BackEndMatch : MonoBehaviour
{
    public class MatchInfo
    {
        public string title;                // 매칭 명
        public string inDate;               // 매칭 inDate (UUID)
        public MatchType matchType;         // 매치 타입
        public MatchModeType matchModeType; // 매치 모드 타입
        public string headCount;            // 매칭 인원
        public bool isSandBoxEnable;        // 샌드박스 모드 (AI매칭)
    }
    private static BackEndMatch instance = null; // 인스턴스

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
}
