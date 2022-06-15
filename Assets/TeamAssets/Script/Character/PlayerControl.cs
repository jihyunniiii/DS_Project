using Unity.Netcode;
using UnityEngine;
using DilmerGames.Core.Singletons;
using Unity.Netcode.Samples;
using TMPro;
[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(ClientNetworkTransform))]
public class PlayerControl : NetworkSingleton<PlayerControl>
{
    public BlockConfig newBlockConfig;
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

    //prevent double jump
    [SerializeField]
    LayerMask groundLayerMask; // in the ground layer, player can jump only
    private Animator animator;
    private PlayerState oldPlayerState = PlayerState.Idle;
    private bool isGrounded;
    private bool isJumping = false;
    #endregion
    private NetworkVariable<NetworkString> playersName = new NetworkVariable<NetworkString>();
    private bool overlaySet = false;
    Camera _camera;
    bool toggleCameraRotation = false;
    float smoothness = 10.0f;
    bool chatting = false;
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
            transform.position = new Vector3(UnityEngine.Random.Range(defaultInitialPositionOnPlane.x, defaultInitialPositionOnPlane.y), 45,
                   UnityEngine.Random.Range(defaultInitialPositionOnPlane.x, defaultInitialPositionOnPlane.y));
            CameraMove.Instance.FollowPlayer(transform);
            _camera = Camera.main;
        }

    }

    // Update is called once per frame
    void Update()
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
        if (IsClient && IsOwner)
        {
            if (chatting)
                return;
            if (Input.GetKey(KeyCode.LeftControl))
            {
                toggleCameraRotation = true;
            }
            else
            {
                toggleCameraRotation = false;
            }

            ClientMove();
        }
        Client_visual();
    }



    private void LateUpdate()
    {
        if (toggleCameraRotation != true)
        {
            Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }
    }
    public void Spawn()
    {
        Vector3 temp;
        Vector3 t = new Vector3(0, 5f, 0);
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
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 moveDriection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");
        //user input - basic movments
        //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (moveDriection == Vector3.zero)
        {
            UpdatePlayerStateServerRpc(PlayerState.Idle);
        }
        else if (!ActiveRunningActionKey() && moveDriection != Vector3.zero)
        {
            UpdatePlayerStateServerRpc(PlayerState.Walk);
        }
        else if (ActiveRunningActionKey() && moveDriection != Vector3.zero)
        {
            moveDriection = moveDriection * runSpeedOffset;
            UpdatePlayerStateServerRpc(PlayerState.Run);
        }
        characterController.Move(moveDriection * Time.deltaTime * speed);
        /*if (moveDriection != Vector3.zero)
            transform.forward = moveDriection;*/
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (lobby.GetInstance().IsLanternGet())
            {
                if (IsServer)
                {
                    Spawn();
                    lobby.GetInstance().updateLantern();
                }
                else if (IsClient && IsOwner)
                {
                    SpawnServerRpc();
                    lobby.GetInstance().updateLantern();
                }
            }
        }
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
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (IsClient && IsOwner)
        {
            if (hit.collider == null) return;
            else if (hit.gameObject.tag == "lotus")
            {
                Destroy(hit.gameObject);
            }
            else if (hit.gameObject.tag == "burger")
            {
                Couponevent.GetInstance().GetCoupon();
            }
            else if (hit.gameObject.tag == "Fall")
            {
                Debug.Log("이동");
                FallManager.GetInstance().SpawnCharacter(transform);
            }
            else if (hit.gameObject.tag == "GameOver")
            {
                Debug.Log("이동");
                GameOverManager.GetInstance().SpawnCharacter(transform);
            }
            else if (hit.gameObject.tag == "Hex1" || hit.gameObject.tag == "Hex2" || hit.gameObject.tag == "Hex3" || hit.gameObject.tag == "Hex4" || hit.gameObject.tag == "Hex5")
            {
                Debug.Log("삭제");
                GameObject explosionObj = newBlockConfig.GetExplosionObject();
                explosionObj.SetActive(true);
                explosionObj.transform.position = hit.gameObject.transform.position;
                Destroy(hit.gameObject, 0.2f);
            }
            else if (hit.gameObject.tag == "GameStart")
            {

                //Debug.Log("게임 시작");
                GameOverManager.GetInstance().GameStart(transform);
            }
        }

    }
}