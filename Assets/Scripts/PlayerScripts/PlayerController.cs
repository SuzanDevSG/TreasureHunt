using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerProfileSO playerProfileSO;
    private Rigidbody rb;

    [SerializeField] private float currentSpeed;

    private Vector3 playerinput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }
    private void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {

    }

}
