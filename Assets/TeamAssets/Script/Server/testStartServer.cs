using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
public class testStartServer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var bro = Backend.Initialize(true);
        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공");
        }
        else {
            Debug.Log("초기화 실패!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Backend.AsyncPoll();
    }
}
