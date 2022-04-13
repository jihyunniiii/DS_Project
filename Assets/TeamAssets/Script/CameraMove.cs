using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public float distance = 6f;
    public float height = 4f;

    private Transform tr;

    private void Start()
    {
        tr = GetComponent<Transform>();
    }
    private void Update()
    {
        tr.position = target.position - (1 * Vector3.forward * distance) + (Vector3.up * height);
        tr.LookAt(target);
    }
}
