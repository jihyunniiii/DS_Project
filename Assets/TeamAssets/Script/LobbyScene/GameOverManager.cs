using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BackEnd;
using System;

public class GameOverManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameOverManager instance;
    public GameObject UI;
    public Transform Camera_transform;
    private bool GameStarting = false;
    private float time;
    public TextMeshProUGUI text_timer;
    string inDate;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }
    // Start is called before the first frame update
    public static GameOverManager GetInstance()
    {
        return instance;
    }
    // Update is called once per frame
    public void SpawnCharacter(Transform CHtransform)
    {
        GameStarting = false;
        UI.SetActive(false);
        Where where = new Where();
        var bro = Backend.GameData.GetMyData("user", where);

        if (bro.Rows().Count > 0)
        {
            inDate = bro.Rows()[0]["inDate"]["S"].ToString();
        }
        Param param = new Param();
        param.AddCalculation("Money", GameInfoOperator.addition, int.Parse(text_timer.text) * 2);
        Backend.GameData.UpdateWithCalculation("user", inDate, param);
        lobby.GetInstance().CoinUpdate();
        CHtransform.position = new Vector3(0, 45, 0);
        Camera_transform.position = new Vector3(0, 45, 0);

    }
    public void GameStart(Transform Chtrnasform)
    {
        Debug.Log("d");
        Chtrnasform.position = new Vector3(0, 680, 0);
        Camera_transform.position = new Vector3(0, 680, 0);
        GameStarting = true;
        UI.SetActive(true);
        time = 0.0f;
    }

    private void Update()
    {
        if (GameStarting)
        {
            time += Time.deltaTime;
            text_timer.text = Mathf.Ceil(time).ToString();
        }
    }
}
