using System.Collections;
using UnityEngine;
using TMPro; // Import the TextMeshPro namespace

public class RaceTimeUIHandler : MonoBehaviour
{
    // Change the type to TextMeshProUGUI to work with TextMeshPro
    TextMeshProUGUI timeText;

    float lastRaceTimeUpdate = 0;

    private void Awake()
    {
        // Ensure the component is of type TextMeshProUGUI
        timeText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        // Subscribe to game state changes to start/stop the timer accordingly
        GameManager.instance.OnGameStateChanged += OnGameStateChanged;

        // Initialize the timer display
        UpdateTimeDisplay(0);
    }

    private void OnDestroy()
    {
        // Unsubscribe from game state changes when this object is destroyed
        GameManager.instance.OnGameStateChanged -= OnGameStateChanged;
    }

    // Called when the game state changes in GameManager
    private void OnGameStateChanged(GameManager gameManager)
    {
        if (gameManager.GetGameState() == GameStates.running)
        {
            // Start updating the race time display when the race begins
            StartCoroutine(UpdateTimeCO());
        }
        else
        {
            // Stop updating the timer when the game is not in "running" state
            StopAllCoroutines();
            lastRaceTimeUpdate = 0; // Reset the last update time
        }
    }

    IEnumerator UpdateTimeCO()
    {
        while (GameManager.instance.GetGameState() == GameStates.running)
        {
            float raceTime = GameManager.instance.GetRaceTime();

            if (lastRaceTimeUpdate != raceTime)
            {
                // Calculate minutes and seconds
                int raceTimeMinutes = (int)Mathf.Floor(raceTime / 60);
                int raceTimeSeconds = (int)Mathf.Floor(raceTime % 60);

                // Update the time display with formatted text
                UpdateTimeDisplay(raceTime);

                lastRaceTimeUpdate = raceTime;
            }

            // Update the display every 0.1 seconds
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void UpdateTimeDisplay(float raceTime)
    {
        int raceTimeMinutes = (int)Mathf.Floor(raceTime / 60);
        int raceTimeSeconds = (int)Mathf.Floor(raceTime % 60);
        timeText.text = $"{raceTimeMinutes.ToString("00")}:{raceTimeSeconds.ToString("00")}";
    }
}


