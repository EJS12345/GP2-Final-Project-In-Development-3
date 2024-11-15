using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUIHandler : MonoBehaviour
{
    public GameObject leaderboardItemPrefab;

    SetLeaderboardItemInfo[] setLeaderboardItemInfo;

    bool isInitialized = false;

    // Other components
    Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false; // Hide initially

        // Hook up events
        GameManager.instance.OnGameStateChanged += OnGameStateChanged;
    }

    // Start is called before the first frame update
    void Start()
    {
        VerticalLayoutGroup leaderboardLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();

        // Get all Car lap counters in the scene
        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();

        // Allocate the array
        setLeaderboardItemInfo = new SetLeaderboardItemInfo[carLapCounterArray.Length];

        // Create the leaderboard items
        for (int i = 0; i < carLapCounterArray.Length; i++)
        {
            // Set the position
            GameObject leaderboardInfoGameObject = Instantiate(leaderboardItemPrefab, leaderboardLayoutGroup.transform);
            setLeaderboardItemInfo[i] = leaderboardInfoGameObject.GetComponent<SetLeaderboardItemInfo>();
            setLeaderboardItemInfo[i].SetPositionText($"{i + 1}.");
        }

        Canvas.ForceUpdateCanvases();
        isInitialized = true;
    }

    public void UpdateList(List<CarLapCounter> lapCounters)
    {
        if (!isInitialized)
            return;

        // Update leaderboard items with driver names
        for (int i = 0; i < lapCounters.Count; i++)
        {
            setLeaderboardItemInfo[i].SetDriverNameText(lapCounters[i].gameObject.name);
        }
    }

    // Event handler for game state changes
    void OnGameStateChanged(GameManager gameManager)
    {
        Debug.Log("Game state changed to: " + GameManager.instance.GetGameState()); // Debug log

        if (GameManager.instance.GetGameState() == GameStates.countDown || GameManager.instance.GetGameState() == GameStates.running)
        {
            // Show leaderboard during countdown and race
            canvas.enabled = true;
        }
        else if (GameManager.instance.GetGameState() == GameStates.raceOver)
        {
            // Keep it visible or add any end-of-race updates if needed
            canvas.enabled = true;
        }
        else
        {
            // Hide leaderboard if not in relevant states
            canvas.enabled = false;
        }
    }

    void OnDestroy()
    {
        // Unhook events
        GameManager.instance.OnGameStateChanged -= OnGameStateChanged;
    }
}





