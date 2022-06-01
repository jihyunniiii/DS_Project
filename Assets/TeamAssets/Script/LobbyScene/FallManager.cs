using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallManager : MonoBehaviour
{
    private static FallManager instance;
    public Transform Camera_transform;
    private void Awake()
    {
        if (instance != null) { 
            Destroy(instance);
        }
        instance = this;
    }
    // Start is called before the first frame update
    public static FallManager GetInstance() {
        return instance;
    }
    // Update is called once per frame
    public void SpawnCharacter(Transform CHtransform) {
        CHtransform.position = new Vector3(0, 45, 0);
        Camera_transform.position = new Vector3(0, 45, 0);
    }
}
