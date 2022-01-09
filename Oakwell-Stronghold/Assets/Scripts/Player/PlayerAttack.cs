#region 'Using' information
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

public class PlayerAttack : MonoBehaviour
{
    public Animator animator; // Allows me to mess with animations through code.
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 0.5f;    
    public int attackDamage = 1; // Default damage is 1, leaves the door open for upgrades :P
    public float attackRate = 2f; // Number of times you can attack in every second
    float nextAttackTime = 0f;

    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Punch"))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    public void Attack()
    {
        if (Input.GetButtonDown("Punch"))
        { StartCoroutine(PunchingAnimation()); } // Animates the punch.

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers); // Looks for enemies in the circle

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage); // default attack damage (at the top) is 1, leaves door open for upgrades :P
        }
    }

    IEnumerator PunchingAnimation()
    {
        animator.SetBool("Punching", true);
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("Punching", false);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
