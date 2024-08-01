using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [SerializeField] private PlayerSkillsSO playerSkillsSO;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private List<Material> playerMat;
    private Rigidbody rb;

    [SerializeField] private float currentDashCooldown;
    [Range(0,255)][SerializeField] private float defaultAlphaValue = 1;
    private bool IsDashing, IsInvisible;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        var playerRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        playerMat = new List<Material>();
        foreach (var item in playerRenderers)
        {
            playerMat.Add(item.material);
        }

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
        Debug.Log("Invisible coroutine called ");
        if (IsInvisible)
        {
            Debug.Log("Invisible Started");
            Color changeColor = new Color();
            foreach (var item in playerMat)
            {
                changeColor = item.color;
                changeColor.a = 0f;

                item.SetFloat("_Mode", 3.0f);
                item.renderQueue = 3000;
                item.color = changeColor;
                Debug.Log("AlpaChanged " + item.color);
            }
            Debug.Log("Player is Invisible");

            yield return new WaitForSeconds(playerSkillsSO.InvisibleDuration);
/*
            foreach (var item in playerMat)
            {
                changeColor = item.color;
                changeColor.a = defaultAlphaValue ;

                item.SetFloat("_Mode", 0.0f);
                item.color = changeColor;
            }
            Debug.Log("Player is visible");


            IsInvisible = false;
            yield return new WaitForSeconds(playerSkillsSO.InvisibleCooldown);

            playerSkillsSO.CanInvisible = true;*/
        }

    }

    private void Invisible()
    {

        if (!playerSkillsSO.CanInvisible)
        {
            Debug.Log("No Skill to Use"+ playerSkillsSO.CanInvisible);
            return;
        }

        if (!playerController.InvisibleAction.triggered)
        {
            return;
        }
        Debug.Log("Triggered E");
        IsInvisible = true;
        playerSkillsSO.CanInvisible = true;
        StartCoroutine(InvisibleReady());
    }



}