using UnityEngine;

[CreateAssetMenu(menuName ="SO/Character/Player")]
public class PlayerProfileSO : ScriptableObject
{
    public int id;
    public string characterName, characterDescription;


    public float maxSpeed, maxHealth, maxEnergy;
    [Range(0,1)]public float lookSpeed =0f;
    public int[] skillId;

}
