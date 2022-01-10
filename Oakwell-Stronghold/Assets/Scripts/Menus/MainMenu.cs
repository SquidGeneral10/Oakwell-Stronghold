#region 'Using' information.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#endregion

public class MainMenu : MonoBehaviour
{
    /*
        Click on the buttons, then add one of the two methods below via the 'OnClicked' part that handles events in the inspector.         
    */

    public GameObject playButton; // play button
    public GameObject contButton; // continue button

    private void Start()
    {
        DataManager.Instance.LoadGame();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if(DataManager.Instance.Level1Complete)
        {
            playButton.SetActive(false); // disables new game button
            contButton.SetActive(true); // enables continue button
            // the actual level the continue button loads must be set by the inspector
        }
    }

    public void QuitGame()
    {
        DataManager.Instance.SaveGame();
        Debug.Log("QUIT!"); // Prints 'QUIT' if testing this in the editor.
        Application.Quit(); // Closes the game.
    }

    public void BacktoMenu()
    { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); }
}