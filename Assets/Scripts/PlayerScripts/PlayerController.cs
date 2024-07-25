using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("GetComponent")]
    public PlayerProfileSO playerProfileSO;
    private Rigidbody rb;

    [Header("PlayerInputActions")]
    public PlayerInputSystem playerInputSystem;
    private InputAction MoveAction;
    private InputAction JumpAction;
    private InputAction WalkAction;
    private Vector2 moveInput;

    [Header("Variables")]
    private float angleVelocity =0;
    [HideInInspector] public Vector3 playerControl;
    [SerializeField] private float runSpeed, walkSpeed, currentSpeed, currentLookSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool IsWalking;
    [SerializeField] private bool IsGrounded, IsJumping;


    private void Awake()
    {
        playerInputSystem = new PlayerInputSystem();
        rb = GetComponent<Rigidbody>();

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
        JumpAction = playerInputSystem.Player.Jump;
        WalkAction = playerInputSystem.Player.Walk;

        MoveAction.performed += MovementInput;
        MoveAction.canceled += MovementInput;

        runSpeed = playerProfileSO.maxSpeed;
        walkSpeed = playerProfileSO.maxSpeed / 2;

    }
    private void OnDestroy()
    {
        MoveAction.performed -= MovementInput;
        MoveAction.canceled -= MovementInput;
    }


    private void Update()
    {
        playerControl = new Vector3(moveInput.x, 0, moveInput.y);

        IsWalking = WalkAction.IsPressed();
        currentSpeed = IsWalking ? walkSpeed : runSpeed;
        currentLookSpeed = IsWalking ? playerProfileSO.lookSpeed * 2 : playerProfileSO.lookSpeed;

        if (JumpAction.triggered)
        {
            IsJumping = true;
        }
    }
    private void FixedUpdate()
    {
        PlayerMovement();
        PlayerRotation();
        PlayerJump();
    }
    private void PlayerRotation()
    {
        if (playerControl.sqrMagnitude == 0) return;

        var angle = Mathf.Atan2(playerControl.x, playerControl.z) * Mathf.Rad2Deg;
        var smoothAngle = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, angle,ref angleVelocity, currentLookSpeed);
        rb.rotation = Quaternion.Euler(0, smoothAngle, 0);
    }
    private void PlayerMovement()
    {
        rb.MovePosition(transform.position + playerControl * currentSpeed * Time.fixedDeltaTime);
    }
    private void PlayerJump()
    {
        if (IsJumping & IsGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            IsJumping= false;
        }
    }
    private void MovementInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Ground")){
            IsGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }


}
