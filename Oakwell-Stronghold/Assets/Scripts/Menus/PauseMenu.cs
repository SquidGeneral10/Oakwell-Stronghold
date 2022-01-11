#region 'Using' information
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion

public class PauseMenu : MonoBehaviour
{
    public Timer timer; // used for triggering a game over in lvl 1
    public Timer2 timer2; // used for triggering a game over in lvl 2
    public Timer3 timer3; // used for triggering a game over in lvl 3
    public Timer4 timer4; // used for triggering a game over in lvl 4
    public Timer5 timer5; // used for triggering a game over in lvl 5

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject gameOverUI;
    public GameObject levelCompleteUI;

    public ExitDoor exitConditions;

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

        if (timer.countdown <= 0f || timer2.countdown <= 0f || timer3.countdown <= 0f || timer4.countdown <= 0f || timer5.countdown <= 0f)
        { GameOver(); } // Game over when any timer hits 0

        if(exitConditions.reachedExit == true)
        { LevelComplete(); } // Runs when the player touches the openDoor object.
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

    public void GameOver() 
    {
        GameIsPaused = true;
        Cursor.visible = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        gameOverUI.SetActive(true);
    }

    public void LevelComplete()
    {
        GameIsPaused = true;
        Cursor.visible = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        levelCompleteUI.SetActive(true);
    }

    public void Retry()
    {
        GameIsPaused = false; 
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BacktoMenu()
    {
        GameIsPaused = false;
        Resume();
        Debug.Log("Back to menu!"); // Prints 'Back to menu!' if testing this in the editor.
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        GameIsPaused = false;
        Debug.Log("NEXT!"); // Prints a message if testing this in the editor.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Loads the next scene in the editor - level 5 just shouldn't have this button.
    }
}
