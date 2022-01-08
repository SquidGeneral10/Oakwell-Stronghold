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
        public int maxHealth = 6;
        public int currentHealth;
        public Animator animator;
        public HealthPickup coin;
        public AudioSource pain;
        public PauseMenu pauseMenu;

        void Start()
        {
            currentHealth = maxHealth; // starts the game with full health
            healthBar.SetMaxHealth(maxHealth);
        }

        void Update()
        {
            if (currentHealth <= 0)
            { StartCoroutine(Dead()); }
        }

        public void TakeDamage(int damage)
        {
            pain.Play();
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
        }

        IEnumerator Dead()
        {
            animator.SetBool("PlayerDead", true); // Shows the 'dead' animation
            yield return new WaitForSeconds(1.3f); // You're dead - let that sink in. It's cold outside.

            pauseMenu.GameOver();
            Cursor.visible = true; // Makes the cursor visible.
            Cursor.lockState = CursorLockMode.None; // Allows the player to move their cursor around and click freely.
        }
    }
}

