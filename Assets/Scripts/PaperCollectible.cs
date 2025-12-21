using UnityEngine;

public class PaperCollectible : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip pickupSound;
    public float pickupVolume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Paper collected!");

            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupVolume);
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.CollectPaper();
            }

            gameObject.SetActive(false);
        }
    }
}
