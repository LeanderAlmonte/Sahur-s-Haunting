using UnityEngine;

public class HidingSystem : MonoBehaviour
{
    public static HidingSystem Instance;

    public GameObject hideOverlay; // assign from Canvas

    private CharacterController controller;
    private bool isHiding = false;
    private Vector3 returnPosition;

    [Header("Audio")]
    public AudioSource hideAudio;  // assign in inspector

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

    public void ToggleHide(Transform hidePosition)
    {
        if (!isHiding)
        {
            // Enter hiding
            returnPosition = transform.position;
            if (controller != null) controller.enabled = false;

            transform.position = hidePosition.position;

            if (hideOverlay != null)
                hideOverlay.SetActive(true);

            if (hideAudio != null && !hideAudio.isPlaying)
            {
                hideAudio.Play();
            }

            isHiding = true;
        }
        else
        {
            // Exit hiding
            transform.position = returnPosition;
            if (controller != null) controller.enabled = true;

            if (hideOverlay != null)
                hideOverlay.SetActive(false);

            if (hideAudio != null && hideAudio.isPlaying)
            {
                hideAudio.Stop();
            }

            isHiding = false;
        }
    }
}
