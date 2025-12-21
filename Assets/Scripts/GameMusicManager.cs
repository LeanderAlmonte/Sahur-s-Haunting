using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMusicManager : MonoBehaviour
{
    [Serializable]
    public class SceneMusic
    {
        public string sceneName;
        public AudioClip musicClip;
    }

    public static GameMusicManager Instance { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Scene Music")]
    [SerializeField] private List<SceneMusic> sceneMusic = new List<SceneMusic>();

    [Header("SFX")]
    [SerializeField] private AudioClip levelCompleteClip;

    private bool isPaused;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource == null || sfxSource == null)
        {
            Debug.LogError("[GameMusicManager] Assign BOTH musicSource and sfxSource in Inspector.");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // In case we start already inside a scene
        PlayMusicForScene(SceneManager.GetActiveScene().name, restart: true);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Always unpause music after scene load (avoids weird stuck pause states)
        SetMusicPaused(false);

        // Restart music on restart / scene load
        PlayMusicForScene(scene.name, restart: true);
    }

    private void PlayMusicForScene(string sceneName, bool restart)
    {
        AudioClip target = null;

        for (int i = 0; i < sceneMusic.Count; i++)
        {
            if (sceneMusic[i] != null && sceneMusic[i].sceneName == sceneName)
            {
                target = sceneMusic[i].musicClip;
                break;
            }
        }

        if (target == null)
        {
            // No music defined for this scene
            if (musicSource != null) musicSource.Stop();
            return;
        }

        if (musicSource == null) return;

        // If same clip and restart requested, restart from beginning
        if (musicSource.clip == target)
        {
            if (restart)
            {
                musicSource.Stop();
                musicSource.time = 0f;
                musicSource.Play();
            }
            return;
        }

        // Switch clip
        musicSource.Stop();
        musicSource.clip = target;
        musicSource.time = 0f;
        musicSource.Play();
    }

    public void SetMusicPaused(bool paused)
    {
        isPaused = paused;

        if (musicSource == null) return;

        if (paused)
        {
            if (musicSource.isPlaying) musicSource.Pause();
        }
        else
        {
            musicSource.UnPause();
        }
    }

    public void PlayLevelCompleteSfx()
    {
        if (sfxSource == null || levelCompleteClip == null) return;

        // OneShot will keep playing even during scene load because this object survives
        sfxSource.PlayOneShot(levelCompleteClip);
    }
}
