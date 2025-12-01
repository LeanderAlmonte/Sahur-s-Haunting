using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public KeyCode pauseKey = KeyCode.P;
    private bool isPaused;

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}

//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class PauseManager : MonoBehaviour
//{
//    public GameObject pauseMenuPanel;
//    public KeyCode pauseKey = KeyCode.P;

//    private bool isPaused;

//    void Start()
//    {
//        // Make sure pause menu starts hidden
//        if (pauseMenuPanel != null)
//            pauseMenuPanel.SetActive(false);
//    }

//    void Update()
//    {
//        if (Input.GetKeyDown(pauseKey))
//        {
//            TogglePause();
//        }
//    }

//    public void TogglePause()
//    {
//        isPaused = !isPaused;

//        if (pauseMenuPanel != null)
//            pauseMenuPanel.SetActive(isPaused);

//        Time.timeScale = isPaused ? 0f : 1f;

//        Cursor.visible = isPaused;
//        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
//    }

//    public void ResumeGame()
//    {
//        isPaused = false;

//        if (pauseMenuPanel != null)
//            pauseMenuPanel.SetActive(false);

//        Time.timeScale = 1f;
//        Cursor.visible = false;
//        Cursor.lockState = CursorLockMode.Locked;
//    }

//    public void QuitToMainMenu()
//    {
//        Time.timeScale = 1f;
//        Cursor.visible = true;
//        Cursor.lockState = CursorLockMode.None;

//        SceneManager.LoadScene("MainMenu");
//    }
//}
