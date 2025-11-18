using UnityEngine;

public class PaperCollectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Paper collected!"); // TEMP: to see in Console

            if (GameManager.Instance != null)
            {
                GameManager.Instance.CollectPaper();
            }

            gameObject.SetActive(false);
        }
    }
}
