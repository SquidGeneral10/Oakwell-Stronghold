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

    public void LoadLevel1()
    { StartCoroutine(LoadLevel(1)); }

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
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

    public void BacktoMenu()
    {
        StartCoroutine(LoadLevel(0));
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!"); // Prints 'QUIT' if testing this in the editor.
        Application.Quit(); // Closes the game.
    }
}
