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
    [SerializeField] private float catchingRange = 2f; // Define the catching range
    [SerializeField] private AudioSource chaseAudioSource;
    public GameOverManager gameOverManager;

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
        else if (!playerInSightRange && !playerInChaseRange)
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
        animator.SetBool("isChasing", false); // Disable chasing animation
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
            Debug.Log("Enemy collided with player");
            float distanceToPlayer = Vector3.Distance(transform.position, other.transform.position);
            if (distanceToPlayer <= catchingRange)
            {
                Debug.Log("Player within catching range");
                CatchPlayer(other.gameObject);
            }
        }
    }
    private void CatchPlayer(GameObject player)
    {
        // Trigger your game logic here
        // For example, disable the player and show a "Game Over" screen

        player.SetActive(false); // Set player inactive
        agent.speed = 0; // Stop the enemy movement

        // Stop the animations
        animator.SetBool("isPatrolling", false);
        animator.SetBool("isChasing", false);

       
        if (chaseAudioSource.isPlaying)
        {
            chaseAudioSource.Stop();
        }
        gameOverManager.ShowGameOver();

        Debug.Log("Player caught and set inactive! Enemy movement stopped.");
    }
}
