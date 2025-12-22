using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LockerHidingSpot : MonoBehaviour
{
    [Header("Locker setup")]
    public Transform hidePosition;     // inside locker
    public Transform exitPosition;     // in front of locker (optional but recommended)

    [Header("Prompt")]
    public string promptMessage = "Press H to hide/unhide";
    public KeyCode hideKey = KeyCode.H;

    [Header("Safe locker")]
    public bool isTrap = false;
    public GameObject safeOverlay;     // locker dark overlay (not your table overlay)

    [Header("Trap locker QTE")]
    public GameObject trapOverlay;         // red overlay panel root
    public CanvasGroup trapCanvasGroup;    // add CanvasGroup on trapOverlay
    public Text trapText;                  // UI text like "TRAP! Mash SPACE!"
    public Slider qteSlider;               // optional but makes it clear
    public float qteDuration = 3f;
    public int requiredPresses = 7;
    public string gameOverSceneName = "GameOverScene";

    private bool playerNearby = false;
    private bool qteRunning = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerNearby = true;

        if (!qteRunning && PromptUI.Instance != null)
            PromptUI.Instance.Show(promptMessage);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerNearby = false;

        if (PromptUI.Instance != null)
            PromptUI.Instance.Hide();
    }

    void Update()
    {
        if (!playerNearby) return;
        if (qteRunning) return;
        if (HidingSystem.Instance == null) return;

        if (Input.GetKeyDown(hideKey))
        {
            if (!isTrap)
            {
                // Safe locker hide toggle
                HidingSystem.Instance.ToggleHide(hidePosition, safeOverlay, null);
            }
            else
            {
                // Trap locker QTE
                StartCoroutine(TrapRoutine());
            }
        }
    }

    private IEnumerator TrapRoutine()
    {
        qteRunning = true;

        if (PromptUI.Instance != null)
            PromptUI.Instance.Hide();

        // Enter hide, but lock exit
        HidingSystem.Instance.SetExitLocked(true);
        HidingSystem.Instance.EnterHide(hidePosition, trapOverlay, null);

        // Setup trap UI
        if (trapOverlay != null) trapOverlay.SetActive(true);

        if (trapCanvasGroup != null)
            trapCanvasGroup.alpha = 0.15f;

        if (trapText != null)
            trapText.text = "TRAP! Mash SPACE to escape!";

        int presses = 0;

        if (qteSlider != null)
        {
            qteSlider.minValue = 0;
            qteSlider.maxValue = requiredPresses;
            qteSlider.value = 0;
        }

        float t = 0f;

        while (t < qteDuration)
        {
            t += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                presses++;
                if (qteSlider != null) qteSlider.value = presses;

                if (presses >= requiredPresses)
                    break;
            }

            // Fade red stronger over time
            if (trapCanvasGroup != null)
            {
                float a = Mathf.Lerp(0.15f, 0.9f, t / qteDuration);
                trapCanvasGroup.alpha = a;
            }

            yield return null;
        }

        if (presses >= requiredPresses)
        {
            // Success: escape
            HidingSystem.Instance.SetExitLocked(false);

            if (trapOverlay != null) trapOverlay.SetActive(false);

            // Spit player out in front so they can run
            HidingSystem.Instance.ExitHide(exitPosition);

            qteRunning = false;
            yield break;
        }

        // Fail: die
        if (trapCanvasGroup != null) trapCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene(gameOverSceneName);
    }
}
