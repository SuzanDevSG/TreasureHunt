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
    private Vector2 moveInput;

    [Header("Variables")]
    private float angleVelocity =0;
    [HideInInspector] public Vector3 playerControl;
    [SerializeField] private float runSpeed, walkSpeed, currentSpeed, currentLookSpeed;
    private bool IsRunning;


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
        currentSpeed = IsRunning ? runSpeed : walkSpeed ;
        currentLookSpeed = IsRunning ? playerProfileSO.lookSpeed : playerProfileSO.lookSpeed * 2;
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

        var moveDir = Quaternion.Euler(0, smoothAngle, 0) * Vector3.forward;
        rb.MovePosition(transform.position + currentSpeed * Time.fixedDeltaTime * moveDir);
    }
    private void MovementInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
