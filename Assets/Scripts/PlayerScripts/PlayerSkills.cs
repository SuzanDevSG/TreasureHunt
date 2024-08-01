using System;
using System.Collections;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [SerializeField] private PlayerSkillsSO playerSkillsSO;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Material[] playerMat;
    private Rigidbody rb;

    [SerializeField] private float currentDashCooldown;
    [Range(0,255)][SerializeField] private float defaultAlphaValue = 255;
    private bool IsDashing, IsInvisible;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        DashReady();
        Invisible();

    }

    private void DashReady()
    {
        currentDashCooldown = Mathf.Clamp(currentDashCooldown, 0, playerSkillsSO.dashCooldown);
        if (!playerSkillsSO.CanDash)
            return;

        if (currentDashCooldown > 0.0f)
        {
            currentDashCooldown -= Time.fixedDeltaTime;
            return;
        }
        if (playerController.DashAction.triggered && !IsDashing) 
        {
            IsDashing = true;
            currentDashCooldown = playerSkillsSO.dashCooldown;
            PlayerDash();
        }
    }
    private void PlayerDash()
    {
        rb.AddForce(playerController.moveDir * playerSkillsSO.dashSpeed, ForceMode.VelocityChange);
        //rb.MovePosition(transform.position + playerProfileSO.dashSpeed  * transform.forward);
        Invoke(nameof(ResetPlayerDash), playerSkillsSO.dashDuration);
    }
    private void ResetPlayerDash()
    {
        rb.velocity = Vector3.zero;
        IsDashing = false;
    }



    private IEnumerator InvisibleReady()
    {
        if (IsInvisible)
        {
            Color changeColor = new Color();
            foreach (var item in playerMat)
            {
                changeColor = item.color;
                changeColor.a = defaultAlphaValue / 2;

                item.SetFloat("_Mode", 1.0f);
                item.color = changeColor;
                Debug.Log("AlpaChanged " + item.color);
            }

            yield return new WaitForSeconds(playerSkillsSO.InvisibleDuration);
            foreach (var item in playerMat)
            {
                changeColor = item.color;
                changeColor.a = defaultAlphaValue ;

                item.SetFloat("_Mode", 0.0f);
                item.color = changeColor;
            }

            IsInvisible = false;
            yield return new WaitForSeconds(playerSkillsSO.InvisibleCooldown);

            playerSkillsSO.CanInvisible = true;
            Debug.Log("Player is Invisible");
        }
        else
        {
            Debug.Log("Player is visible");

        }
    }

    private void Invisible()
    {
        if (!playerSkillsSO.CanInvisible)
        {
            return;
        }

        if (!playerController.InvisibleAction.triggered)
        {
            return;
        }
        IsInvisible = true;
        playerSkillsSO.CanInvisible = false;
        StartCoroutine(InvisibleReady());
    }



}