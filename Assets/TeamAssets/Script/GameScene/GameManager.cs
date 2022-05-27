using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject ExitPannel;
    public string thisScene;

    // Use this for initialization
    void Start()
    {
        thisScene = SceneManager.GetActiveScene().name;  // 실행 중인 씬의 이름을 가져온다
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))   // Esc키를 누르면
        {
            //Time.timeScale = 0f;
            ExitPannel.SetActive(true);
        }
    }

    public void 계속하기()
    {
        Time.timeScale = 1f;
        //SceneManager.LoadSceneAsync("MainGame");
        ExitPannel.SetActive(false);
    }

    public void 게임종료()

    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();   // 종료한다
#endif
    }
    public void GotoLobby()
    {
        SceneManager.LoadScene("MainLobby");
    }

}