using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Papers")]
    public int papersNeeded = 5;
    public int papersCollected = 0;
    public Text papersText;

    [Header("Timer")]
    public float levelTime = 180f; // 3 minutes
    private float remainingTime;
    public Text timerText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // For now, we can keep it scene-only
        // DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        remainingTime = levelTime;
        UpdatePapersUI();
        UpdateTimerUI();
    }

    void Update()
    {
        if (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime < 0f) remainingTime = 0f;
            UpdateTimerUI();

            if (remainingTime <= 0f)
            {
                OnTimeUp();
            }
        }
    }

    void UpdatePapersUI()
    {
        if (papersText != null)
        {
            papersText.text = $"Papers: {papersCollected}/{papersNeeded}";
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(remainingTime);
            int minutes = seconds / 60;
            int secs = seconds % 60;
            timerText.text = $"{minutes:00}:{secs:00}";
        }
    }

    public void CollectPaper()
    {
        papersCollected++;
        UpdatePapersUI();

        if (papersCollected >= papersNeeded)
        {
            OnAllPapersCollected();
        }
    }

    void OnAllPapersCollected()
    {
        Debug.Log("All papers collected! Tutorial complete.");
        // Later: show UI / change scene
    }

    void OnTimeUp()
    {
        Debug.Log("Time up!");
        // Later: show fail screen / restart
    }
}