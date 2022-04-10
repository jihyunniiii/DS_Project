using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLogo : MonoBehaviour
{
  
    // Start is called before the first frame update
    void Start()
    {
        Invoke("delay", 3f);
    }

    // Update is called once per frame
    public void delay()
    {
        GameObject.FindWithTag("FadeController").GetComponent<FadeInOut>().FadeToNext();
    }
    
}
