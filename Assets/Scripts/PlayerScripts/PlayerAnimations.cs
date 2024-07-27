using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private PlayerController playerController;
    private Animator animator;
    private float speed = 0.00f;
    private Vector3 initialPosition;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        initialPosition = transform.position;
    }

    private void Update()
    {
        MovementAnimations();
    }
    void FixedUpdate()
    {
        
        speed = Vector3.Distance(initialPosition,transform.position) / Time.fixedDeltaTime;
        initialPosition = transform.position;
        
    }
    private void MovementAnimations()
    {
        animator.SetFloat("Speed", speed);
    }
}
