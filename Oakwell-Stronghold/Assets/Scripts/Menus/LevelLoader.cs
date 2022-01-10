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
    public PlayerData playerData;
    public GameObject congratulations;

    #region Star objects
    /// <summary>
    /// L = Level, A = Purple, B = Gold, C = Silver
    /// </summary>
    public GameObject L1SA; // the joyful
    public GameObject L1SB; 
    public GameObject L1SC;

    public GameObject L2SA;
    public GameObject L2SB;
    public GameObject L2SC;

    public GameObject L3SA;
    public GameObject L3SB;
    public GameObject L3SC;

    public GameObject L4SA; // beam. pew pew
    public GameObject L4SB;
    public GameObject L4SC;

    public GameObject L5SA;
    public GameObject L5SB;
    public GameObject L5SC;

    #endregion

    #region Level buttons

    public GameObject Level2;
    public GameObject Level3;
    public GameObject Level4;
    public GameObject Level5;

    #endregion

    #region Menu loading methods

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

    #endregion

    private void Start()
    {
        DataManager.Instance.LoadGame();

        #region level 1 scores

        if(DataManager.Instance.Level1Complete == true)
        {
            if (DataManager.Instance.Level1Stars == 3)
            {
                L1SA.SetActive(true);
                L1SB.SetActive(true);
                L1SC.SetActive(true);
            }

            if (DataManager.Instance.Level1Stars == 2)
            {
                L1SB.SetActive(true);
                L1SC.SetActive(true);
            }

            if (DataManager.Instance.Level1Stars == 1)
            {
                L1SC.SetActive(true);
            }
        }


        #endregion

        #region level 2 scores

        if (DataManager.Instance.Level2Complete == true)
        {
            if (DataManager.Instance.Level2Stars == 3)
            {
                L2SA.SetActive(true);
                L2SB.SetActive(true);
                L2SC.SetActive(true);
            }

            if (DataManager.Instance.Level2Stars == 2)
            {
                L2SB.SetActive(true);
                L2SC.SetActive(true);
            }

            if (DataManager.Instance.Level2Stars == 1)
            {
                L2SC.SetActive(true);
            }
        }



        #endregion

        #region level 3 scores

        if (DataManager.Instance.Level3Complete == true)
        {
            if (DataManager.Instance.Level3Stars == 3)
            {
                L3SA.SetActive(true);
                L3SB.SetActive(true);
                L3SC.SetActive(true);
            }

            if (DataManager.Instance.Level3Stars == 2)
            {
                L3SB.SetActive(true);
                L3SC.SetActive(true);
            }

            if (DataManager.Instance.Level3Stars == 1)
            {
                L3SC.SetActive(true);
            }
        }



        #endregion

        #region level 4 scores

        if (DataManager.Instance.Level4Complete == true)
        {
            if (DataManager.Instance.Level4Stars == 3)
            {
                L4SA.SetActive(true);
                L4SB.SetActive(true);
                L4SC.SetActive(true);
            }

            if (DataManager.Instance.Level4Stars == 2)
            {
                L4SB.SetActive(true);
                L4SC.SetActive(true);
            }

            if (DataManager.Instance.Level4Stars == 1)
            {
                L4SC.SetActive(true);
            }
        }

        #endregion

        #region level 5 scores

        if (DataManager.Instance.Level1Complete == true)
        {
            if (DataManager.Instance.Level5Stars == 3)
            {
                L5SA.SetActive(true);
                L5SB.SetActive(true);
                L5SC.SetActive(true);
            }

            if (DataManager.Instance.Level5Stars == 2)
            {
                L5SB.SetActive(true);
                L5SC.SetActive(true);
            }

            if (DataManager.Instance.Level5Stars == 1)
            {
                L5SC.SetActive(true);
            }
        }

        #endregion

        #region Level select conditions

        if(DataManager.Instance.Level1Complete)
        { Level2.SetActive(true); } // After beating level 1, Level 2 appears on the level select menu.

        if (DataManager.Instance.Level2Complete)
        { Level3.SetActive(true); } // After beating level 2, Level 3 appears on the level select menu.

        if (DataManager.Instance.Level3Complete)
        { Level4.SetActive(true); } // After beating level 3, Level 4 appears on the level select menu.

        if (DataManager.Instance.Level4Complete)
        { Level5.SetActive(true); } // After beating level 4, Level 5 appears on the level select menu.

        if(DataManager.Instance.Level5Complete)
        { congratulations.SetActive(true); }

        #endregion
    }
}