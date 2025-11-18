using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    public string levelName;
    public bool isTutorialLevel = false;
    
    [Header("Spawn Points")]
    public Transform playerSpawnPoint;
    public Transform[] sahurSpawnPoints;
    
    [Header("Level Objectives")]
    public int totalPapersToCollect = 5;
    public Transform[] paperSpawnPoints;
    
    [Header("Level Transitions")]
    public string nextLevelName;
    public GameObject levelCompleteUI;
    
    private int papersCollected = 0;
    private GameObject player;
    private MenuManager menuManager;
    
    void Start()
    {
        // Find player and menu manager
        player = GameObject.FindGameObjectWithTag("Player");
        menuManager = FindObjectOfType<MenuManager>();
        
        // Set up level
        SpawnPlayer();
        SpawnSahur();
        SetupLevelObjectives();
        
        // Lock cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void SpawnPlayer()
    {
        if (player != null && playerSpawnPoint != null)
        {
            player.transform.position = playerSpawnPoint.position;
            player.transform.rotation = playerSpawnPoint.rotation;
        }
    }
    
    void SpawnSahur()
    {
        GameObject sahurPrefab = Resources.Load<GameObject>("Prefabs/SahurNPC");
        if (sahurPrefab != null && sahurSpawnPoints.Length > 0)
        {
            int randomSpawn = Random.Range(0, sahurSpawnPoints.Length);
            Instantiate(sahurPrefab, sahurSpawnPoints[randomSpawn].position, sahurSpawnPoints[randomSpawn].rotation);
        }
    }
    
    void SetupLevelObjectives()
    {
        if (isTutorialLevel)
        {
            // This is for Ibrahim's paper collection system
            Debug.Log($"Tutorial Level: Collect {totalPapersToCollect} papers to complete");
        }
    }
    
    public void CollectPaper()
    {
        papersCollected++;
        Debug.Log($"Papers collected: {papersCollected}/{totalPapersToCollect}");
        
        if (papersCollected >= totalPapersToCollect)
        {
            CompleteLevel();
        }
    }
    
    void CompleteLevel()
    {
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        // Unlock next level or return to menu
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            Invoke("LoadNextLevel", 3f);
        }
    }
    
    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }
    
    public void PlayerCaught()
    {
        // Game over logic
        if (menuManager != null)
        {
            menuManager.ShowGameOver();
        }
    }
}