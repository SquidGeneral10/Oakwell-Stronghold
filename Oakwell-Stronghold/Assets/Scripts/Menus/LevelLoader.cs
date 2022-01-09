#region 'using' information
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public void BacktoMenu()
    { StartCoroutine(LoadLevel(0)); } // Make sure the buildindex has the main menu has its '0th' scene.

    public void LoadLevel1()
    { StartCoroutine(LoadLevel(1)); } // Ditto for all of these - the number should correspond to the level.

    public void LoadLevel2()
    { StartCoroutine(LoadLevel(2)); }

    public void LoadLevel3()
    { StartCoroutine(LoadLevel(3)); }

    public void LoadLevel4()
    { StartCoroutine(LoadLevel(4)); }

    public void LoadLevel5()
    { StartCoroutine(LoadLevel(5)); }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start"); // Sets off the scene transition animation.
        yield return new WaitForSeconds(transitionTime); // Waits for the animation to finish before going to a new level
        SceneManager.LoadScene(levelIndex); // Loads the scene in the build index that has the number the coroutines had in them.
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!"); // Prints 'QUIT' if testing this in the editor.
        Application.Quit(); // Closes the game.
    }
}