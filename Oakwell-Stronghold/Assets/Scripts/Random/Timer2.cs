#region 'Using' information
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
#endregion

public class Timer2 : MonoBehaviour
{
    static Timer2 instance;

    public float countdown = 180.0f; // Every level will count down from 3 minutes. CHANGE IN EDITOR.
    static bool active;
    public float transitionTime = 1f;
    public TMP_Text timerUI;
    public AudioSource starLost;
    public AudioClip starBloop;
    private bool alreadyPlayedA = false; // makes sure it hasn't already played the lose sound when losing
    private bool alreadyPlayedB = false; // makes sure it hasn't already played the win sound when winning

    #region gameobjects

    /// <summary>
    /// these stars are in the UI in the top right
    /// </summary>
    public GameObject purpleStarA;
    public GameObject goldStarA;
    public GameObject silverStarA;

    /// <summary>
    /// these stars are in the 'level complete' menu
    /// </summary>
    public GameObject purpleStarB;
    public GameObject goldStarB;
    public GameObject silverStarB;

    #endregion

    public DataManager dataManager;
    public PlayerData playerData;

    void Awake()
    { instance = this; }

    void Update()
    {
        if (countdown > 0) // Gradually counts down until reaching 0.
            countdown -= Time.deltaTime;

        int displayTimer = Mathf.CeilToInt(countdown);

        string minutes = ((int)displayTimer / 60).ToString();
        string seconds = (displayTimer % 60).ToString("00");
        double b = System.Math.Round(countdown, 2);

        timerUI.text = minutes + ":" + seconds;

        if (countdown >= 120) // player still has 3 stars til a minute's passed
        {
            DataManager.Instance.Level2Stars = 3;
        }

        if (countdown <= 119f)
        {
            purpleStarA.SetActive(false); // Removes the purple star that's always visible
            purpleStarB.SetActive(false); // Removes the purple star on the LevelComplete UI         

            if (!alreadyPlayedA) // only plays this sound effect / reduces score by one / saves score once.
            {
                starLost.PlayOneShot(starBloop);
                alreadyPlayedA = true;
                DataManager.Instance.Level2Stars = 2; // damn, they down to gold
            }
        }

        if (countdown <= 59f)
        {
            goldStarA.SetActive(false); // Removes the gold star that's always visible
            goldStarB.SetActive(false); // Removes the gold star on the LevelComplete UI

            if (!alreadyPlayedB) // only plays this sound effect once. removing this will cause audio spam.
            {
                starLost.PlayOneShot(starBloop);
                alreadyPlayedB = true;
                DataManager.Instance.Level2Stars = 1; // ZAMN, they down to SILVER
            }
        }
    }
}