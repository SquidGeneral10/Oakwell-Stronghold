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

    public float Health { get; set; } = 0; // these become the star values
    public float Stamina { get; set; } = 0;
    public float Slurp { get; set; } = 0;

    public bool InDangerZone { get; set; } // becomes level completion bools

    public void Awake()
    {
        if (Instance != null) // if an instance of the singleton already exists
        { Destroy(gameObject); } // destroy it
        else
        { Instance = this; } // and make this one the real slim shady

        DontDestroyOnLoad(gameObject);
        LoadGame();
    }
    public PlayerData data;
    private string file = "player.txt";

    public void Save()
    {
        string json = JsonUtility.ToJson(data);
        WriteToFile(file, json);
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveData.dat");

        PlayerData data = new PlayerData();

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

            file.Close();
        }
    }

    public void Load()
    {
        data = new PlayerData();
        string json = ReadFromFile(file);
        JsonUtility.FromJsonOverwrite(json, data);
    }

    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }

    private string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);

        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
            Debug.LogWarning("FILE NOT FOUND!");
        return "";
    }

    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }
    
}

public class SaveData
{
    #region Star counters
    /// <summary>
    /// These ints are used in LevelLoader.cs to show stars (unlocked via Timer.cs) on the level select screen.
    /// </summary>

    public int level1Stars = 0;
    public int level2Stars = 0;
    public int level3Stars = 0;
    public int level4Stars = 0;
    public int level5Stars = 0;
    #endregion

    #region Completion bools
    /// <summary>
    /// These bool-y bad boys are used in LevelLoader.cs to show the next level on the level select screen when the previous level has been completed.
    /// </summary>

    public bool level1Complete;
    public bool level2Complete;
    public bool level3Complete;
    public bool level4Complete;
    public bool level5Complete;
    #endregion

    public SaveData(int l1s, int l2s, int l3s, int l4s, int l5s, bool l1c, bool l2c, bool l3c, bool l4c, bool l5c)
    {
        l1s = level1Stars;
        l2s = level2Stars;
        l3s = level3Stars;
        l4s = level4Stars;
        l5s = level5Stars;

        l1c = level1Complete;
        l2c = level2Complete;
        l3c = level3Complete;
        l4c = level4Complete;
        l5c = level5Complete;
    }
}