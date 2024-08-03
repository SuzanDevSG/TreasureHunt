using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("GetComponent")]
    public PlayerProfileSO playerProfileSO;
    private Rigidbody rb;
    private Camera cam;

    [Header("PlayerInputActions")]
    public PlayerInputSystem playerInputSystem;
    private InputAction MoveAction;
    private InputAction RunAction;
    [HideInInspector] public InputAction DashAction;
    [HideInInspector] public InputAction InvisibleAction;

    private Vector2 moveInput;

    [Header("Variables")]
    private float angleVelocity =0;
    [HideInInspector] public Vector3 playerControl;
    [HideInInspector] public Vector3 moveDir;
    [SerializeField] private float runSpeed, walkSpeed, maxSpeed, currentLookSpeed;
    [HideInInspector] public bool IsRunning;

    [Header("SpeedHandler")]
    [HideInInspector] public bool Dashing;
    private bool smoothSpeedCoroutine;
    public float smoothSpeed;
    private float defualtSpeedFactor = 3;
    [SerializeField] private float currentSpeedFactor;

    private void Awake()
    {
        playerInputSystem = new PlayerInputSystem();
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }
    private void OnEnable()
    {
        playerInputSystem.Enable();
    }
    private void OnDisable()
    {
        playerInputSystem.Disable();
        StopAllCoroutines();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        MoveAction = playerInputSystem.Player.Movement;
        RunAction = playerInputSystem.Player.Run;
        DashAction = playerInputSystem.Player.Dash;
        InvisibleAction = playerInputSystem.Player.Invisible;

        MoveAction.performed += MovementInput;
        MoveAction.canceled += MovementInput;

        walkSpeed = playerProfileSO.maxSpeed;
        runSpeed = playerProfileSO.maxSpeed * 2;


    }
    private void OnDestroy()
    {
        MoveAction.performed -= MovementInput;
        MoveAction.canceled -= MovementInput;
    }
    private void Update()
    {
        playerControl = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        IsRunning = RunAction.IsPressed();
        currentLookSpeed = IsRunning ? playerProfileSO.lookSpeed : playerProfileSO.lookSpeed * 2;
        maxSpeed = IsRunning ? runSpeed : walkSpeed ;
        currentSpeedFactor = IsRunning ? defualtSpeedFactor * 2 +4 : defualtSpeedFactor;
        SmoothSpeedCondition();

    }


    private void FixedUpdate()
    {
        PlayerControl();
    }
    private void PlayerControl()
    {
        if (playerControl.sqrMagnitude == 0) return ;

        var angle = Mathf.Atan2(playerControl.x, playerControl.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
        var smoothAngle = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, angle, ref angleVelocity, currentLookSpeed);
        rb.rotation = Quaternion.Euler(0, smoothAngle, 0);

        moveDir = Quaternion.Euler(0, smoothAngle, 0) * Vector3.forward;
        rb.MovePosition(transform.position + smoothSpeed * Time.fixedDeltaTime * moveDir);
    }
    private void MovementInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    private void SmoothSpeedCondition()
    {
        if (playerControl.sqrMagnitude == 0)
        {
            smoothSpeed -= Time.deltaTime * currentSpeedFactor;
            maxSpeed = 0;
        }
        if (playerControl.sqrMagnitude != 0 && !IsRunning)
        {
            maxSpeed = walkSpeed;
        }
        if (playerControl.sqrMagnitude != 0 && IsRunning)
        {
            maxSpeed = runSpeed;
        }
        if (Dashing)
        {
            currentSpeedFactor = 12f;
            smoothSpeed = 15f;
            maxSpeed = 15f;

        }
        if (playerControl.sqrMagnitude != 0 && !smoothSpeedCoroutine)
        {
            StartCoroutine(nameof(SmoothSpeed));
        }
        smoothSpeed = Mathf.Clamp(smoothSpeed, 0, maxSpeed);

    }
    private IEnumerator SmoothSpeed()
    {
        smoothSpeedCoroutine = !smoothSpeedCoroutine;
        yield return null;
        smoothSpeed += Time.deltaTime * currentSpeedFactor;
        smoothSpeedCoroutine = !smoothSpeedCoroutine;
    }

}
