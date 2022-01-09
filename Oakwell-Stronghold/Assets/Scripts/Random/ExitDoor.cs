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

    public Enemy enemy1;
    public Enemy enemy2;
    public Enemy enemy3;
    [HideInInspector] public bool reachedExit = false;
    public BoxCollider2D exitBox;
    public AudioSource readyToGo; // door opening sound
    public AudioClip readyClip; // also door opening sound
    private bool alreadyPlayed = false;

    public AudioSource completeFanfare; // the little doo-doo-doo that plays after beating a level
    public AudioSource Music; // for turning off the gameplay music after beating a level
    

    private void Start()
    { spriteRenderer = gameObject.GetComponent<SpriteRenderer>(); }

    // Update is called once per frame
    void Update()
    {
        if (enemy1.Boned == true) // Only opens the door when all three enemies are killed. 3 enemies per level. 
            /// TODO:When enemy 2 and 3 are in, add "&& enemy2.Boned == true && enemy3.Boned == true"
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

    private void OnTriggerEnter2D(Collider2D collision) // when ya touch the pickup
    {
        Music.Stop();
        completeFanfare.Play();
        pauseMenu.LevelComplete();
        DataManager.Instance.SaveGame();
    }
}