using UnityEngine;

[CreateAssetMenu(menuName ="SO/Character/Player")]
public class PlayerProfileSO : ScriptableObject
{
    public int id;
    public string characterName, characterDescription;


    public float maxSpeed, maxHealth, maxEnergy;
    [Range(0,1)]public float lookSpeed =0f;

    public float dashCooldown, dashDuration, dashSpeed;
    public bool CanDash = false;
    public float InvisibleCooldown, InvisibleDuration;
    public bool CanInvisible = false;

    public int[] skillId;

}
