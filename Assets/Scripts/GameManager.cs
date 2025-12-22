using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Papers")]
    public int papersNeeded = 5;
    public int papersCollected = 0;
    public Text papersText;

    [Header("Timer")]
    public float levelTime = 180f;
    private float remainingTime;
    public Text timerText;

    [Header("Fail State")]
    [SerializeField] private string gameOverSceneName = "GameOverScene";

    private bool hasEnded = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        remainingTime = levelTime;
        UpdatePapersUI();
        UpdateTimerUI();
    }

    void Update()
    {
        if (hasEnded) return;

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
            papersText.text = $"Papers: {papersCollected}/{papersNeeded}";
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
        if (hasEnded) return;

        papersCollected++;
        UpdatePapersUI();

        if (papersCollected >= papersNeeded)
            OnAllPapersCollected();
    }

    void OnAllPapersCollected()
    {
        if (hasEnded) return;
        hasEnded = true;

        Debug.Log("All papers collected! Tutorial complete.");

        if (GameMusicManager.Instance != null)
            GameMusicManager.Instance.PlayLevelCompleteSfx();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    void OnTimeUp()
    {
        if (hasEnded) return;
        hasEnded = true;

        Debug.Log("Time up!");

        // In case you were paused when time hits 0
        Time.timeScale = 1f;

        SceneManager.LoadScene(gameOverSceneName);
    }
}
