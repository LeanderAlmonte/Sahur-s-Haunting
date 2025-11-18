using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject levelSelectPanel;
    public GameObject pauseMenuPanel;
    public GameObject gameOverPanel;
    
    [Header("Buttons")]
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;
    public Button resumeButton;
    public Button mainMenuButton;
    
    [Header("Level Selection")]
    public Button cafeteriaLevelButton;
    public Button lockerLevelButton;
    public Button schoolyardLevelButton;
    
    [Header("Settings")]
    public Slider volumeSlider;
    public Slider sensitivitySlider;
    
    private bool isPaused = false;
    
    void Start()
    {
        // Initialize menu
        ShowMainMenu();
        SetupButtonListeners();
        LoadSettings();
        
        // Unlock cursor for menus
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    void Update()
    {
        // Handle pause menu toggle
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                TogglePause();
            }
        }
    }
    
    void SetupButtonListeners()
    {
        // Main Menu
        if (playButton) playButton.onClick.AddListener(ShowLevelSelect);
        if (settingsButton) settingsButton.onClick.AddListener(ShowSettings);
        if (quitButton) quitButton.onClick.AddListener(QuitGame);
        
        // Pause Menu
        if (resumeButton) resumeButton.onClick.AddListener(ResumeGame);
        if (mainMenuButton) mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        
        // Level Selection
        if (cafeteriaLevelButton) cafeteriaLevelButton.onClick.AddListener(() => LoadLevel("CafeteriaLevel"));
        if (lockerLevelButton) lockerLevelButton.onClick.AddListener(() => LoadLevel("LockerLevel"));
        if (schoolyardLevelButton) schoolyardLevelButton.onClick.AddListener(() => LoadLevel("SchoolyardLevel"));
        
        // Settings
        if (volumeSlider) volumeSlider.onValueChanged.AddListener(SetVolume);
        if (sensitivitySlider) sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
    }
    
    public void ShowMainMenu()
    {
        SetActivePanel(mainMenuPanel);
        Time.timeScale = 1f;
        isPaused = false;
    }
    
    public void ShowLevelSelect()
    {
        SetActivePanel(levelSelectPanel);
    }
    
    public void ShowSettings()
    {
        SetActivePanel(settingsPanel);
    }
    
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    
    public void PauseGame()
    {
        SetActivePanel(pauseMenuPanel);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void ResumeGame()
    {
        SetActivePanel(null);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void LoadLevel(string levelName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelName);
    }
    
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }
    
    public void SetSensitivity(float sensitivity)
    {
        // This will be used by the camera controller
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
    }
    
    void LoadSettings()
    {
        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        float sensitivity = PlayerPrefs.GetFloat("Sensitivity", 2f);
        
        if (volumeSlider) volumeSlider.value = volume;
        if (sensitivitySlider) sensitivitySlider.value = sensitivity;
        
        AudioListener.volume = volume;
    }
    
    void SetActivePanel(GameObject panelToShow)
    {
        // Hide all panels
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(false);
        if (levelSelectPanel) levelSelectPanel.SetActive(false);
        if (pauseMenuPanel) pauseMenuPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
        
        // Show the selected panel
        if (panelToShow != null)
        {
            panelToShow.SetActive(true);
        }
    }
    
    public void ShowGameOver()
    {
        SetActivePanel(gameOverPanel);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}