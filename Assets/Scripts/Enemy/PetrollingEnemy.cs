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

    private bool isChasingPlayer = false;
    private bool isGameOver = false; // Add this line

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (chaseAudioSource == null)
        {
            Debug.LogError("chaseAudioSource is not assigned in the inspector");
        }
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
        if (isGameOver) return; // Add this line to prevent further updates if the game is over

        playerInSightRange = CheckPlayerInSightRange();
        playerInChaseRange = CheckPlayerInChaseRange();

        if (!isChasingPlayer)
        {
            if (playerInSightRange && playerInChaseRange)
            {
                isChasingPlayer = true;
                ChasePlayer();
            }
            else
            {
                Patrol();
            }
        }
        else
        {
            if (playerInChaseRange)
            {
                ChasePlayer();
            }
            else
            {
                isChasingPlayer = false;
                Patrol();
            }
        }
    }

    private bool CheckPlayerInSightRange()
    {
        // Create a sphere around the enemy to detect players within sight range
        Collider[] hits = Physics.OverlapSphere(transform.position, enemyData.sightRange, enemyData.WhatIsPlayer);
        foreach (var hit in hits)
        {
            Transform target = hit.transform;
            Vector3 directionToPlayer = (target.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);

            // Check if the player is within the field of view cone
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer < fieldOfViewAngle * 0.5f)
            {
                // Perform a raycast to check for obstacles between the enemy and the player
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
            Debug.Log("Stopping chase audio during patrol");
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
        if (isGameOver) return; // Add this line to prevent chasing if the game is over

        animator.SetBool("isPatrolling", false); // Disable patrolling animation
        animator.SetBool("isChasing", true); // Enable chasing animation
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
        if (!chaseAudioSource.isPlaying)
        {
            Debug.Log("Starting chase audio");
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
        Debug.Log("Catching player");
        isGameOver = true; // Add this line to mark the game as over

        if (chaseAudioSource.isPlaying)
        {
            Debug.Log("Stopping chase audio");
            chaseAudioSource.Stop();
        }

        player.SetActive(false); // Set player inactive
        agent.speed = 0; // Stop the enemy movement

        // Stop the animations
        animator.SetBool("isPatrolling", false);
        animator.SetBool("isChasing", false);

        gameOverManager.ShowGameOver();

        Debug.Log("Player caught and set inactive! Enemy movement stopped.");
    }
}
