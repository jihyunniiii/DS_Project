using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    public GameObject LanternPrefeb;
    public GameObject lobbyscript;
    public Transform character;
    public int y = 1;

    public Camera getCamera;
    private RaycastHit hit;

    public GameObject PrintWishUI;
    

    // Start is called before the first frame update
    void Start()
    {
        y = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("연등 개수 : " + lobbyscript.GetComponent<lobby>().Lantern.ToString());
            if (lobbyscript.GetComponent<lobby>().Lantern > 0)
            {
                lobbyscript.GetComponent<lobby>().Lantern--;
                Vector3 temp;
                Vector3 t = new Vector3(0, y, 0);
                temp = character.position - t;
                GameObject LanternClone = Instantiate(LanternPrefeb, temp, Quaternion.identity);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = getCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "lantern")
                {
                    if (lobbyscript.GetComponent<lobby>().UI.activeSelf == false)
                    {
                        PrintWishUI.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void CheckButtonClick()
    {
        PrintWishUI.gameObject.SetActive(false);
    }
}
