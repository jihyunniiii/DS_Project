using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    string networkState; // 네트워크 상태를 확인 하기위한 문자열

    void Start() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster() =>
        PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby() =>
        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions { MaxPlayers = 5 }, null);

    // Update is called once per frame
    void Update()
    {
        string curNetworkState = PhotonNetwork.NetworkClientState.ToString();
        if (networkState != curNetworkState) {
            networkState = curNetworkState;
            print(networkState);
        }
    }
}
