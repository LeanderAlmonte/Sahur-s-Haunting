using UnityEngine;
using UnityEngine.AI;

public class SahurNPCController : MonoBehaviour
{
    [Header("Sahur NPC Settings")]
    public float detectionRange = 10f;
    public float chaseSpeed = 3.5f;
    public float patrolSpeed = 2f;
    public Transform[] patrolPoints;
    public LayerMask playerLayer;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] sahurSounds;
    
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;
    private int currentPatrolIndex = 0;
    private bool isChasing = false;
    private bool playerDetected = false;
    
    public enum SahurState
    {
        Patrolling,
        Chasing,
        Searching,
        Idle
    }
    
    public SahurState currentState = SahurState.Patrolling;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        // Start patrolling
        if (patrolPoints.Length > 0)
        {
            agent.speed = patrolSpeed;
            GoToNextPatrolPoint();
        }
    }
    
    void Update()
    {
        DetectPlayer();
        
        switch (currentState)
        {
            case SahurState.Patrolling:
                HandlePatrolling();
                break;
            case SahurState.Chasing:
                HandleChasing();
                break;
            case SahurState.Searching:
                HandleSearching();
                break;
        }
        
        UpdateAnimations();
    }
    
    void DetectPlayer()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= detectionRange)
        {
            // Raycast to check if player is visible (not behind walls)
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            RaycastHit hit;
            
            if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer, out hit, detectionRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    playerDetected = true;
                    StartChasing();
                }
            }
        }
        else if (distanceToPlayer > detectionRange * 1.5f)
        {
            playerDetected = false;
            if (currentState == SahurState.Chasing)
            {
                StartSearching();
            }
        }
    }
    
    void HandlePatrolling()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }
    }
    
    void HandleChasing()
    {
        if (player != null && playerDetected)
        {
            agent.SetDestination(player.position);
            PlaySahurSound();
        }
        else
        {
            StartSearching();
        }
    }
    
    void HandleSearching()
    {
        // Search for a few seconds, then return to patrolling
        if (!isChasing)
        {
            Invoke("ReturnToPatrolling", 5f);
            isChasing = true;
        }
    }
    
    void StartChasing()
    {
        currentState = SahurState.Chasing;
        agent.speed = chaseSpeed;
        isChasing = true;
        CancelInvoke("ReturnToPatrolling");
    }
    
    void StartSearching()
    {
        currentState = SahurState.Searching;
        agent.speed = patrolSpeed;
        isChasing = false;
    }
    
    void ReturnToPatrolling()
    {
        currentState = SahurState.Patrolling;
        agent.speed = patrolSpeed;
        isChasing = false;
        GoToNextPatrolPoint();
    }
    
    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }
    
    void UpdateAnimations()
    {
        if (animator != null)
        {
            float speed = agent.velocity.magnitude;
            animator.SetFloat("Speed", speed);
            animator.SetBool("IsChasing", currentState == SahurState.Chasing);
        }
    }
    
    void PlaySahurSound()
    {
        if (audioSource != null && sahurSounds.Length > 0 && !audioSource.isPlaying)
        {
            int randomSound = Random.Range(0, sahurSounds.Length);
            audioSource.PlayOneShot(sahurSounds[randomSound]);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Draw patrol points
        if (patrolPoints != null)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] != null)
                {
                    Gizmos.DrawWireSphere(patrolPoints[i].position, 1f);
                    if (i < patrolPoints.Length - 1 && patrolPoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
                    }
                }
            }
        }
    }
}