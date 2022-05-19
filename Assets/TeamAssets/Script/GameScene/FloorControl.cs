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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(CoStartSimpleExplosion(true));
        }
    }
    IEnumerator CoStartSimpleExplosion(bool bDestroy = true)
    {

        GameObject explosionObj = newBlockConfig.GetExplosionObject();
        explosionObj.SetActive(true);
        explosionObj.transform.position = this.transform.position;

        yield return new WaitForSeconds(0.1f);

        //2. 블럭 GameObject 객체 삭제 or make size zero
        if (bDestroy)
            Destroy(gameObject,0.6f);
        else
        {
            Debug.Assert(false, "Unknown Action : GameObject No Destory After Particle");
        }
    }
}
