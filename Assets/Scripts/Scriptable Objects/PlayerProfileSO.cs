using UnityEngine;

[CreateAssetMenu(menuName ="SO/Character/Player")]
public class PlayerProfileSO : ScriptableObject
{
    public int id;
    public string characterName, characterDescription;


    public float maxSpeed, maxJump, maxHealth, maxEnergy;
    public int[] skillId;

}
