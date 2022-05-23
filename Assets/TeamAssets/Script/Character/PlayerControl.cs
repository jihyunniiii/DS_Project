using System;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class PlayerControl : NetworkBehaviour
{
    #region Variables
    //basic variables about the movements
    [SerializeField]
    float speed = 2.0f;
    [SerializeField]
    float jumpHeight = 1.5f;
    [SerializeField]
    private float runSpeedOffset = 1.5f;
    //gravity and drag
    [SerializeField]
    float gravity = -29.81f;
    [SerializeField]
    private Vector3 drags;

    //to get CharacterController from the unity
    private CharacterController characterController;
    [SerializeField]
    private Vector2 defaultInitialPositionOnPlane = new Vector2(-4, 4);

    [SerializeField]
    private NetworkVariable<Vector3> networkPositionDirection = new NetworkVariable<Vector3>();
    [SerializeField]
    private NetworkVariable<Vector3> networkPositionDirectionY = new NetworkVariable<Vector3>();

    [SerializeField]
    private NetworkVariable<PlayerState> networkPlayerState = new NetworkVariable<PlayerState>();
    //to calculate
    private Vector3 calcVelocity = Vector3.zero;

    //movement(vector3)
    private Vector3 inputDirection = Vector3.zero;

    //prevent double jump
    [SerializeField]
    LayerMask groundLayerMask; // in the ground layer, player can jump only
    [SerializeField]
    float groundCheckDistance = 0.3f;
    private Animator animator;
    #endregion

    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        if (IsClient && IsOwner)
        {
            transform.position = new Vector3(UnityEngine.Random.Range(defaultInitialPositionOnPlane.x, defaultInitialPositionOnPlane.y), 0,
                   UnityEngine.Random.Range(defaultInitialPositionOnPlane.x, defaultInitialPositionOnPlane.y));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(IsClient && IsOwner)
            ClientMove();

        ClientMoveingtoserver();
        //Client_visual();
    }

    private void Client_visual()
    {
        
    }

    private void ClientMoveingtoserver()
    {
       // characterController.Move(networkPositionDirectionY.Value * Time.deltaTime);
        characterController.Move(networkPositionDirection.Value * Time.deltaTime * speed);
        if(networkPositionDirection.Value != Vector3.zero)
            transform.forward = networkPositionDirection.Value;
        characterController.Move(networkPositionDirectionY.Value * Time.deltaTime);
    }

    private void ClientMove() {
        bool isGrounded = characterController.isGrounded;

        //if player is on the ground, gravity = 0
        if (isGrounded && calcVelocity.y < 0)
        {
            calcVelocity.y = 0f;
        }

        //user input - basic movments
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (ActiveRunningActionKey() && move != Vector3.zero)
        {
            move = move * runSpeedOffset;
            //UpdatePlayerStateServerRpc(PlayerState.Run);
        }
      
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            calcVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //gravity
        calcVelocity.y += gravity * Time.deltaTime;

        UpdateClientPositionAndRotationServerRpc(move, calcVelocity);

    }
    [ServerRpc]
    public void UpdateClientPositionAndRotationServerRpc(Vector3 newPosition ,Vector3 Y)
    {
        networkPositionDirection.Value = newPosition;
        networkPositionDirectionY.Value = Y;
    }

    [ServerRpc]
    public void UpdatePlayerStateServerRpc(PlayerState state)
    {
        networkPlayerState.Value = state;
    }
    private static bool ActiveRunningActionKey()
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }
    
}
/*using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class PlayerControl : NetworkBehaviour
{
    [SerializeField]
    private float walkSpeed = 2.0f;

    [SerializeField]
    private float runSpeedOffset = 1.5f;


    [SerializeField]
    private Vector2 defaultInitialPositionOnPlane = new Vector2(-4, 4);

    [SerializeField]
    private NetworkVariable<Vector3> networkPositionDirection = new NetworkVariable<Vector3>();
    [SerializeField]
    private NetworkVariable<Vector3> networkPositionDirectionY = new NetworkVariable<Vector3>();
    [SerializeField]
    private NetworkVariable<Quaternion> networkRotationDirection = new NetworkVariable<Quaternion>();

    [SerializeField]
    private NetworkVariable<PlayerState> networkPlayerState = new NetworkVariable<PlayerState>();

    public bool IsJumping;
    public float JumpPower;

    private CharacterController CharacterController;
    // client caches positions
    private Vector3 oldInputPosition = Vector3.zero;
    private Vector3 oldInputRotation = Vector3.zero;
    private PlayerState oldPlayerState = PlayerState.Idle;
    Vector3 moveDirection = Vector3.zero;
    private float currentV = 0;
    private float currentH = 0;
    private readonly float interpolation = 10;
    private Vector3 currentDirection = Vector3.zero;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        CharacterController = GetComponent<CharacterController>();
        IsJumping = false;
    }

    void Start()
    {
        if (IsClient && IsOwner)
        {
            transform.position = new Vector3(Random.Range(defaultInitialPositionOnPlane.x, defaultInitialPositionOnPlane.y), 0,
                   Random.Range(defaultInitialPositionOnPlane.x, defaultInitialPositionOnPlane.y));
        }
    }

    void FixedUpdate()
    {
        ClientMoveAndRotate();
        ClientVisuals();
        if (IsClient && IsOwner)
        {
            ClientInput();
            if (moveDirection.y == 0)
                UpdatePlayerStateServerRpc(PlayerState.Ground);
        }
    }

    private void ClientMoveAndRotate()
    {

        transform.position += networkPositionDirection.Value;
        if (networkPositionDirectionY.Value != Vector3.zero)
            CharacterController.Move(networkPositionDirectionY.Value);
        transform.rotation = networkRotationDirection.Value;

    }

    private void ClientVisuals()
    {
        if (oldPlayerState != networkPlayerState.Value && networkPlayerState.Value != PlayerState.Jump && networkPlayerState.Value != PlayerState.Ground)
        {
            oldPlayerState = networkPlayerState.Value;
            animator.SetTrigger($"{networkPlayerState.Value}");
        }
        if (oldPlayerState != networkPlayerState.Value && networkPlayerState.Value == PlayerState.Jump && networkPlayerState.Value != PlayerState.Ground)
        {
            oldPlayerState = networkPlayerState.Value;
            animator.SetBool("Jump", true);
        }
        if (oldPlayerState != networkPlayerState.Value && networkPlayerState.Value == PlayerState.Ground && networkPlayerState.Value != PlayerState.Jump)
        {
            oldPlayerState = networkPlayerState.Value;
            animator.SetBool("Jump", false);
        }

    }

    private void ClientInput()
    {
        // left & right rotation
        Vector3 inputRotation = new Vector3(0, Input.GetAxis("Horizontal"), 0);

        // forward & backward direction
        //Vector3 direction = transform.TransformDirection(Vector3.forward);
        float forwardInput = Input.GetAxis("Vertical");
        //Vector3 inputPosition = direction * forwardInput;
        Vector3 inputPosition = Vector3.zero;
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        Transform camera = Camera.main.transform;
        currentV = Mathf.Lerp(currentV, v, Time.deltaTime * interpolation);
        currentH = Mathf.Lerp(currentH, h, Time.deltaTime * interpolation);
        Vector3 direction = camera.forward * currentV + camera.right * currentH;
        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if (direction != Vector3.zero)
        {
            currentDirection = Vector3.Slerp(currentDirection, direction, Time.deltaTime * interpolation);
            inputPosition = currentDirection;
        }
        // change animation states
        if (inputPosition == Vector3.zero)
            UpdatePlayerStateServerRpc(PlayerState.Idle);
        else if (!ActiveRunningActionKey() && inputPosition != Vector3.zero)
            UpdatePlayerStateServerRpc(PlayerState.Walk);
        else if (ActiveRunningActionKey() && inputPosition != Vector3.zero)
        {
            inputPosition = currentDirection * runSpeedOffset;
            UpdatePlayerStateServerRpc(PlayerState.Run);
        }

        Jump();
        if (oldInputPosition != inputPosition ||
            oldInputRotation != inputRotation)
        {
            oldInputPosition = inputPosition;

            UpdateClientPositionAndRotationServerRpc(inputPosition * walkSpeed * Time.deltaTime, Quaternion.LookRotation(currentDirection), moveDirection);
        }
    }

    private void Jump()
    {
        if (CharacterController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveDirection.y = JumpPower;
                UpdatePlayerStateServerRpc(PlayerState.Jump);
            }
        }
        else
        {
            moveDirection.y -= JumpPower * Time.deltaTime;
        }
    }
    private static bool ActiveRunningActionKey()
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    [ServerRpc]
    public void UpdateClientPositionAndRotationServerRpc(Vector3 newPosition, Quaternion newRotation, Vector3 y)
    {
        networkPositionDirection.Value = newPosition;
        networkRotationDirection.Value = newRotation;
        networkPositionDirectionY.Value = y;
    }

    [ServerRpc]
    public void UpdatePlayerStateServerRpc(PlayerState state)
    {
        networkPlayerState.Value = state;
    }
}
*/