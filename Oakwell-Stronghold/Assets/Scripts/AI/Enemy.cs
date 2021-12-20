#region 'Using' info
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

public class Enemy : MonoBehaviour
{
    public int enemyMaxHealth = 3;
    int enemyCurrentHealth;
    public AudioSource hurt; // The source of the squish sound effect that plays whenever the enemy's hit. drag it into the inspector.
    public AudioSource dead; // source of the dying sound effect that plays when the enemy conks out. drag into inspector

    public Animator animator; // Lets me mess with animations through code.
    bool enemyPunch = false;
    float enemyHorizontalMove = 0f;
    public Rigidbody2D rigidBody;

    private void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
        animator.GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetFloat("EnemySpeed", Mathf.Abs(enemyHorizontalMove)); // maybe use this to have the new skeleton run off.
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
        enemyPunch = true;
        StartCoroutine(EnemyPunchingAnimation()); // Animates the punch.
    }

    IEnumerator EnemyPunchingAnimation()
    {
        animator.SetBool("Punching", true);
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("Punching", false);
        enemyPunch = false;
    }
}