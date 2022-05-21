using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using TMPro;
public class Getname : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        textMeshPro.text = bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();
    }
}
