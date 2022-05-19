using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnLotus : MonoBehaviour
{

    public GameObject LotusPrefab;

    public int count;//생성되는 연꽃 개수

    public float timeSpan;//경과 시간
    public int checkTime;//특정 시간

    private List<GameObject> lotus = new List<GameObject>();

    private void Start()
    {
        timeSpan = 0.0f; // 경과 시간 초기화 
        checkTime = Random.Range(5, 10);

        Invoke("Spawn", checkTime);

    }
    private void Update()
    {
        timeSpan += Time.deltaTime;

  
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {   
            Destroy();
            Debug.Log("연꽃 획득");
        }
    }*/


    private void Spawn()
    {

        count = Random.Range(2, 7);
        Debug.Log("checkTime: " + Mathf.Round(checkTime));
        Debug.Log("count: " + Mathf.Round(count));
        if (timeSpan > checkTime)
        {
            for (int i = 0; i < count; ++i)
            {
                SpawnPosition();
            }
            timeSpan = 0.0f;

        }
    }


    private void SpawnPosition()
    {

        GameObject selectedPrefab = LotusPrefab;

        Vector3 spawnPos = new Vector3(Random.Range(-45, 45), 25, Random.Range(-45, 45));

        GameObject instance = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        lotus.Add(instance);



    }
}