using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingEnemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public EnemyData enemyData;
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void Update()
    {
        playerInSightRange = CheckPlayerInSightRange();
        playerInAttackRange = Physics.CheckSphere(transform.position, enemyData.attackRange, enemyData.WhatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patrol();
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            // Have to add or remaining to add chase player or other behavior
        }
        else if (playerInAttackRange)
        {
            // Have to add or remaining to add attack player or other behavior
        }
    }

    private bool CheckPlayerInSightRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, enemyData.sightRange, enemyData.WhatIsPlayer);
        foreach (var hit in hits)
        {
            Transform target = hit.transform;
            Vector3 directionToPlayer = (target.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);

            //perform raycast and check obstacle
            if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, enemyData.ObstacleMask))
            {
                return true;
            }
        }
        return false;
    }

    private void Patrol()
    {
        if (waypoints.Length == 0)
            return;

        if (agent.remainingDistance < 1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) %waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }
}
