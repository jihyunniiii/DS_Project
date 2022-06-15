using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DilmerGames.Core.Singletons;
public class CameraMove : Singleton<CameraMove>
{
    public Transform objectTofollow;
    public float followSpeed = 10f;
    public float sensitivity = 100f;
    public float clampAngle = 70f;
    public Vector3 temp = new Vector3(0, 1, 0);
    private float rotX;
    private float rotY;

    public Transform realCamera;
    public Vector3 dirNorMalized;
    public Vector3 finalDir;
    public float minDistance;
    public float maxDistance;
    public float finalDistance;
    public float smoothness;
    bool chatting = false;
    private void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNorMalized = realCamera.localPosition.normalized;
        finalDistance = realCamera.localPosition.magnitude;
        
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
    public void FollowPlayer(Transform transform) {
        objectTofollow = transform;
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F2))
        {
            if (chatting == true)
            {
                chatting = false;
            }
            else if (chatting == false)
            {
                chatting = true;
            }
        }
        if (chatting)
            return;
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }
    private void LateUpdate()
    {

        transform.position = Vector3.MoveTowards(transform.position, objectTofollow.position + temp, followSpeed * Time.deltaTime);

        finalDir = transform.TransformPoint(dirNorMalized * maxDistance);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, finalDir, out hit))
        {
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else {
            finalDistance = maxDistance;
        }
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNorMalized * finalDistance, Time.deltaTime* smoothness);
    }
}
