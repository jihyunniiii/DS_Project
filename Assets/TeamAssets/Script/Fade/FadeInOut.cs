using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInOut : MonoBehaviour
{
    public Animator animator; //애니메이션 객체
    public string NextScene;
    // Start is called before the first frame update

    // Update is called once per frame

    public void FadeToNext() {
        animator.SetTrigger("FadeOut");
    }

    public void FadeComplete() {
        SceneManager.LoadScene(NextScene);
    }
}
