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
    public bool playerInSightRange, playerInChaseRange;
    [SerializeField] private Transform SpawnPoint;
    public float Speed;

    [SerializeField] private Animator animator;

    // Field of View angle in degrees
    public float fieldOfViewAngle = 110f;
    [SerializeField] private float chaseRange = 15f; // Chase range for the enemy
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private AudioSource chaseAudioSource;

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
        playerInChaseRange = CheckPlayerInChaseRange();

        if (!playerInSightRange && !playerInChaseRange)
        {
            Patrol();
        }
        else if (playerInSightRange && playerInChaseRange)
        {
            ChasePlayer();
        }
        else if (!playerInSightRange && playerInChaseRange)
        {
            Patrol();
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

    private bool CheckPlayerInChaseRange()
    {
        return Vector3.Distance(transform.position, player.position) <= chaseRange;
    }

    private void Patrol()
    {
        if (waypoints.Length == 0)
            return;

        animator.SetBool("isPatrolling", true); // Set patrolling animation
        animator.SetBool("isChasing", false); // Disable chasing animation\
        agent.speed = patrolSpeed;
        if (chaseAudioSource.isPlaying)
        {
            chaseAudioSource.Stop(); 
        }
    

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
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
        if (!chaseAudioSource.isPlaying)
        {
            chaseAudioSource.Play(); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           other.gameObject.SetActive(false) ; // Set player inactive
        }
    }
}
