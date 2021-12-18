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
    float countdown = 302.0f;
    static bool active;
    public TMP_Text timerUI;

    void Awake()
    {
        instance = this;
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

        if (countdown < 0)
        {
            StartCoroutine("LoadNewScene"); // Loads the main menu when timer hits 0
        }
    }

    void LoadNewScene()
    {
        SceneManager.LoadScene(0);
    }
}