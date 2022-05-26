using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmp : MonoBehaviour
{

    public GameObject wordUI_0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setset()
    {
        Time.timeScale = 1f;
        Debug.Log("확인 완료");
        wordUI_0.SetActive(false);
    }
}
