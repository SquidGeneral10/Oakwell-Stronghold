#region
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
#endregion

public class Score : MonoBehaviour
{
    static Score instance;

    public Enemy enemy1;
    public Enemy enemy2;
    public Enemy enemy3;
    int skeletonsSaved = 0;
    public TMP_Text scoreUI;

    bool scoreIncrease1;
    bool scoreIncrease2;
    bool scoreIncrease3;

    void Awake()
    { instance = this; }

    void Update()
    {
        #region Score increases

        if (!scoreIncrease1)
        {
            if (enemy1.Boned)
            {
                scoreIncrease1 = true;
                skeletonsSaved++;
            }
        }

        if (!scoreIncrease2)
        {
            if (enemy2.Boned)
            {
                scoreIncrease2 = true;
                skeletonsSaved++;
            }
        }

        if (!scoreIncrease3)
        {
            if (enemy3.Boned)
            {
                scoreIncrease3 = true;
                skeletonsSaved++;
            }
        }

        #endregion

        scoreUI.text = skeletonsSaved + " / 3";  
    }
}
