using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DilmerGames.Core.Singletons;
using Unity.Netcode;
public class PlayerManager : NetworkSingleton<PlayerManager> 
{
    
    private NetworkVariable<int> playersInGame = new NetworkVariable<int>();

    public int PlayersInGame
    {
        get { 
            return playersInGame.Value; 
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) => {
            if (IsServer) {
                Logger.Instance.LogInfo($"{id} just connected....");
                playersInGame.Value++;
            }
        };
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) => {
            if (IsServer)
            {
                Logger.Instance.LogInfo($"{id} just disconnected....");
                playersInGame.Value--;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
