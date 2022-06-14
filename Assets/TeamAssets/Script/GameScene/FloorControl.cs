using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorControl : MonoBehaviour
{
    public BlockConfig newBlockConfig;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject explosionObj = newBlockConfig.GetExplosionObject();
            explosionObj.SetActive(true);
            explosionObj.transform.position = gameObject.transform.position;
            Destroy(gameObject, 0.5f);
        }
    }*/
}
