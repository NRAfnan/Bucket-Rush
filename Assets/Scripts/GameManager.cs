using UnityEngine;

public class GameManager : MonoBehaviour
{
    // The static global point of contact (Singleton)
    public static GameManager Instance { get; private set; }

    [Header("Game States")]
    public bool isGameStarted = false;
    public bool isGamePaused = false;
    public bool isGameOver = false;

    [Header("Scoring System")]
    public int score = 0;
    public int highScore = 0;

    private PlayerVariables playerVars;

    void Awake()
    {
        // Enforce that only one instance of the GameManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Make sure time scale is normal when a brand new session kicks off
        Time.timeScale = 1f;
        playerVars = FindFirstObjectByType<PlayerVariables>();
        Debug.Log("Press 'Enter' to start a clean game, or press '1', '2', '3' to load a profile.");
    }

    void Update()
    {
        // --- PRE-GAME STATE INPUTS (Loading Only) ---
        if (!isGameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Return)) StartCleanGame();
            else if (Input.GetKeyDown(KeyCode.Alpha1)) ExecuteLoadSequence(1);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) ExecuteLoadSequence(2);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) ExecuteLoadSequence(3);
            return;
        }

        // --- ACTIVE GAMEPLAY STATE INPUTS (Pausing & Saving) ---
        if (isGameStarted && !isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space)) TogglePause();
            else if (Input.GetKeyDown(KeyCode.Alpha1)) ExecuteSaveSequence(1);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) ExecuteSaveSequence(2);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) ExecuteSaveSequence(3);
        }
    }


    void StartCleanGame()
    {
        isGameStarted = true;
        Time.timeScale = 1f;
        Debug.Log("Game Started Fresh!");
    }


    void TogglePause()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Time.timeScale = 0f; // Freezes physics, velocities, and Time.deltaTime
            Debug.Log("Game Paused! Press 'Space' to resume.");
        }
        else
        {
            Time.timeScale = 1f; // Restores full game speed simulation
            Debug.Log("Game Resumed!");
        }
    }

    public void AddScore(int points)
    {
        if (isGameOver || isGamePaused || !isGameStarted) return;

        score += points;

        if (score > highScore)
        {
            highScore = score;
        }

        Debug.Log($"Diamond Collected! Score: {score}");
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f; // Freeze everything cleanly when a game over screen triggers
        Debug.Log("Game Over! The diamond fell out of bounds.");
    }

    public void ResetGame()
    {
        score = 0;
        isGameOver = false;
        isGamePaused = false;
        isGameStarted = false;

        Time.timeScale = 1f; // Reset simulation scale back to normal
        Debug.Log("Game Loop Restarted! Press 'Enter' to start the game.");
    }


    // --- DECOUPLED SAVE/LOAD ROUTINES ---

    private void ExecuteSaveSequence(int profileIndex)
    {
        if (playerVars == null) return;

        // 1. Gather active data values into the player runtime container
        playerVars.UpdatePositionData();
        playerVars.data.savedScore = score;
        playerVars.data.savedHighScore = highScore;

        // 2. Pass the data struct over to our independent SaveSystem to handle disk writing
        SaveSystem.SaveProfile(profileIndex, playerVars.data);
    }

    private void ExecuteLoadSequence(int profileIndex)
    {
        // 1. Request the data structure from our utility script
        PlayerData loadedData = SaveSystem.LoadProfile(profileIndex);

        // 2. If valid data was found, populate our live scene parameters
        if (loadedData != null)
        {
            if (playerVars == null) playerVars = FindFirstObjectByType<PlayerVariables>();

            playerVars.data = loadedData;
            score = playerVars.data.savedScore;
            highScore = playerVars.data.savedHighScore;
            playerVars.ApplyLoadedData();

            // 3. Force state transitions and pause the game scale
            isGameStarted = true;
            isGamePaused = true;
            Time.timeScale = 0f;

            Debug.Log($"Profile {profileIndex} loaded! Game is currently PAUSED. Press 'Space' to play.");
        }
    }
}