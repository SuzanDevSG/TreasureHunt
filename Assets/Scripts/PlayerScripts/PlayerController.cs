using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public PlayerProfileSO playerProfileSO;
    public PlayerInputSystem playerInputSystem;

    private Rigidbody rb;
    [SerializeField] private float currentSpeed;
    private Vector3 inputMovement;
    private Vector2 playerinput;

    private float velocity = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Control();
    }

    private void FixedUpdate()
    {
        Movement();
        Rotation();
    }

    private void Rotation()
    {
        if (inputMovement.sqrMagnitude == 0) return;
        var angle = Mathf.Atan2(playerinput.x, playerinput.y) * Mathf.Rad2Deg;
        var direction = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle,ref velocity, .5f);
        rb.rotation = Quaternion.Euler(0,direction,0);
    }

    private void Control()
    {
        inputMovement = new Vector3(playerinput.x, 0, playerinput.y);
    }

    private void Movement()
    {
        rb.MovePosition(transform.position + playerProfileSO.maxSpeed * Time.deltaTime * inputMovement);

    }

    public void PlayerInput(InputAction.CallbackContext context)
    {
        playerinput = context.ReadValue<Vector2>();
        //jumpInput = context.ReadValue<int>();

        Debug.Log(playerinput);
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        rb.AddForce(transform.up * playerProfileSO.maxJump, ForceMode.Impulse);


    }

}
