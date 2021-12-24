#region 'using' information
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using TMPro;
using Player;
#endregion

public class TheManager : MonoBehaviour
{
    public static TheManager Instance { get; private set; }

    public int Score { get; set; }
    public bool HasTransitioned { get; set; } = false;
    public float Health { get; set; } = 1;
    public Healthbar healthBar;
    private PlayerHealth health;

    private void Awake()
    {
        // Check if an instance of the singleton already exists
        // If so, destroy the new instance object
        // If not, set this object to be the singleton

        if (Instance != null)
        { Destroy(gameObject); }

        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(@"D:\OAKWELL-STRONGHOLD");

        SaveDataObj data = new SaveDataObj(Health)
        { health = Health };

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(@"D:\OAKWELL-STRONGHOLD\playerSaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(@"C:\Users\2015999\Desktop\playerSaveData.dat", FileMode.Open);

            SaveDataObj data = (SaveDataObj)bf.Deserialize(file);
            Health = data.health;
        }
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(health);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        health.currentHealth = data.health;

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;
    }
}

public class SaveDataObj
{
    public float health;

    public SaveDataObj(float h)
    {
        health = h;
        {
            
        }
    }
}

