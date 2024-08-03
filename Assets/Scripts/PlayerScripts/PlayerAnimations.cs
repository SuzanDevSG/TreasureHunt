using System.Collections;
using Unity.Properties;
using UnityEditor.U2D;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private PlayerController playerController;
    private Animator animator;
    private float playerSpeed;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        playerSpeed = playerController.smoothSpeed;
        MovementAnimations();
    }


    private void MovementAnimations()
    {
        animator.SetFloat("Speed", playerSpeed);
    }
}
