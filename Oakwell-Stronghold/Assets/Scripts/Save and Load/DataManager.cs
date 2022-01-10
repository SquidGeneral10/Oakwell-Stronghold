#region
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEditor;
#endregion

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    #region Publics 
    public int Level1Stars { get; set; } = 0;
    public int Level2Stars { get; set; } = 0;
    public int Level3Stars { get; set; } = 0;
    public int Level4Stars { get; set; } = 0;
    public int Level5Stars { get; set; } = 0;

    public bool Level1Complete { get; set; }
    public bool Level2Complete { get; set; }
    public bool Level3Complete { get; set; }
    public bool Level4Complete { get; set; }
    public bool Level5Complete { get; set; }

    #endregion

    public void Awake()
    {
        if (Instance != null) // if an instance of the datamanager already exists
        { Destroy(gameObject); } // destroy it
        else
        { Instance = this; } // and make this one the real slim shady. makes sure the damn thing works, and stops the game from making more when a new scene's loaded.

        DontDestroyOnLoad(gameObject);
        LoadGame();
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveData.dat");

        PlayerData data = new PlayerData(Level1Stars, Level2Stars, Level3Stars, Level4Stars, Level5Stars, Level1Complete, Level2Complete, Level3Complete, Level4Complete, Level5Complete);
        
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/saveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveData.dat", FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);

            Level1Stars = data.Level1Stars;
            Level2Stars = data.Level2Stars;
            Level3Stars = data.Level3Stars;
            Level4Stars = data.Level4Stars;
            Level5Stars = data.Level5Stars;

            Level1Complete = data.Level1Complete;
            Level2Complete = data.Level2Complete;
            Level3Complete = data.Level3Complete;
            Level4Complete = data.Level4Complete;
            Level5Complete = data.Level5Complete;

            file.Close();
        }
    }    
}

[Serializable]
public class PlayerData
{
    #region Star counters

    /// <summary>
    /// These ints are used in LevelLoader.cs to show stars (unlocked via Timer.cs) on the level select screen.
    /// </summary>

    private int level1Stars;
    private int level2Stars;
    private int level3Stars;
    private int level4Stars;
    private int level5Stars;

    public int Level1Stars => level1Stars;
    public int Level2Stars => level2Stars;
    public int Level3Stars => level3Stars;
    public int Level4Stars => level4Stars;
    public int Level5Stars => level5Stars;

    #endregion

    #region Completion bools

    /// <summary>
    /// These bool-y bad boys are used in LevelLoader.cs to show the next level on the level select screen when the previous level has been completed.
    /// </summary>

    private bool level1Complete;
    private bool level2Complete;
    private bool level3Complete;
    private bool level4Complete;
    private bool level5Complete;

    public bool Level1Complete => level1Complete;
    public bool Level2Complete => level2Complete;
    public bool Level3Complete => level3Complete;
    public bool Level4Complete => level4Complete;
    public bool Level5Complete => level5Complete;

    #endregion

    public PlayerData(int l1s, int l2s, int l3s, int l4s, int l5s, bool l1c, bool l2c, bool l3c, bool l4c, bool l5c)
    {
        level1Stars = l1s;
        level2Stars = l2s;
        level3Stars = l3s;
        level4Stars = l4s;
        level5Stars = l5s;

        level1Complete = l1c;
        level2Complete = l2c;
        level3Complete = l3c;
        level4Complete = l4c;
        level5Complete = l5c;
    }
}