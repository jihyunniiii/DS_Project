using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTileMapTUT : MonoBehaviour
{
    public GameObject hexTilePrefab;
    public Transform holder;
    public int y = 0;
    
    int count = 0;

    [SerializeField] int mapWidth = 25;
    [SerializeField] int mapHeight = 25;

    float tileXOffset = 1.0f;
    float tileZOffset = 0.87f;

    // Start is called before the first frame update
    void Start()
    {
        CreatHexTileMap();
        Invoke("settingFinish", 0.1f);
    }

    void CreatHexTileMap() {
        float mapXmin = -mapWidth / 2;
        float mapXmax = mapWidth / 2;

        float mapZmin = -mapHeight / 2;
        float mapZmax = mapHeight / 2;

        for (float x = mapXmin; x < mapXmax; x++)
        {
            for (float z = mapZmin; z < mapZmax; z++)
            {
                GameObject TempGo = Instantiate(hexTilePrefab);
                Vector3 pos;

                if (z % 2 == 0)
                {
                    pos = new Vector3(x * tileXOffset, y, z * tileZOffset);
                }
                else {
                    pos = new Vector3(x * tileXOffset + tileXOffset / 2, y, z * tileZOffset);
                }
                StartCoroutine(SetTileInfo(TempGo, x, z, pos));
                count++;
            }
        }
    }



    IEnumerator SetTileInfo(GameObject tempGo, float x, float z, Vector3 pos)
    {
        yield return new WaitForSeconds(0.00001f);
        tempGo.transform.parent = holder;
        tempGo.name = x.ToString() + "," + z.ToString();
        tempGo.transform.position = pos;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Hex") {
            Destroy(other.gameObject);
            count--;
        }
    }

    private void settingFinish() {
        gameObject.GetComponent<SphereCollider>().enabled = false;
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Hex");
        for (int x = 0; x < count; x++) {
            temp[x].GetComponent<MeshCollider>().isTrigger = false; 
        }
       
    }
}