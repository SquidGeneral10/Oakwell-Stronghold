#region 'Using' info
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

public class Enemy : MonoBehaviour
{
    public int maxHealth = 3;
    int currentHealth;
    public Animator animator; // Lets me mess with animations through code.
    bool enemyPunch = false;
    float enemyHorizontalMove = 0f;

    private void Start()
    {
        animator.GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetFloat("EnemySpeed", Mathf.Abs(enemyHorizontalMove)); // maybe use this to have the new skeleton run off.
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth >=0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("EnemyDie", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
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