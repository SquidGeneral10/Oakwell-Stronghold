#region 'Using' information
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
#endregion

public class Timer : MonoBehaviour
{
    static Timer instance;
    public float countdown = 180.0f; // Every level will count down from 3 minutes. CHANGE IN EDITOR.
    static bool active;
    public float transitionTime = 1f;
    public TMP_Text timerUI;
    public AudioSource starLost;
    public AudioClip starBloop;
    private bool alreadyPlayedA = false;
    private bool alreadyPlayedB = false;

    public GameObject purpleStarA;
    public GameObject goldStarA;
    public GameObject silverStarA;

    public GameObject purpleStarB;
    public GameObject goldStarB;
    public GameObject silverStarB;

    public DataManager dataManager;

    void Awake()
    { instance = this; }

    private void Start()
    {
        dataManager.Load();

    }

    void Update()
    {
        if (countdown > 0) // Gradually counts down until reaching 0.
        countdown -= Time.deltaTime;

        int displayTimer = Mathf.CeilToInt(countdown);

        string minutes = ((int)displayTimer / 60).ToString();
        string seconds = (displayTimer % 60).ToString("00");
        double b = System.Math.Round(countdown, 2);

        timerUI.text = minutes + ":" + seconds;

        if(countdown <= 119f)
        {
            purpleStarA.SetActive(false); // Removes the purple star that's always visible
            purpleStarB.SetActive(false); // Removes the purple star on the LevelComplete UI

            if (!alreadyPlayedA) // only plays this sound effect once. removing this will cause audio spam.
            {
                starLost.PlayOneShot(starBloop);
                alreadyPlayedA = true;
            }
        }
        
        if(countdown <= 59f)
        {
            goldStarA.SetActive(false); // Removes the gold star that's always visible
            goldStarB.SetActive(false); // Removes the gold star on the LevelComplete UI

            if (!alreadyPlayedB) // only plays this sound effect once. removing this will cause audio spam.
            {
                starLost.PlayOneShot(starBloop);
                alreadyPlayedB = true;
            }
        }
    }
}