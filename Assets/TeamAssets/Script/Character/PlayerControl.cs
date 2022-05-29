using Unity.Netcode;
using UnityEngine;
using DilmerGames.Core.Singletons;
using Unity.Netcode.Samples;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(ClientNetworkTransform))]
public class PlayerControl : NetworkSingleton<PlayerControl>
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
    [SerializeField]
    private LayerMask ground;
    [SerializeField]
    private GameObject objectPrefabLantern;
    //to get CharacterController from the unity
    private CharacterController characterController;
    [SerializeField]
    private Vector2 defaultInitialPositionOnPlane = new Vector2(-4, 4);

    [SerializeField]
    private NetworkVariable<PlayerState> networkPlayerState = new NetworkVariable<PlayerState>();
    //to calculate
    private Vector3 calcVelocity = Vector3.zero;

    //movement(vector3)

    //prevent double jump
    [SerializeField]
    LayerMask groundLayerMask; // in the ground layer, player can jump only
    private Animator animator;
    private PlayerState oldPlayerState = PlayerState.Idle;
    private bool isGrounded;
    private bool isJumping = false;
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
        CameraMove.Instance.FollowPlayer(transform);
    }

    // Update is called once per frame
    void Update()
    {

        if (IsClient && IsOwner)
        {
            ClientMove();
            LanternSetting();
        }
        Client_visual();
    }
   
    public void LanternSetting()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (IsServer)
            {
                Spawn();
            }
            else if (IsClient && IsOwner)
            {
                SpawnServerRpc();
            }

        }
    }
    public void Spawn()
    {
        Vector3 temp;
        Vector3 t = new Vector3(0, 1.5f, 0);
        temp = transform.position - t;
        GameObject go = Instantiate(objectPrefabLantern, temp, Quaternion.identity);
        go.GetComponent<NetworkObject>().Spawn();
    }
    [ServerRpc]
    public void SpawnServerRpc()
    {
        Spawn();
    }
    private void Client_visual()
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
            isJumping = true;
        }
        if (oldPlayerState != networkPlayerState.Value && networkPlayerState.Value == PlayerState.Ground && networkPlayerState.Value != PlayerState.Jump)
        {
            oldPlayerState = networkPlayerState.Value;
            animator.SetBool("Jump", false);
            isJumping = false;
        }
    }

    private void ClientMove()
    {
        //bool isGrounded = characterController.isGrounded;
        isGrounded = IsCheckGrounded();
       
        if (isGrounded && calcVelocity.y < 0)
        {
            calcVelocity.y = 0f;
        }
        
        //user input - basic movments
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (move == Vector3.zero)
        {
            UpdatePlayerStateServerRpc(PlayerState.Idle);
        }
        else if (!ActiveRunningActionKey() && move != Vector3.zero)
        {
            UpdatePlayerStateServerRpc(PlayerState.Walk);
        }
        else if (ActiveRunningActionKey() && move != Vector3.zero)
        {
            move = move * runSpeedOffset;
            UpdatePlayerStateServerRpc(PlayerState.Run);
        }
        characterController.Move(transform.TransformDirection(move) * Time.deltaTime * speed);
        /*if (move != Vector3.zero)
            transform.forward = move;*/
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            calcVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
            UpdatePlayerStateServerRpc(PlayerState.Jump);
        }

        if (isGrounded && isJumping)
        {
            UpdatePlayerStateServerRpc(PlayerState.Ground);
        }

        //gravity
        calcVelocity.y += gravity * Time.deltaTime;
        characterController.Move(calcVelocity * Time.deltaTime);   
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
    private bool IsCheckGrounded()
    {
        // CharacterController.IsGrounded가 true라면 Raycast를 사용하지 않고 판정 종료
        if (characterController.isGrounded) return true;
        // 발사하는 광선의 초기 위치와 방향
        // 약간 신체에 박혀 있는 위치로부터 발사하지 않으면 제대로 판정할 수 없을 때가 있다.
        var ray = new Ray(this.transform.position + Vector3.up * 0.1f, Vector3.down);
        // 탐색 거리
        var maxDistance = 0.15f;
        // 광선 디버그 용도
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * maxDistance, Color.red);
        // Raycast의 hit 여부로 판정
        // 지상에만 충돌로 레이어를 지정
        return Physics.Raycast(ray, maxDistance, ground);
    }

}