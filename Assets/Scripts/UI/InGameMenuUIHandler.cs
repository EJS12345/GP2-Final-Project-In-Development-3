using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuUIHandler : MonoBehaviour
{
    Canvas canvas;
    bool isMenuVisible = false;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        GameManager.instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void OnRaceAgain()
    {
        GameManager.instance.ResetGameState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnExitToMainMenu()
    {
        GameManager.instance.ResetGameState();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    private void ToggleMenu()
    {
        isMenuVisible = !isMenuVisible;
        canvas.enabled = isMenuVisible;

        if (isMenuVisible)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    IEnumerator ShowMenuCO()
    {
        yield return new WaitForSeconds(1);
        canvas.enabled = true;
    }

    void OnGameStateChanged(GameManager gameManager)
    {
        if (GameManager.instance.GetGameState() == GameStates.raceOver)
        {
            StartCoroutine(ShowMenuCO());
        }
    }

    void OnDestroy()
    {
        GameManager.instance.OnGameStateChanged -= OnGameStateChanged;
    }
}







