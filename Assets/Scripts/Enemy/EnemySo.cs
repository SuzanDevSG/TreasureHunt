using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    
    public float sightRange;
    public float attackRange;
    public LayerMask WhatIsPlayer;
    public LayerMask WhatIsGround;
    public LayerMask ObstacleMask;
}
