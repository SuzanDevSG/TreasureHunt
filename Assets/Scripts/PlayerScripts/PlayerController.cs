using System;
using UnityEditor.Experimental.GraphView;
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
    private InputAction DashAction;

    private Vector2 moveInput;

    [Header("Variables")]
    private float angleVelocity =0;
    [HideInInspector] public Vector3 playerControl;
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private float runSpeed, walkSpeed, currentSpeed, currentLookSpeed;
    private bool IsRunning, IsDashing;
    [SerializeField] private float currentDashCooldown =0.0f;
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
    }
    private void Start()
    {
        MoveAction = playerInputSystem.Player.Movement;
        RunAction = playerInputSystem.Player.Run;
        DashAction = playerInputSystem.Player.Dash;

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
        currentSpeed = IsRunning ? runSpeed : walkSpeed ;

        DashReady();
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

        if (!IsDashing)
        {
            moveDir = Quaternion.Euler(0, smoothAngle, 0) * Vector3.forward;
            rb.MovePosition(transform.position + currentSpeed * Time.fixedDeltaTime * moveDir);
        }
    }
    private void MovementInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    private void DashReady()
    {
        if(!playerProfileSO.CanDash) 
            return ;

        if (currentDashCooldown > 0.0f)
        {
            currentDashCooldown -= Time.fixedDeltaTime;
            return ;
        }
        if (DashAction.triggered == true)
        {
            IsDashing = true;
            currentDashCooldown = playerProfileSO.dashCooldown;
            PlayerDash();
        }
    }
    private void PlayerDash()
    {
        //rb.AddForce(transform.forward * playerProfileSO.dashSpeed,ForceMode.Impulse);
        rb.MovePosition(transform.position + playerProfileSO.dashSpeed  * transform.forward);
        Invoke(nameof(ResetPlayerDash), playerProfileSO.dashDuration);
    }
    private void ResetPlayerDash()
    {
        IsDashing = false;
    }
}
