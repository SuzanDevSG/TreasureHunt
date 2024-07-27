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
    [SerializeField] private Transform SpawnPoint;
    public float Speed;
    [SerializeField] private Bullet Bullet;
    public float timeBetweenAttacks;

    private Animator animator;
    private bool alreadyAttacked;

    // Field of View angle in degrees
    public float fieldOfViewAngle = 110f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Get the Animator component
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
            ChasePlayer();
        }
        else if (playerInAttackRange)
        {
            AttackPlayer();
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

            // Check if the player is within the field of view
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer < fieldOfViewAngle * 0.5f)
            {
                // Perform raycast and check for obstacles
                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, enemyData.ObstacleMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void Patrol()
    {
        if (waypoints.Length == 0)
            return;

        animator.SetBool("isPatrolling", true); // Set patrolling animation
        animator.SetBool("isChasing", false); // Disable chasing animation
        animator.SetBool("isAttacking", false); // Disable attacking animation

        if (agent.remainingDistance < 1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    private void ChasePlayer()
    {
        animator.SetBool("isPatrolling", false); // Disable patrolling animation
        animator.SetBool("isChasing", true); // Enable chasing animation
        animator.SetBool("isAttacking", false); // Disable attacking animation

        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        animator.SetBool("isPatrolling", false); // Disable patrolling animation
        animator.SetBool("isChasing", false); // Disable chasing animation
        animator.SetBool("isAttacking", true); // Enable attacking animation

        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Attack code here
            var spawnBullet = Instantiate(Bullet, SpawnPoint.position, SpawnPoint.rotation); // bullet parent => spawnPoint
            spawnBullet.throwBullet = true;
            // End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
