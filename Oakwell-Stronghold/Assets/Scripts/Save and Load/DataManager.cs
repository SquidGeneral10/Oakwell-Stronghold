#region
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#endregion

public class DataManager : MonoBehaviour
{

    public static DataManager Instance { get; private set; }

    private void Awake()
    {
        //Check if an instance of the singleton already exists
        //If so, destroy the new instance object
        //If not, set this object to be the singleton
        if (Instance != null)
        { Destroy(gameObject); }

        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Load();
        }
    }
    public PlayerData data;
    private string file = "player.txt";

    public void Save()
    {
        string json = JsonUtility.ToJson(data);
        WriteToFile(file, json);
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
