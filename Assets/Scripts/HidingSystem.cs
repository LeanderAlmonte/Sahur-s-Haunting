using UnityEngine;

public class HidingSystem : MonoBehaviour
{
    public static HidingSystem Instance;

    [Header("Default hide (tables etc)")]
    public GameObject hideOverlay;     // your current table overlay
    public AudioSource hideAudio;      // your current breathing audio etc

    private CharacterController controller;
    private bool isHiding = false;
    private bool exitLocked = false;

    private Vector3 returnPosition;

    private GameObject activeOverlay;
    private AudioSource activeAudio;

    public bool IsHiding => isHiding;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        controller = GetComponent<CharacterController>();

        if (hideOverlay != null)
            hideOverlay.SetActive(false);
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void SetExitLocked(bool locked)
    {
        exitLocked = locked;
    }

    // Backward compatible (your tables call this)
    public void ToggleHide(Transform hidePosition)
    {
        ToggleHide(hidePosition, null, null);
    }

    // New: allow custom overlay/audio per hiding spot (lockers)
    public void ToggleHide(Transform hidePosition, GameObject overlayOverride, AudioSource audioOverride)
    {
        if (!isHiding)
        {
            EnterHide(hidePosition, overlayOverride, audioOverride);
        }
        else
        {
            // If a trap is running, do not allow exit
            if (exitLocked) return;

            ExitHide(null);
        }
    }

    public void EnterHide(Transform hidePosition, GameObject overlayOverride, AudioSource audioOverride)
    {
        if (isHiding) return;
        if (hidePosition == null) return;

        returnPosition = transform.position;

        // Freeze player movement
        if (controller != null) controller.enabled = false;

        // Teleport inside hiding spot
        transform.position = hidePosition.position;

        // Activate overlay/audio
        activeOverlay = overlayOverride != null ? overlayOverride : hideOverlay;
        if (activeOverlay != null) activeOverlay.SetActive(true);

        activeAudio = audioOverride != null ? audioOverride : hideAudio;
        if (activeAudio != null && !activeAudio.isPlaying) activeAudio.Play();

        isHiding = true;
    }

    // Optional exitPosition: useful for trap lockers to spit you out in front
    public void ExitHide(Transform exitPosition)
    {
        if (!isHiding) return;

        // Move out first, then restore controller
        transform.position = (exitPosition != null) ? exitPosition.position : returnPosition;

        if (controller != null) controller.enabled = true;

        if (activeOverlay != null) activeOverlay.SetActive(false);

        if (activeAudio != null && activeAudio.isPlaying) activeAudio.Stop();

        activeOverlay = null;
        activeAudio = null;

        isHiding = false;
        exitLocked = false;
    }
}
