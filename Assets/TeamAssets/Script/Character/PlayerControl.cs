using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PlayerControl : NetworkBehaviour
{
    [SerializeField]
    private float walkSpeed = 1f;
    [SerializeField]
    private Vector2 defaultPositionRange = new Vector2(-4, 4);
    [SerializeField]
    private NetworkVariable<float> forwardBackPosition = new NetworkVariable<float> ();
    [SerializeField]
    private NetworkVariable<float> leftRightPosition = new NetworkVariable<float> ();

    private float oldForwardBackPosition;
    private float oldLeftRightPosition;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.Range(defaultPositionRange.x, defaultPositionRange.y), 0, Random.Range(defaultPositionRange.x, defaultPositionRange.y));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
