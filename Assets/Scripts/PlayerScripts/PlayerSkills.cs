using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [SerializeField] private PlayerSkillsSO playerSkillsSO;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerAnimations playerAnimations;
    [SerializeField] private List<Material> playerMat;
    private Rigidbody rb;

    [SerializeField] private float currentDashCooldown;
    private bool IsDashing, IsInvisible;
    private bool isOpaque;

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
        playerAnimations.Dashing = true;
        rb.AddForce(playerController.moveDir * playerSkillsSO.dashSpeed, ForceMode.VelocityChange);
        //rb.MovePosition(transform.position + playerProfileSO.dashSpeed  * transform.forward);
        Invoke(nameof(ResetPlayerDash), playerSkillsSO.dashDuration);
    }
    private void ResetPlayerDash()
    {
        playerAnimations.Dashing = false;
        rb.velocity = Vector3.zero;
        IsDashing = false;
    }

    private void FixedUpdate()
    {

       /* MaterialPropertyBlock block = new MaterialPropertyBlock();
        Debug.Log("Invisible Started");
        Color changeColor = new Color();
        foreach (var item in playerMat)
        {
            changeColor = item.color;
            changeColor.a = 0.2f;
            block.SetFloat("_Mode", 3.0f);
            block.SetColor("_Color", changeColor);
            *//*item.SetFloat("_Mode", 3.0f);
            item.renderQueue = 3000;
            item.color = changeColor;*//*

            renderer.SetPropertyBlock(block);
            Debug.Log("AlpaChanged " + item.color);
        }*/
    }


    private IEnumerator InvisibleReady()
    {
        Debug.Log("Invisible coroutine called ");
        if (IsInvisible)
        {
            Debug.Log("Invisible Started");
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
            Debug.Log("Player is visible");

            IsInvisible = false;
            yield return new WaitForSeconds(playerSkillsSO.InvisibleCooldown);

            playerSkillsSO.CanInvisible = true;
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

public static class MaterialUtils
{
    public enum BlendMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent
    }

    public static void SetupBlendMode(Material material, BlendMode blendMode)
    {
        switch (blendMode)
        {
            case BlendMode.Transparent:
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                material.SetFloat("_Mode", 3.0f);
                break;
            case BlendMode.Opaque:
                material.SetOverrideTag("RenderType", "");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                material.SetFloat("_Mode", 0.0f);
                break;
            default:
                Debug.LogWarning("Warning: BlendMode: " + blendMode + " is not yet implemented!");
                break;
        }
    }
}