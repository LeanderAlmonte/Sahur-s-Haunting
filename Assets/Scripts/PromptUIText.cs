using UnityEngine;
using UnityEngine.UI;

public class PromptUI : MonoBehaviour
{
    public static PromptUI Instance;

    [SerializeField] private Text promptText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void Show(string msg)
    {
        if (promptText == null) return;
        promptText.text = msg;
        promptText.gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (promptText == null) return;
        promptText.gameObject.SetActive(false);
    }
}
