using UnityEngine;


[CreateAssetMenu(menuName ="SO/Character/Skills")]
public class PlayerSkillsSO : ScriptableObject
{
    public bool CanDash = false;
    public float dashCooldown, dashDuration, dashSpeed;

    public bool CanInvisible = false;
    public float InvisibleCooldown, InvisibleDuration;

}
