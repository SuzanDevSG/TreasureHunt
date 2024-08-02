using System.Collections;
using Unity.Properties;
using UnityEditor.U2D;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private PlayerController playerController;
    private Animator animator;
    private float speed = 0.00f;
    private float smoothSpeed;
    public bool Dashing;
    private bool smoothSpeedCoroutine;
    private float speedFactor = 3;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        
        if(playerController.playerControl.sqrMagnitude == 0)
        {
            smoothSpeed -= Time.deltaTime * speedFactor;
            speed = 0;
        }
        if(playerController.playerControl.sqrMagnitude != 0 && !playerController.IsRunning)
        {
            speed = 2.5f;
        }
        if (playerController.playerControl.sqrMagnitude != 0 && playerController.IsRunning)
        {
            speed = 5f;
        }
        if (Dashing)
        {
            speed = 15f;
        }
        if (playerController.playerControl.sqrMagnitude != 0 && !smoothSpeedCoroutine)
        {

            StartCoroutine(SmoothSpeed(speed));
        }
        MovementAnimations();
    }

    private IEnumerator SmoothSpeed(float speed)
    {

        yield return null;
        smoothSpeed = Mathf.Clamp(smoothSpeed, 0, speed);
        smoothSpeed += Time.deltaTime * speedFactor;

    }
    private void MovementAnimations()
    {
        animator.SetFloat("Speed", smoothSpeed);
    }
}
