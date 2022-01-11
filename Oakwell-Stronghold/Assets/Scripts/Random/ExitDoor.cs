#region 'Using' info
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
#endregion

public class ExitDoor : MonoBehaviour
{
    public PlayerHealth player;
    public PauseMenu pauseMenu;

    public SpriteRenderer spriteRenderer; // closed door sprite
    public Sprite newSprite; // opened door sprite

    public Enemy enemy1; // drag enemy1 in here, easiest enemy to reach
    public Enemy enemy2; // drag enemy2 in here in inspector
    public Enemy enemy3; // ditto above w/ enemy 3

    [HideInInspector] public bool reachedExit = false;
    public BoxCollider2D exitBox;
    public AudioSource readyToGo; // door opening sound
    public AudioClip readyClip; // also door opening sound
    private bool alreadyPlayed = false;

    public AudioSource completeFanfare; // the little doo-doo-doo that plays after beating a level
    public AudioSource Music; // for turning off the gameplay music after beating a level
    

    private void Start()
    { spriteRenderer = gameObject.GetComponent<SpriteRenderer>(); }

    void Update()
    {
        if (enemy1.Boned == true && enemy2.Boned == true && enemy3.Boned == true) // Only opens the door when all three enemies are killed. 3 enemies per level. 
        { ChangeSpriteBeginSound(); }
    }

    void ChangeSpriteBeginSound()
    {
        spriteRenderer.sprite = newSprite; // Changes the closed door sprite to an open door sprite.
        exitBox.enabled = true; // Lets the player touch the exit, thus enabling the OnTriggerEnter2D method.

        if(!alreadyPlayed) // only plays this sound effect once. removing this will cause audio spam.
        {
            readyToGo.PlayOneShot(readyClip);
            alreadyPlayed = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // when ya touch the door
    {
        if (DataManager.Instance.Level1Stars > 0)
        {
            DataManager.Instance.Level1Complete = true;
            DataManager.Instance.SaveGame();
        }

        if (DataManager.Instance.Level2Stars > 0)
        {
            DataManager.Instance.Level2Complete = true;
            DataManager.Instance.SaveGame();
        }

        if (DataManager.Instance.Level3Stars > 0)
        {
            DataManager.Instance.Level3Complete = true;
            DataManager.Instance.SaveGame();
        }

        if (DataManager.Instance.Level4Stars > 0)
        {
            DataManager.Instance.Level4Complete = true;
            DataManager.Instance.SaveGame();
        }

        if (DataManager.Instance.Level5Stars > 0)
        {
            DataManager.Instance.Level5Complete = true;
            DataManager.Instance.SaveGame();
        }


        Music.Stop(); // music stops
        completeFanfare.Play(); // happy music starts
        pauseMenu.LevelComplete(); // level's done
    }
}