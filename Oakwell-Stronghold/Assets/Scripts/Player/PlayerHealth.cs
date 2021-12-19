﻿#region 'Using' information
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {

        public Healthbar healthBar;
        public int maxHealth = 7;
        public int currentHealth;
        public Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            currentHealth = maxHealth; // starts the game with full health
            healthBar.SetMaxHealth(maxHealth);
        }

        // Update is called once per frame
        void Update()
        {
           if(Input.GetKeyDown(KeyCode.F))
           {
                TakeDamage(1);
           }

            if (currentHealth <= 0)
            {
                StartCoroutine(Dead());
            }
        }

        void TakeDamage(int damage)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
        }

        IEnumerator Dead()
        {
            animator.SetBool("PlayerDead", true); // Shows the 'dead' animation
            yield return new WaitForSeconds(1.3f); // You're dead - let that sink in. It's cold outside.

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // Loads the main menu.
            Cursor.visible = true; // Makes the cursor visible.
            Cursor.lockState = CursorLockMode.None; // Allows the player to move their cursor around and click freely.
        }
    }
}
