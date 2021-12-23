#region 'Using' info
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
    public GameObject hitBox; // An empty with a boxcollider2d attached - will hopefully be used to hurt the player if they're in it and the enemy is punching
    public int enemyMaxHealth = 3; // Enemy's maximum health. Can be modified in-editor.
    public Rigidbody2D rigidBody; // Moves the enemy. Disable to stop it from moving.
    public AudioSource hurt; // The source of the squish sound effect that plays whenever the enemy's hit.
    public AudioSource dead; // source of the dying sound effect that plays when the enemy conks out. 
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange; // Check if Player is in range
    public GameObject aggroZone; // Larger box - if the player is in it after being chased, the enemy continues chasing them.
    public GameObject triggerArea; // Smaller box - if the player is in it, the enemy begins chasing them.
    public int attackDamage = 1; // The damage that the enemy does to the player.
    public LayerMask playerLayer;
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
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance)
        { StopAttack(); }

        else if (attackDistance >= distance && cooling == false)
        { Attack(); }

        if (cooling)
        {
            Cooldown();
            anim.SetBool("Punching", false);
        }
    }

    void Move()
    {
        anim.SetBool("canMove", true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Punching"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void Attack()
    {
        timer = intTimer; 
        attackMode = true;
        anim.SetBool("canMove", false);
        anim.SetBool("Punching", true);

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(hitBox.transform.position, attackDistance, playerLayer); // Looks for player in the circle

        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(attackDamage); // default attack damage (at the top) is 1, leaves door open for upgrades :P
            Debug.Log("Whack!");
        }
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

    void StopAttack()
    {
        cooling = false;
        attackMode = false;        
        anim.SetBool("Punching", false);
        hitBox.SetActive(false);
    }

    public void TriggerCooling()
    { cooling = true; }

    private bool InsideOfLimits() // Sends the enemy back to their starting position if you leave their aggro range.
    { return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x; }

    public void SelectTarget()
    {
        float distanceToLeft = Vector3.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector3.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight)
        { target = leftLimit; }
        else
        { target = rightLimit; }

        Flip();
    }

    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
        { rotation.y = 180; }
        else
        { rotation.y = 0; }

        transform.eulerAngles = rotation;
    }

    public void TakeDamage(int damage)
    {
        enemyCurrentHealth -= damage;  // calls in the PlayerAttack script's damage number
        hurt.Play(); // plays the hurt sound

        if (enemyCurrentHealth <= 0) // if his health is equal to or below 0...
        { Die(); } // dead lol
    }

    void Die()
    {
        dead.Play();
        anim.SetBool("EnemyDie", true); // shows the dead animation
        rigidBody.simulated = false; // Stops the body from sliding away.
        GetComponent<BoxCollider2D>().enabled = false;
        enabled = false; // Disables this script when the enemy's dead. make sure this line is on the bottom, nothing below it is run
    }

    void StopKillingMe()
    {
        hitBox.SetActive(false);
    }
}