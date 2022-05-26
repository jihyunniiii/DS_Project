using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandomLotus : MonoBehaviour
{

    public GameObject LotusPrefab;

    public int count; //생성되는 연꽃 개수
    public float TimeSpan; // 경과 시간
    public int CheckTime;// 특정 시간

    public GameObject[] L_count;//획득한 연꽃의 수를 세기 위한 배열 선언
    public int len = 0;//배열의 길이 확인


    public GameObject lotusUI;
    public TextMeshProUGUI lotusTxt;

    public GameObject wordUI;
    public TextMeshProUGUI wordTxt;
    public Button Wordbtn;

    string word = "타인이 너를 무엇으로 생각하는지는 중요치 않다. 스스로 생각하는 너 자신이 너의 모든 것이다.";
    public string[] TodayWord;//단어마다 저장하기 위한 배열
    public int letter;//랜덤으로 선택될 배열 index
    public bool A ;//button 활성화,비활성화 위함

    public int i = 1;

    void Start()
    {   
        
        A = false;
        TimeSpan = 0.0f;
        CheckTime = Random.Range(5, 10);

        Invoke("Spawn", CheckTime);

        InvokeRepeating("phrase", 10, 200);

    }

    // Update is called once per frame
    void Update()
    {
        

        TimeSpan += Time.deltaTime;
        DestroyLotus();

    }

    public void Spawn()
    {
        count = Random.Range(6, 8);


        if (TimeSpan > CheckTime)
        {
            for (int i = 0; i < count; i++)
            {
                SpawnPosition();
            }
            TimeSpan = 0.0f;

        }


    }

    public void SpawnPosition()
    {
        GameObject selectedPrefab = LotusPrefab;

        Vector3 spawnPos = new Vector3(Random.Range(-300, -170), 85, Random.Range(-60, 60));

        GameObject instance = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);


    }

    public void DestroyLotus()
    {
        L_count = GameObject.FindGameObjectsWithTag("lotus");//생성된 연꽃을 배열에 담음
        len = L_count.Length;//남아있는 연꽃의 개수 


        for (i = 1; i < count + 1; i++)
        {
            if (len == count - i)
            {
                lotusTxt.text = ": " + i.ToString();

                
                if (i != 0 && i % 5 == 0 && A==false )
                {
                    wordTxt.text = TodayWord[letter].ToString();
                    wordUI.SetActive(true);

                    A = true;
                }
            }
        }
    }

    public void phrase()
    {
        TodayWord = word.Split(new char[] { ' ' });
        letter = Random.Range(0, TodayWord.Length);
    }

    public void button()
    {
        if (wordUI.activeSelf == true)
        {
            wordUI.SetActive(false);
            A = false;

        }
    }
   
}



