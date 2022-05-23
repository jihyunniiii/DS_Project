using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class CharacterSet : NetworkBehaviour
{
    public enum PlayerStates { 
        Ground,
        Land,
        Move,
        Jump
    }
    /*[SerializeField]
    private float walkSpeed = 1f;*/
    [SerializeField]
    private Vector2 defaultPositionRange = new Vector2(-4, 4);

    [SerializeField] private float moveSpeed = 2;

    [SerializeField] private float jumpForce = 4;

    [SerializeField] private Animator animator = null;
    [SerializeField] private Rigidbody rigidBody = null;
    Vector3 direction;

    // Start is called before the first frame update
    private float currentV = 0;
    private float currentH = 0;

    private readonly float interpolation = 10;
    private readonly float walkScale = 0.33f;


    private bool wasGrounded;
    private Vector3 currentDirection = Vector3.zero;

    private float jumpTimeStamp = 0;
    private float minJumpInterval = 0.25f;
    private bool jumpInput = false;

    private bool isGrounded;

    //private Vector3 ServerJump = Vector3.zero;
    private List<Collider> collisions = new List<Collider>();
    [SerializeField]
    private NetworkVariable<Vector3> networkPositionDirection = new NetworkVariable<Vector3>();

    [SerializeField]
    private NetworkVariable<Quaternion> networkRotationDirection = new NetworkVariable<Quaternion>();

    [SerializeField]
    private NetworkVariable<PlayerStates> networkPlayerState = new NetworkVariable<PlayerStates>();

    private void Awake()
    {
        if (!animator) { gameObject.GetComponent<Animator>(); }
        if (!rigidBody) { gameObject.GetComponent<Animator>(); }
    }
    void Start()
    {
        transform.position = new Vector3(Random.Range(defaultPositionRange.x, defaultPositionRange.y), 0, Random.Range(defaultPositionRange.x, defaultPositionRange.y));
       
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

    }

    private void FixedUpdate()
    {
        
        if (IsClient && IsOwner)
        {
            UpdatePlayerStateServerRpc(PlayerStates.Ground);
            if (!jumpInput && Input.GetKey(KeyCode.Space))
            {
                Debug.Log("½ÇÇà");
                jumpInput = true;
            }
            DirectUpdate();

            wasGrounded = isGrounded;
            jumpInput = false;
        }
        ClientMovve();
        ClientVisual();
    }

    private void ClientVisual()
    {
        if (networkPlayerState.Value == PlayerStates.Ground)
            animator.SetBool("Grounded", isGrounded);
        else if (networkPlayerState.Value == PlayerStates.Land)
            animator.SetTrigger("Land");
        else if (networkPlayerState.Value == PlayerStates.Jump)
            animator.SetTrigger("Jump");
        else if (networkPlayerState.Value == PlayerStates.Move)
            animator.SetFloat("MoveSpeed", direction.magnitude);
    }

    private void ClientMovve()
    {
        transform.rotation = networkRotationDirection.Value;
        transform.position += networkPositionDirection.Value;

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

        direction = camera.forward * currentV + camera.right * currentH;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if (direction != Vector3.zero)
        {
            currentDirection = Vector3.Slerp(currentDirection, direction, Time.deltaTime * interpolation);

            // transform.rotation = Quaternion.LookRotation(currentDirection);
            //transform.position += currentDirection * moveSpeed * Time.deltaTime;
            UpdateClientPositionServerRpc(Quaternion.LookRotation(currentDirection), currentDirection * moveSpeed * Time.deltaTime);
            UpdatePlayerStateServerRpc(PlayerStates.Move);
        }

        JumpingAndLanding();

    }

    [ServerRpc]
    private void UpdateClientPositionServerRpc(Quaternion quaternion, Vector3 vector3)
    {
        networkRotationDirection.Value = quaternion;
        networkPositionDirection.Value = vector3;
    }
    [ServerRpc]
    public void UpdatePlayerStateServerRpc(PlayerStates states)
    {
        networkPlayerState.Value = states;
    }
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
            UpdatePlayerStateServerRpc(PlayerStates.Land);
        }

        if (!isGrounded && wasGrounded)
        {
            UpdatePlayerStateServerRpc(PlayerStates.Jump);
        }
    }
}
