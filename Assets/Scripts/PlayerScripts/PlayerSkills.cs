using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [SerializeField] private PlayerSkillsSO playerSkillsSO;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private List<Material> playerMat;
    private Rigidbody rb;

    private float currentDashCooldown;
    private bool IsDashing;
    public static bool IsInvisible;

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

    #region Dash
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
        playerController.Dashing = true;
        rb.AddForce(playerController.moveDir * playerSkillsSO.dashSpeed, ForceMode.VelocityChange);
        //rb.MovePosition(transform.position + playerProfileSO.dashSpeed  * transform.forward);
        Invoke(nameof(ResetPlayerDash), playerSkillsSO.dashDuration);
    }
    private void ResetPlayerDash()
    {
        playerController.Dashing = false;
        rb.velocity = Vector3.zero;
        IsDashing = false;
    }
    #endregion

    #region Invisible
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
        playerSkillsSO.CanInvisible = true;
        StartCoroutine(InvisibleReady());
    }
    private IEnumerator InvisibleReady()
    {
        if (IsInvisible)
        {
            Color changeColor = new();
            for (int i = 0; i < playerMat.Count; i++)
            {
                Material mat = playerMat[i];

                MaterialUtils.SetupBlendMode(mat,MaterialUtils.BlendMode.Transparent);

                changeColor = mat.color;
                changeColor.a = 0.2f;

                mat.color = changeColor;
            }

            yield return new WaitForSeconds(playerSkillsSO.InvisibleDuration);

            for (int i = 0; i < playerMat.Count; i++)
            {
                Material mat = playerMat[i];

                MaterialUtils.SetupBlendMode(mat, MaterialUtils.BlendMode.Opaque);

                changeColor = mat.color;
                changeColor.a = 1f;

                mat.color = changeColor;
            }

            IsInvisible = false;
            yield return new WaitForSeconds(playerSkillsSO.InvisibleCooldown);

            playerSkillsSO.CanInvisible = true;
        }

    }
    #endregion
}
