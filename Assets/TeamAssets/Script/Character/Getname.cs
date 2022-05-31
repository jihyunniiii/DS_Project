using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using TMPro;
using Unity.Netcode;
public class Getname : NetworkBehaviour
{
    private NetworkVariable<NetworkString> playersName = new NetworkVariable<NetworkString>();
    public TextMeshProUGUI localPlayerOverlay;
    private bool overlaySet = false;
    //private bool isonetimeset = false;
    // Start is called before the first frame update
    
    public override void OnNetworkSpawn()
    {
       
    }
    public void SetOverlay() { 
        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        localPlayerOverlay.text = bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();
    }

    private void Update()
    {
        if (!overlaySet) { 
            SetOverlay();
            overlaySet = true;
        }
    }
}
