#region 'Using' info
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
#endregion

public class Enemy : MonoBehaviour
{
    #region Public Variables
    public Transform rayCast;
    public LayerMask raycastMask;
    public float rayCastLength;
    public float attackDistance; //Minimum distance for attack
    public float moveSpeed;
    public float timer; //Timer for cooldown between attacks
    public Transform leftLimit;
    public Transform rightLimit;
    public int enemyMaxHealth = 3;
    public Rigidbody2D rigidBody;
    public Animator animator; // Lets me mess with animations through code.
    public AudioSource hurt; // The source of the squish sound effect that plays whenever the enemy's hit. drag it into the inspector.
    public AudioSource dead; // source of the dying sound effect that plays when the enemy conks out. drag into inspector

    public Transform attackPoint;
    public float attackRange = 0.5f;
    #endregion

    #region Private Variables
    private RaycastHit2D hit;
    private Transform target;
    private Animator anim;
    private float distance; //Store the distance b/w enemy and player
    private bool attackMode;
    private bool inRange; //Check if Player is in range
    private bool cooling; //Check if Enemy is cooling after attack
    private float intTimer;
    private int enemyCurrentHealth;
    private int attackDamage = 1;
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
        {
            Move();
        }

        if (!InsideOfLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Punching"))
        {
            SelectTarget();
        }

        if (inRange)
        {
            hit = Physics2D.Raycast(rayCast.position, transform.right, rayCastLength, raycastMask);
            RaycastDebugger();
        }

        //When Player is detected
        if (hit.collider != null)
        {
            EnemyLogic();
        }
        else if (hit.collider == null)
        {
            inRange = false;
        }

        if (inRange == false)
        {
            StopAttack(); // The enemy stops attacking when the player's totally out of their range.
        }
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {
            target = trig.transform;
            inRange = true;
            Flip();
        }
    }

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
            StopAttack();
        }
        else if (attackDistance >= distance && cooling == false)
        {
            Attack();
        }

        if (cooling)
        {
            Cooldown();
            anim.SetBool("Punching", false);
        }
    }

    void Move()
    {
        anim.SetBool("canMove", true);
        animator.SetFloat("EnemySpeed", Mathf.Abs(moveSpeed)); // maybe use this to have the new skeleton run off.

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Punching"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void Attack()
    {
        timer = intTimer; // Reset Timer when Player enter Attack Range
        attackMode = true; // To check if Enemy can still attack or not

        anim.SetBool("canMove", false);
        anim.SetBool("Punching", true);

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, raycastMask); // Looks for enemies in the circle

        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(attackDamage); // default attack damage is 1
            Debug.Log(player.name + "hit by enemy");
        }

        
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;
        animator.SetFloat("EnemySpeed", Mathf.Abs(0)); // Forces the idle animation to play while cooling down from an attack.

        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
            animator.SetFloat("EnemySpeed", Mathf.Abs(moveSpeed)); // maybe use this to have the new skeleton run off.
        }
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("Punching", false);
    }

    void RaycastDebugger()
    {
        if (distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.red);
        }
        else if (attackDistance > distance)
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.green);
        }
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

    private bool InsideOfLimits() // Sends the enemy back to their starting position if you leave their aggro range.
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    private void SelectTarget()
    {
        float distanceToLeft = Vector3.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector3.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }

        //Ternary Operator
        //target = distanceToLeft > distanceToRight ? leftLimit : rightLimit;

        Flip();
    }

    void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
        {
            rotation.y = 180;
        }
        else
        {
            rotation.y = 0;
        }

        //Ternary Operator
        //rotation.y = (currentTarget.position.x < transform.position.x) ? rotation.y = 180f : rotation.y = 0f;

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
        animator.SetBool("EnemyDie", true); // shows the dead animation
        rigidBody.simulated = false; // Stops the body from sliding away.
        GetComponent<BoxCollider2D>().enabled = false;
        enabled = false; // Disables this script when the enemy's dead. make sure this line is on the bottom, nothing below it is run
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}