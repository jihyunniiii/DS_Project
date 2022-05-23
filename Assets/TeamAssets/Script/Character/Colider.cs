using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colider : MonoBehaviour
{
    PlayerControl playerControls = GameObject.Find("PlayerArmatureNetworkAuthorative").GetComponent<PlayerControl>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        playerControls.IsJumping = false;
    }*/
}
