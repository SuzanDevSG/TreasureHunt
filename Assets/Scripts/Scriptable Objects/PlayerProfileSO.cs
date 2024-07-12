using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/Character/Player")]
public class PlayerProfileSO : ScriptableObject
{
    public int id;
    public string characterName, characterDescription;


    public float maxSpeed, maxHealth, maxEnergy;
    public int[] skillId;

}
