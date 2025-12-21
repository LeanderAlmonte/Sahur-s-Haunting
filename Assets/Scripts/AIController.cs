using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent Agent { get; private set; }
    public AIAnimationController aiAnimationController { get; private set; }
    // public Animator Animator { get; private set; } // Not needed since we're not using animations
    public Transform[] Waypoints;
    public Transform Player;

    public float AttackRange = 2f; // New attack range variable
    public LayerMask PlayerLayer;
    public StateType currentState;

    [Header("Attack Settings")]
    public Transform leftHandTransform;
    public Transform rightHandTransform;
    [Header("Vision Settings")]
    public float viewDistance = 10f;
    public float viewAngle = 90f;
    public float eyeHeight = 1.6f; // where the AI "looks" from
    public LayerMask obstacleMask;
    public LayerMask playerMask;

    [Header("Vision Stability")]
    public float visionPersistence = 0.5f; // seconds to keep seeing after losing sight
    private float lastSeenTime = -999f;

    [Header("Audio")]
    public AudioSource chaseAudioSource;
    public AudioClip chaseClip;
    private StateType _prevState;



    // Add State Machine code Here

    public StateMachine StateMachine { get; private set; }

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        aiAnimationController = GetComponent<AIAnimationController>();
        // Animator = GetComponent<Animator>(); // Commented out since we're not using animations

        StateMachine = new StateMachine();
        StateMachine.AddState(new IdleState(this));
        StateMachine.AddState(new PatrolState(this));
        StateMachine.AddState(new ChaseState(this));
        StateMachine.AddState(new AttackState(this)); // Add the new AttackState

        StateMachine.TransitionToState(StateType.Idle);

        _prevState = StateMachine.GetCurrentStateType();
        if (chaseAudioSource != null)
        {
            chaseAudioSource.playOnAwake = false;
            chaseAudioSource.loop = true;
        }
    }

    void Update()
    {
        StateMachine.Update();
        currentState = StateMachine.GetCurrentStateType();

        // Start/stop chase music based on state transitions only
        if (currentState != _prevState)
        {
            if (currentState == StateType.Chase) StartChaseAudio();
            if (_prevState == StateType.Chase && currentState != StateType.Chase) StopChaseAudio();

            _prevState = currentState;
        }
    }

    private void StartChaseAudio()
    {
        if (chaseAudioSource == null || chaseClip == null) return;

        // If already playing, do nothing
        if (chaseAudioSource.isPlaying) return;

        chaseAudioSource.clip = chaseClip;
        chaseAudioSource.loop = true;
        chaseAudioSource.Play();
    }

    private void StopChaseAudio()
    {
        if (chaseAudioSource == null) return;
        if (!chaseAudioSource.isPlaying) return;

        chaseAudioSource.Stop();
    }


    // 
    public bool CanSeePlayer()
    {
        if (Player == null)
        {
            return false;
        }

        Vector3 eyePosition = transform.position + Vector3.up * eyeHeight;
        Vector3 targetPosition = Player.position + Vector3.up * 0.5f;
        Vector3 directionToPlayer = (targetPosition - eyePosition).normalized;
        float distanceToPlayer = Vector3.Distance(eyePosition, targetPosition);
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // Check field of view
        if (angleToPlayer > viewAngle / 2f)
        {
            return Time.time - lastSeenTime < visionPersistence;
        }

        // Check distance
        if (distanceToPlayer > viewDistance)
        {
            return Time.time - lastSeenTime < visionPersistence;
        }

        // Perform raycast
        if (Physics.Raycast(eyePosition, directionToPlayer, out RaycastHit hit, viewDistance))
        {
            // If hit the player
            if (hit.transform == Player)
            {
                lastSeenTime = Time.time;
                return true;
            }
        }
    

        // If recently seen, still count as visible
        bool recentlySeen = Time.time - lastSeenTime < visionPersistence;

        return recentlySeen;
    }

    public bool CheckHandsCollision(out GameObject collidedObject, string Tag)
    {
        
        // You can define these in AIController (leftHandPoint, rightHandPoint)
        Transform[] handTransforms = { leftHandTransform, rightHandTransform };
        Debug.Log(handTransforms);
        foreach (Transform hand in handTransforms)
        {
            Debug.Log("Hand : " + hand);
            // Overlap check — sphere or capsule works well for melee hitboxes
            Collider[] hits = Physics.OverlapSphere(hand.position, 3.0f, PlayerLayer);

            foreach (var hit in hits)
            {
                 Debug.Log("Hit : "  + hit);
                if (hit.CompareTag(Tag))
                {
                    collidedObject = hit.gameObject;
                    return true;
                }
            }
        }
        collidedObject = null;
        return false;
    }
    


    // New method to check if the AI is within attack range
    public bool IsPlayerInAttackRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
        return distanceToPlayer <= AttackRange;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewDistance);
    }

}
