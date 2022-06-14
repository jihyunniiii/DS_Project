using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTileMapTUT : MonoBehaviour
{
    public GameObject[] hexTilePrefab;
    public Transform holder;

    [SerializeField] int mapWidth = 25;
    [SerializeField] int mapHeight = 25;

    float tileXOffset = 4.0f;
    float tileZOffset = 3.48f;

    // Start is called before the first frame update
    void Start()
    {
        CreatHexTileMap();
        Invoke("settingFinish", 1f);
    }

    void CreatHexTileMap() {
        float mapXmin = -mapWidth / 2;
        float mapXmax = mapWidth / 2;

        float mapZmin = -mapHeight / 2;
        float mapZmax = mapHeight / 2;
        for (int i = 0; i < hexTilePrefab.Length; i++)
        {
            for (float x = mapXmin; x < mapXmax; x++)
            {
                for (float z = mapZmin; z < mapZmax; z++)
                {
                    GameObject TempGo = Instantiate(hexTilePrefab[i]);
                    Vector3 pos;

                    if (z % 2 == 0)
                    {
                        pos = new Vector3(x * tileXOffset, 0, z * tileZOffset);
                    }
                    else
                    {
                        pos = new Vector3(x * tileXOffset + tileXOffset / 2, 0, z * tileZOffset);
                    }
                    StartCoroutine(SetTileInfo(TempGo, x, z, pos));
                }
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
        if (other.gameObject.tag == "Hex1") {
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Hex2") {
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Hex3")
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Hex4")
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Hex5")
        {
            Destroy(other.gameObject);
        }
    }

    private void settingFinish() {
        gameObject.GetComponent<SphereCollider>().enabled = false;
        GameObject[] temp1 = GameObject.FindGameObjectsWithTag("Hex1");
        GameObject[] temp2 = GameObject.FindGameObjectsWithTag("Hex2");
        GameObject[] temp3 = GameObject.FindGameObjectsWithTag("Hex3");
        GameObject[] temp4 = GameObject.FindGameObjectsWithTag("Hex4");
        GameObject[] temp5 = GameObject.FindGameObjectsWithTag("Hex5");
        for (int x = 0; x < temp1.Length; x++) {
            temp1[x].GetComponent<MeshCollider>().isTrigger = false;
            temp1[x].transform.Translate(new Vector3(0, 500, 0), Space.Self);
        }
        for (int x = 0; x < temp2.Length; x++)
        {
            temp2[x].GetComponent<MeshCollider>().isTrigger = false;
            temp2[x].transform.Translate(new Vector3(0, 540, 0), Space.Self);
        }
        for (int x = 0; x < temp3.Length; x++)
        {
            temp3[x].GetComponent<MeshCollider>().isTrigger = false;
            temp3[x].transform.Translate(new Vector3(0, 580, 0), Space.Self);
        }
        for (int x = 0; x < temp4.Length; x++)
        {
            temp4[x].GetComponent<MeshCollider>().isTrigger = false;
            temp4[x].transform.Translate(new Vector3(0, 620, 0), Space.Self);
        }
        for (int x = 0; x < temp5.Length; x++)
        {
            temp5[x].GetComponent<MeshCollider>().isTrigger = false;
            temp5[x].transform.Translate(new Vector3(0, 660, 0), Space.Self);
        }
    }
}
/*
 캐릭터 오브젝트 - 상대좌표 x,z그대로 사용 y는 발부분 
vector3 pos = new vector3(x,y,z);
 temp.trasfrom. =pos;

 */