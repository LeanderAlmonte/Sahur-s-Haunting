using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    public Transform hidePosition;

    private bool playerNearby = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.H))
        {
            if (HidingSystem.Instance != null && hidePosition != null)
            {
                HidingSystem.Instance.ToggleHide(hidePosition);
            }
        }
    }
}
