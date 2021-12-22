#region 'Using' info
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

public class Enemy : MonoBehaviour
{
    #region Healthy bits
    public int enemyMaxHealth = 3;
    int enemyCurrentHealth;
    #endregion

    #region Patrolling bits
    public Transform rayCast;
    public Transform leftLimit;
    public Transform rightLimit;
    public LayerMask raycastMask;
    public float rayCastLength;
    public float attackDistance;

    private RaycastHit2D hit;
    private Transform target;
    private float distance; // the distance between enemy and player
    private bool attackMode;
    private bool inRange; // checks if player is in attack range
    private bool waiting;
    #endregion

    #region Other bits
    public float enemyHorizontalMove = 0f;
    public Rigidbody2D rigidBody;
    #endregion

    #region Fighty bits
    public int attackDamage = 1; 
    public float attackTimer; // Timer for cooldown between attacks
    private float internalTimer;
    #endregion

    #region Sounds and Animatey Bits
    public Animator animator; // Lets me mess with animations through code.
    public AudioSource hurt; // The source of the squish sound effect that plays whenever the enemy's hit. drag it into the inspector.
    public AudioSource dead; // source of the dying sound effect that plays when the enemy conks out. drag into inspector
    #endregion

    private void Start()
    {
        SelectTarget();
        enemyCurrentHealth = enemyMaxHealth;
        internalTimer = attackTimer; // Store the inital value of timer
        animator.GetComponent<Animator>();
    }

    void Update()
    {
        if(!attackMode)
        {
            animator.SetFloat("EnemySpeed", Mathf.Abs(enemyHorizontalMove)); // maybe use this to have the new skeleton run off.
            Move();
        }
        
        if(!InsideofLimits() && !inRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("Punching"))
        {

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
            animator.SetBool("canMove", false);
            animator.SetFloat("EnemySpeed", Mathf.Abs(0)); // Makes sure the run animation plays whichever way you go (left or right).
            
            StopAttack();
        }
    }

    private void OnTriggerEnter2D(Collider2D trig)
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
        else if (attackDistance >= distance && waiting == false)
        {
            Attack();
        }

        if (waiting)
        {
            BingChilling();
            animator.SetBool("Punching", false);
        }
    }

    void Move()
    {
        animator.SetBool("canMove", true);
        animator.SetFloat("EnemySpeed", Mathf.Abs(enemyHorizontalMove)); // Makes sure the run animation plays whichever way you go (left or right).

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Punching"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, enemyHorizontalMove * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        enemyCurrentHealth -= damage;  // calls in the PlayerAttack script's damage number
        hurt.Play(); // plays the hurt sound

        if(enemyCurrentHealth <=0) // if his health is equal to or below 0...
        { Die(); } // dead lol
    }

    void Die()
    {
        animator.SetBool("EnemyDie", true); // shows the dead animation
        rigidBody.simulated = false; // Stops the body from sliding away.
        dead.Play();
        GetComponent<BoxCollider2D>().enabled = false; // makes the enemy fall through the floor upon death, hmm
        enabled = false;
    }

    void Attack()
    {
        attackTimer = internalTimer;
        StartCoroutine(EnemyPunchingAnimation()); // Animates the punch.
    }

    IEnumerator EnemyPunchingAnimation()
    {
        animator.SetBool("canMove", false);
        animator.SetBool("Punching", true);
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("Punching", false);
    }

    void StopAttack() // For making the enemy stop attacking when the player's out of their range.
    {
        waiting = false;
        attackMode = false;
        animator.SetBool("Punching", false);
    }

    void BingChilling()
    {    
        
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0 && waiting && attackMode)
        {
            waiting = false;
            attackTimer = internalTimer;
        }
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

    public void HoldYourHorses()
    {
        waiting = true;
    }

    private bool InsideofLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    private void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if(distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }

        Flip();
    }

    private void Flip()
    {
        Vector3 rotation = transform.eulerAngles;

        if(transform.position.x > target.position.y)
        {
            rotation.y = 180f; // Flips the enemy around when they've reached the edge of their patrol route.
        }
        else
        {
            rotation.y = 0f;
        }

        transform.eulerAngles = rotation;
    }
}