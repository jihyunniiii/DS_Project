using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class charactertemp : MonoBehaviour
{
    private enum ControlMode
    {
        Tank,
        Direct
    }
    /*[SerializeField]
    private float walkSpeed = 1f;*/
    [SerializeField]
    private Vector2 defaultPositionRange = new Vector2(-4, 4);

    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private float turnSpeed = 200;
    [SerializeField] private float jumpForce = 4;

    [SerializeField] private Animator animator = null;
    [SerializeField] private Rigidbody rigidBody = null;

    [SerializeField] private ControlMode controlMode = ControlMode.Direct;
    // Start is called before the first frame update
    private float currentV = 0;
    private float currentH = 0;

    private readonly float interpolation = 10;
    private readonly float walkScale = 0.33f;
    private readonly float backwardsWalkScale = 0.16f;
    private readonly float backwardRunScale = 0.66f;

    private bool wasGrounded;
    private Vector3 currentDirection = Vector3.zero;

    private float jumpTimeStamp = 0;
    private float minJumpInterval = 0.25f;
    private bool jumpInput = false;

    private bool isGrounded;
    private Vector3 ServerVector3 = Vector3.zero;
    Quaternion Serverquaternion;
    private List<Collider> collisions = new List<Collider>();

    private void Awake()
    {
        if (!animator) { gameObject.GetComponent<Animator>(); }
        if (!rigidBody) { gameObject.GetComponent<Animator>(); }
    }
    void Start()
    {
        //transform.position = new Vector3(Random.Range(defaultPositionRange.x, defaultPositionRange.y), 21, Random.Range(defaultPositionRange.x, defaultPositionRange.y));
    }
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!collisions.Contains(collision.collider))
                {
                    collisions.Add(collision.collider);
                }
                isGrounded = true;
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            isGrounded = true;
            if (!collisions.Contains(collision.collider))
            {
                collisions.Add(collision.collider);
            }
        }
        else
        {

            if (collisions.Contains(collision.collider))
            {
                collisions.Remove(collision.collider);
            }
            if (collisions.Count == 0) { isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {

        if (collisions.Contains(collision.collider))
        {
            collisions.Remove(collision.collider);
        }

        if (collisions.Count >= 0) { isGrounded = false; }
    }

    private void Update()
    {
        if (!jumpInput && Input.GetKey(KeyCode.Space))
        {
            jumpInput = true;
        }
    }

    private void FixedUpdate()
    {
        animator.SetBool("Grounded", isGrounded);
        
        
            switch (controlMode)
            {
                case ControlMode.Direct:
                    DirectUpdate();
                    break;

                case ControlMode.Tank:
                    TankUpdate();
                    break;

                default:
                    Debug.LogError("Unsupported state");
                    break;
            }
        

        wasGrounded = isGrounded;
        jumpInput = false;
    }
    private void UpdateServer()
    {
        transform.rotation = Serverquaternion;
        transform.position += ServerVector3;
    }

    private void TankUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        bool walk = Input.GetKey(KeyCode.LeftShift);

        if (v < 0)
        {
            if (walk) { v *= backwardsWalkScale; }
            else { v *= backwardRunScale; }
        }
        else if (walk)
        {
            v *= walkScale;
        }

        currentV = Mathf.Lerp(currentV, v, Time.deltaTime * interpolation);
        currentH = Mathf.Lerp(currentH, h, Time.deltaTime * interpolation);

        transform.position += transform.forward * currentV * moveSpeed * Time.deltaTime;
        transform.Rotate(0, currentH * turnSpeed * Time.deltaTime, 0);

        animator.SetFloat("MoveSpeed", currentV);

        JumpingAndLanding();
    }

    private void DirectUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Transform camera = Camera.main.transform;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            v *= walkScale;
            h *= walkScale;
        }

        currentV = Mathf.Lerp(currentV, v, Time.deltaTime * interpolation);
        currentH = Mathf.Lerp(currentH, h, Time.deltaTime * interpolation);

        Vector3 direction = camera.forward * currentV + camera.right * currentH;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if (direction != Vector3.zero)
        {
            currentDirection = Vector3.Slerp(currentDirection, direction, Time.deltaTime * interpolation);

             transform.rotation = Quaternion.LookRotation(currentDirection);
            transform.position += currentDirection * moveSpeed * Time.deltaTime;
           // UpdateClientPositionServerRpc(Quaternion.LookRotation(currentDirection), currentDirection * moveSpeed * Time.deltaTime);
            animator.SetFloat("MoveSpeed", direction.magnitude);
        }

        JumpingAndLanding();
    }

    /*[ServerRpc]
    private void UpdateClientPositionServerRpc(Quaternion quaternion, Vector3 vector3)
    {
        Serverquaternion = quaternion;
        ServerVector3 = vector3;
    }*/


    private void JumpingAndLanding()
    {
        bool jumpCooldownOver = (Time.time - jumpTimeStamp) >= minJumpInterval;

        if (isGrounded && rigidBody.velocity.y < -1)
        {
            isGrounded = false;
        }

        if (jumpCooldownOver && isGrounded && jumpInput)
        {
            jumpTimeStamp = Time.time;
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (!wasGrounded && isGrounded)
        {
            animator.SetTrigger("Land");
        }

        if (!isGrounded && wasGrounded)
        {
            animator.SetTrigger("Jump");
        }
    }
}
