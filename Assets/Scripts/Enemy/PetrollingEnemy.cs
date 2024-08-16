using UnityEngine;
using UnityEngine.AI;

public class PetrollingEnemy : MonoBehaviour
{
    [SerializeField] private EventHandler eventHandler;
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
    public GameOverManager gameOverManager;

    public static bool isChasingPlayer = false;
    public static bool isGameOver = false;


    

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
        isGameOver = false;
    }

    void Update()
    {
        if (isGameOver) return; // Add this line to prevent further updates if the game is over

        playerInSightRange = CheckPlayerInSightRange();
        playerInChaseRange = CheckPlayerInChaseRange();

        if (!isChasingPlayer)
        {
            if (playerInSightRange)
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
            if (playerInChaseRange && !playerInSightRange)
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
                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, enemyData.ObstacleMask )&& !PlayerSkills.IsInvisible)
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
        Debug.Log("patrolling");
        eventHandler.StopPlayerChasing?.Invoke();

        if (waypoints.Length == 0)
            return;

        animator.SetBool("isPatrolling", true); // Set patrolling animation
        animator.SetBool("isChasing", false); // Disable chasing animation
        agent.speed = patrolSpeed;
        if (agent.remainingDistance < 1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    private void ChasePlayer()
    {
        Debug.Log("chasingPlayer");
        eventHandler.StartPlayerChasing?.Invoke();

        if (isGameOver) return; // Add this line to prevent chasing if the game is over

        animator.SetBool("isPatrolling", false); // Disable patrolling animation
        animator.SetBool("isChasing", true); // Enable chasing animation
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float distanceToPlayer = Vector3.Distance(transform.position, other.transform.position);
            if (distanceToPlayer <= catchingRange)
            {
                CatchPlayer(other.gameObject);
            }
        }
    }

    private void CatchPlayer(GameObject player)
    {
        isGameOver = true; 

        player.SetActive(false); // Set player inactive
        agent.speed = 0; // Stop the enemy movement

        // Stop the animations
        animator.SetBool("isPatrolling", false);
        animator.SetBool("isChasing", false);

        gameOverManager.ShowGameOver();
    }
}
