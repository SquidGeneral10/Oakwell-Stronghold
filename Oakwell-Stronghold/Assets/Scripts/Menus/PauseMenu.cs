#region 'Using' information
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion

public class PauseMenu : MonoBehaviour
{
    public Timer timer; // used for triggering a game over
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject gameOverUI;

    private void Awake()
    { Resume(); } // If the player has to retry the level, this Resume makes sure the level isn't started paused.

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            if(GameIsPaused)
            { Resume(); }
            else
            { Pause(); }
        }

        if(timer.countdown <= 0f)
        { GameOver(); } // Game over when timer hits 0
    }

    public void Resume()
    {
        Cursor.visible = false;
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
    }

    public void Pause()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);
    }

    public void GameOver() // TODO: Make a similar copy of this for winning, w/ stars visible - e.g if they did great, gameObject.PurpleStar = true, etc.
    {
        GameIsPaused = true;
        Cursor.visible = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        gameOverUI.SetActive(true);
    }

    public void Retry()
    {
        GameIsPaused = false; 
        Resume();
        SceneManager.LoadScene("Level1");
    }

    public void BacktoMenu()
    {
        GameIsPaused = false;
        Debug.Log("Back to menu!"); // Prints 'Back to menu!' if testing this in the editor.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
