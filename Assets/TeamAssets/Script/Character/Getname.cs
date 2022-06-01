using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using TMPro;
using Unity.Netcode;
public class Getname : NetworkBehaviour
{
    private NetworkVariable<NetworkString> playersName = new NetworkVariable<NetworkString>();
    
    private bool overlaySet = false;
    private bool isSet = false;
    //private bool isonetimeset = false;
    // Start is called before the first frame update
    
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            string[] select = { "Nickname", "ServerJoin" };
            Where where = new Where();
            where.Equal("ServerJoin", "O");
            var bro = Backend.GameData.Get("user", where, select);
            string inDate = bro.Rows()[0]["inDate"]["S"].ToString();
            Debug.Log(inDate);
            string name = bro.Rows()[0]["Nickname"]["S"].ToString();
            Debug.Log(name);
            
            playersName.Value = name;
            Param param = new Param();
            param.Add("ServerJoin", "X");
            Backend.GameData.Update("user", inDate, param);
            //playersName.Value = $"Player : {OwnerClientId}";
        }
    }
    public void SetOverlay() {
        var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        localPlayerOverlay.text = playersName.Value;
    }

    private void Update()
    {
        if (!overlaySet && !string.IsNullOrEmpty(playersName.Value)) { 
            SetOverlay();
            overlaySet = true;
        }
    }
}
