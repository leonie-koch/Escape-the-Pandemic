using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandlerMenuScene : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject introductionText;
    public Animator startButton;
    public Animator menuItems;
    public Animator dialog;
    public GameObject HintPanel;
    private int prevScene;

    private void Start()
    {
        prevScene = SceneManager.GetActiveScene().buildIndex - 1;
    }
    public void StartGame()
    {
        SceneManager.LoadScene("ConnectScene");
    }

    public void OpenSettings()
    {
        // SceneManager.LoadScene("Settings");
        startButton.SetBool("isHidden", true);
        menuItems.SetBool("isHidden", true);
        dialog.SetBool("isHidden", false);
    }

    public void OpenHints()
    {
        // to be done
    }

    public void CloseSettings()
    {
        User.enteredHighscoreFromMenu = false;
        startButton.SetBool("isHidden", false);
        menuItems.SetBool("isHidden", false);
        dialog.SetBool("isHidden", true);
    }

    public void OpenHighscore()
    {
        // User.enteredHighscoreFromMenu = true;
        SceneManager.LoadScene("Highscore");
    }

    public void ExitGame()
    {
        // This is not working while running the application in the Unity Editor.
        // To test this build and run the game on an actual device
        Highscores.Instance.gameObject.GetComponent<Highscores>().DontEnterHighscore();
        Application.Quit();
    }

    public void BackButtonClicked()
    {
        Debug.Log("back button clicked");
        User.enteredHighscoreFromMenu = false;
        SceneManager.LoadScene(prevScene);
    }

    public void Resume()
    {
        HintPanel.SetActive(false);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
        User.enteredHighscoreFromMenu = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        User.enteredHighscoreFromMenu = true;
    }

    public void closeIntroduction()
    {
        introductionText.SetActive(false);
    }

    public void PlayAgain()
    {
        Highscores.Instance.gameObject.GetComponent<Highscores>().DontEnterHighscore();
        SceneManager.LoadScene("StartScene");
        Destroy(GameObject.Find("Video"));
    }
}
