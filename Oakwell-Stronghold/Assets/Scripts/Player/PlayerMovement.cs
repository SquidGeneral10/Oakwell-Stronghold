#region 'Using' inofrmation
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public CharacterController2D controller;
        float horizontalMove = 0f;
        public float runSpeed = 14f;
        public PlayerHealth playerHealth;

        bool jump = false;
        bool fall = false;

        public Animator animator; // Lets me mess with animations through code.

        void Update()
        {
            if (playerHealth.currentHealth > 0)
            {
                Input.GetAxisRaw("Horizontal");
                horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

                animator.SetFloat("Speed", Mathf.Abs(horizontalMove)); // Makes sure the run animation plays whichever way you go (left or right).

                if (Input.GetButtonDown("Jump"))
                {
                    jump = true;
                    animator.SetBool("Jumping", true);
                }          
            }
        }

        public void OnLanding()
        {
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
        }

        public void OnFalling()
        {
            animator.SetBool("Falling", true);
            fall = true;
        }

        private void FixedUpdate()
        {
            if (playerHealth.currentHealth > 0)
            {
                controller.Move(horizontalMove * Time.fixedDeltaTime, jump); // When the player moves horizontally, they simply move in that direction without jumping.
                jump = false; // Stops the player from jumping forever and ever and ever and ever.
            }
            else
            {
                controller.Move(0 * 0, jump);
                runSpeed = 0f;
            }
        }
    }
}