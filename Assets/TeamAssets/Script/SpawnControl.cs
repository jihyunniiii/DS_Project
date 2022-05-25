using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DilmerGames.Core.Singletons;
using Unity.Netcode;
using BackEnd;

public class SpawnControl : NetworkSingleton<SpawnControl>
{
    [SerializeField]
    private GameObject objectPrefabLantern;

    Transform SetCharacter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void SpawnLantern(Transform character)
    {
        
        SetCharacter = character;
        if (IsServer)
        {
            Spawn();
        }
        else{
            SpawnServerRpc();
        }

    }
    public void Spawn() {
        Debug.Log("t");
        Vector3 temp;
        Vector3 t = new Vector3(0, 1.5f, 0);
        temp = SetCharacter.position - t;
        GameObject go = Instantiate(objectPrefabLantern, temp, Quaternion.identity);
        go.GetComponent<NetworkObject>().Spawn();
    }
    [ServerRpc(RequireOwnership = false)]
    public void SpawnServerRpc() {
        Debug.Log("C");
        Spawn();
    }
}
