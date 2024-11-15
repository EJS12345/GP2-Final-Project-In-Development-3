using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates { countDown, running, raceOver };

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    GameStates gameState = GameStates.countDown;
    float raceStartedTime = 0;
    float raceCompletedTime = 0;
    List<DriverInfo> driverInfoList = new List<DriverInfo>();

    public event Action<GameManager> OnGameStateChanged;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeDrivers();
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        Debug.Log("Starting countdown...");
        gameState = GameStates.countDown;
        yield return new WaitForSeconds(3);  // 3-second countdown before the race starts
        OnRaceStart();
    }

    public GameStates GetGameState()
    {
        return gameState;
    }

    void ChangeGameState(GameStates newGameState)
    {
        if (gameState != newGameState)
        {
            gameState = newGameState;
            OnGameStateChanged?.Invoke(this);
            Debug.Log($"Game state changed to: {gameState}");
        }
    }

    public float GetRaceTime()
    {
        if (gameState == GameStates.countDown)
            return 0;
        if (gameState == GameStates.raceOver)
            return raceCompletedTime - raceStartedTime;
        return Time.time - raceStartedTime;
    }

    public void ClearDriversList()
    {
        driverInfoList.Clear();
    }

    public void AddDriverToList(int playerNumber, string name, int carUniqueID, bool isAI)
    {
        driverInfoList.Add(new DriverInfo(playerNumber, name, carUniqueID, isAI));
    }

    public void SetDriversLastRacePosition(int playerNumber, int position)
    {
        DriverInfo driverInfo = FindDriverInfo(playerNumber);
        driverInfo.lastRacePosition = position;
    }

    public void AddPointsToChampionship(int playerNumber, int points)
    {
        DriverInfo driverInfo = FindDriverInfo(playerNumber);
        driverInfo.championshipPoints += points;
    }

    DriverInfo FindDriverInfo(int playerNumber)
    {
        foreach (DriverInfo driverInfo in driverInfoList)
        {
            if (playerNumber == driverInfo.playerNumber)
                return driverInfo;
        }

        Debug.LogError($"FindDriverInfo failed for player number {playerNumber}");
        return null;
    }

    public List<DriverInfo> GetDriverList()
    {
        return driverInfoList;
    }

    public void OnRaceStart()
    {
        Debug.Log("OnRaceStart called. Race begins.");
        raceStartedTime = Time.time;
        ChangeGameState(GameStates.running);
    }

    public void OnRaceCompleted()
    {
        Debug.Log("OnRaceCompleted called. Race over.");
        raceCompletedTime = Time.time;
        ChangeGameState(GameStates.raceOver);
    }

    public void ResetGameState()
    {
        gameState = GameStates.countDown;
        raceStartedTime = 0;
        raceCompletedTime = 0;
        InitializeDrivers();
        StartCoroutine(StartCountdown());
        Debug.Log("Game state reset to countDown.");
    }

    private void InitializeDrivers()
    {
        ClearDriversList();
        AddDriverToList(1, "P1", 0, false);  // Initialize Player 1
        AddDriverToList(2, "P2", 1, false);  // Initialize Player 2
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetGameState();
    }
}










