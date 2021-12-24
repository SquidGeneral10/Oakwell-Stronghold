#region 'Using' information
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
#endregion

// using this tutorial: https://youtu.be/XOjd_qU2Ido

[System.Serializable] // lets me save this class in a file
public class PlayerData
{
    public int level;
    public int health;
    public float[] position;

    public PlayerData(PlayerHealth player)
    {
        health = player.currentHealth;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[0] = player.transform.position.y;
        position[0] = player.transform.position.z;
    }
}
