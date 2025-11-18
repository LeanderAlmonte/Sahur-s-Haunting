using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SahurModelSetup : MonoBehaviour
{
    [Header("Model Setup")]
    public GameObject sahurModel;
    public Animator sahurAnimator;
    
    [Header("Audio Setup")]
    public AudioSource sahurAudioSource;
    
    void Start()
    {
        SetupSahurModel();
    }
    
    void SetupSahurModel()
    {
        // Ensure NavMeshAgent is properly configured
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.radius = 0.5f;
            agent.height = 1.8f;
            agent.speed = 2f;
            agent.acceleration = 8f;
            agent.angularSpeed = 120f;
        }
        
        // Setup colliders for detection
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        if (capsule == null)
        {
            capsule = gameObject.AddComponent<CapsuleCollider>();
            capsule.radius = 0.5f;
            capsule.height = 1.8f;
            capsule.center = new Vector3(0, 0.9f, 0);
        }
        
        // Add rigidbody for physics interactions
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true; // NavMeshAgent controls movement
        }
        
        // Setup audio
        if (sahurAudioSource == null)
        {
            sahurAudioSource = gameObject.AddComponent<AudioSource>();
            sahurAudioSource.spatialBlend = 1f; // 3D audio
            sahurAudioSource.rolloffMode = AudioRolloffMode.Linear;
            sahurAudioSource.maxDistance = 20f;
        }
        
        // Tag the object
        gameObject.tag = "Enemy";
    }
}