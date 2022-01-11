#region 'Using' information
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
#endregion

public class Enemy : MonoBehaviour
{
    #region Public Variables

    public float attackDistance; // The closest the player needs to be for the enemy to swing.
    public float moveSpeed; // The enemy's movement speed. Modify in-editor.
    public float timer; // Timer for cooldown between attacks
    public Transform leftLimit; // The left side of the enemy's patrol route.
    public Transform rightLimit; // The right side of the enemy's patrol route.
    public GameObject hitBox; // An empty object with a boxcollider2d attached - will hurt the player IF they're in it AND the enemy is punching
    public int enemyMaxHealth = 3; // Enemy's maximum health. Can be modified in-editor.
    public Rigidbody2D rigidBody; // Moves the enemy. Disable to stop it from moving.
    public AudioSource hurt; // The source of the squish sound effect that plays whenever the enemy's hit.
    public AudioSource dead; // source of the dying sound effect that plays when the enemy conks out. 
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange; // Check if Player is in range
    public GameObject aggroZone; // Larger box - if the player is in it after being chased, the enemy continues chasing them.
    public GameObject triggerArea; // Smaller box - if the player is in it, the enemy begins chasing them.
    public int attackDamage = 1; // The damage that the enemy does to the player.
    public LayerMask playerLayer; // Makes sure the enemy only chases the PLAYER (because it's the only thing on the player layer)
    [HideInInspector] public bool Boned; // Used in the ExitDoor script to ensure all enemies are dead before unlocking the exit door

    #endregion

    #region Private Variables

    private Animator anim; // Lets me mess with animations through code.
    private float distance; //Stores the distance between enemy and player
    private bool attackMode; // Yes / no - is the enemy attacking?
    private bool cooling; // Check if Enemy is 'cooling' (waiting) after attack
    private float intTimer; // Time between attacks.
    private int enemyCurrentHealth; // the enemy's current health - if it's equal to or below zero, they're having a reeeal bad time.

    #endregion

    void Awake()
    {
        SelectTarget();
        intTimer = timer; //Store the inital value of timer
        anim = GetComponent<Animator>();
        enemyCurrentHealth = enemyMaxHealth;
    }

    void Update()
    {
        if (!attackMode)
        { Move(); }

        if (!InsideOfLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Punching"))
        { SelectTarget(); }      

        if (inRange)
        { EnemyLogic(); } // The enemy stops attacking when the player's totally out of their range.
    }

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position); // Constantly checks where the enemy is - useful for setting patrol boundaries and checking distance between enemy and player.

        if (distance > attackDistance)
        { StopAttack(); }

        else if (attackDistance >= distance && cooling == false)
        { Attack(); }

        if (cooling)
        { Cooldown(); }
    }

    void Move() // As you'd expect, this method makes the enemy move.
    {
        anim.SetBool("canMove", true); // TODO: Figure out why the first two frames of the moving animation are the only ones playing. Maybe it's trying to play more than one animation at once?

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Punching")) // Moves the enemy, as long as they aren't already punching the player.
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void Attack() // When the player's in the aggro zone, this method is responsible for bringing the hurt.
    {
        timer = intTimer; 
        attackMode = true;
        anim.SetBool("canMove", false);
        { StartCoroutine(PunchingAnimation()); }
        TriggerCooling();
        hitBox.SetActive(true);

        if (hitBox.activeInHierarchy)
        {
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(hitBox.transform.position, attackDistance, playerLayer); // Looks for player in the circle

            foreach (Collider2D player in hitPlayer) // hurts the player when they're in the circle
            {
                player.GetComponent<PlayerHealth>().TakeDamage(attackDamage); // default attack damage (at the top) is 1, leaves door open for upgrades :P
            }
        }
    }

    IEnumerator PunchingAnimation()
    {
        anim.SetBool("Punching", true);
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("Punching", false);
    }

    void Cooldown() // Enemies will have a short 'rest' in between punches.
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    void StopAttack() // When the player's successfully left the aggro zone, this method is responsible for un-triggering all their attack stuff.
    {
        cooling = false;
        attackMode = false;        
        anim.SetBool("Punching", false);
        hitBox.SetActive(false);
    }

    public void TriggerCooling() // Called by the enemy's punch animation; makes sure they're waiting for a second or two between their attacks.
    { cooling = true; }

    private bool InsideOfLimits() // Sends the enemy back to their starting position if you leave their aggro range.
    { return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x; }

    public void SelectTarget() // Responsible for making the enemy walk left and right.
    {
        float distanceToLeft = Vector3.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector3.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight)
        { target = leftLimit; }
        else
        { target = rightLimit; }

        Flip();
    }

    public void Flip() // Flips the enemy sprite 180 degrees whenever they need to turn around.
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
        { rotation.y = 180; }
        else
        { rotation.y = 0; }

        transform.eulerAngles = rotation;
    }

    public void TakeDamage(int damage) // Hurts the enemy. Most will only have 3 health.
    {
        enemyCurrentHealth -= damage;  // calls in the PlayerAttack script's damage number
        hurt.Play(); // plays the hurt sound

        if (enemyCurrentHealth <= 0) // if enemy's health is equal to or below 0...
        { Die(); } // they're dead lol
    }

    IEnumerator Skelefied()
    {
        yield return new WaitForSeconds(1f); // Waits a sec so the player can understand the consequences of their actions. They killed a man. Pretty messed up, dawg.
        anim.SetBool("EnemyDie", false); // prevents the death animation from playing any more
        anim.SetBool("EnemyBoned", true); // Turns the dude into a skeleton
        Boned = true; // This enemy becomes one of the three boned boys required to get the exit door open.
    }

    void Die() 
    {
        dead.Play(); // Plays the enemy death sound
        anim.SetBool("EnemyDie", true); // shows the enemy's death animation
        StartCoroutine(Skelefied()); // Begins the coroutine that will swap his sprite out with that of a skeleton.
        rigidBody.simulated = false; // Stops the body from sliding away.
        GetComponent<BoxCollider2D>().enabled = false; // Turns off the enemy's hitbox.
        enabled = false; // Disables this script when the enemy's dead. make sure this line is on the bottom of the method, nothing below it is run
    }
}