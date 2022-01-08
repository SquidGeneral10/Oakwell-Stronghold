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

    public GameObject purpleStar;
    public GameObject goldStar;
    public GameObject silverStar;

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

        if(countdown <= 120f)
        { purpleStar.SetActive(false); }
        
        if(countdown <= 60f)
        { goldStar.SetActive(false); }
    }
}