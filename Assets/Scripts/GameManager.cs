using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject controlsMenuUI;
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void StartNewGame()
    {
        SaveSystem.DeleteSave();
        SceneManager.LoadScene("GameScene"); // Replace with your game scene name
        SaveSystem.ClearCollectedCoins();
        Time.timeScale = 1f;
    }

    public void LoadGame()
    {
        if (SaveSystem.HasSave())
        {
            SceneManager.LoadScene("GameScene");
            Time.timeScale = 1f;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenControls()
    {
        mainMenuUI.SetActive(false);
        controlsMenuUI.SetActive(true);
    }

    public void CloseControls()
    {
        controlsMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }
}
